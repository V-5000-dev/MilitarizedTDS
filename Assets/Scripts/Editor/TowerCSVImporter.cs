using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class TowerCSVImporter
{
    private const string CsvFolder = "Assets/Data/TowerCSV";
    private const string AssetOutputFolder = "Assets/Prefabs/Towers";

    [MenuItem("Tools/Import Tower CSVs")]
    public static void ImportAll()
    {
        string[] csvFiles = Directory.GetFiles(Application.dataPath + "/Data/TowerCSV", "*.csv");

        if (csvFiles.Length == 0)
        {
            EditorUtility.DisplayDialog("Import Tower CSVs", $"No CSV files found in {CsvFolder}", "OK");
            return;
        }

        // Load all TowerClass assets once, keyed by towerName
        string[] guids = AssetDatabase.FindAssets("t:TowerClass");
        var assetsByName = new Dictionary<string, TowerClass>(StringComparer.OrdinalIgnoreCase);
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var tc = AssetDatabase.LoadAssetAtPath<TowerClass>(path);
            if (tc != null && !string.IsNullOrEmpty(tc.towerName))
                assetsByName[tc.towerName.Trim()] = tc;
        }

        int updated = 0;
        var log = new System.Text.StringBuilder();

        foreach (string csvPath in csvFiles)
        {
            string[] lines = File.ReadAllLines(csvPath);
            if (lines.Length < 2) continue;

            // Parse header row
            string[] headers = ParseCSVLine(lines[0]);
            var colIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < headers.Length; i++)
                colIndex[headers[i].Trim()] = i;

            // Collect ordered chain for this CSV: (sortKey, TowerClass)
            var chain = new List<(int sortKey, TowerClass tc)>();

            // Process data rows
            for (int row = 1; row < lines.Length; row++)
            {
                if (string.IsNullOrWhiteSpace(lines[row])) continue;
                string[] cols = ParseCSVLine(lines[row]);

                string name = Get(cols, colIndex, "Tower Name");
                if (string.IsNullOrWhiteSpace(name)) continue;

                if (!assetsByName.TryGetValue(name.Trim(), out TowerClass tc))
                {
                    tc = ScriptableObject.CreateInstance<TowerClass>();
                    tc.towerName = name.Trim();
                    string safeName = name.Trim().Replace("/", "-").Replace("\\", "-");
                    string assetPath = $"{AssetOutputFolder}/{safeName}.asset";
                    AssetDatabase.CreateAsset(tc, assetPath);
                    assetsByName[name.Trim()] = tc;
                    log.AppendLine($"  CREATED: {name}");
                    updated++;
                }
                else
                {
                    log.AppendLine($"  OK: {name}");
                    updated++;
                }

                ApplyRow(tc, cols, colIndex);
                EditorUtility.SetDirty(tc);

                int sortKey = ParseTowerLevel(Get(cols, colIndex, "Tower Level"));
                chain.Add((sortKey, tc));
            }

            // Sort chain by level order and wire up nextTier + towerIcon
            chain.Sort((a, b) => a.sortKey.CompareTo(b.sortKey));
            for (int i = 0; i < chain.Count; i++)
            {
                TowerClass tc = chain[i].tc;

                // Assign rank sprite (TowerLevel0 = Base, TowerLevel1 = Upgrade 1, …)
                string spritePath = $"Assets/TowerRanks/TowerLevel{i}.png";
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (sprite != null)
                    tc.towerIcon = sprite;

                // Link to next tier and set upgrade cost
                if (i + 1 < chain.Count)
                {
                    TowerClass next = chain[i + 1].tc;
                    tc.nextTier = next;
                    tc.upgradeCost = next.cost;
                }
                else
                {
                    tc.nextTier = null;
                }

                EditorUtility.SetDirty(tc);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        string summary = $"Done. {updated} updated.\n\n{log}";
        Debug.Log("[TowerCSVImporter] " + summary);
        EditorUtility.DisplayDialog("Import Tower CSVs", summary, "OK");
    }

    private static void ApplyRow(TowerClass tc, string[] cols, Dictionary<string, int> idx)
    {
        tc.towerDisc            = Get      (cols, idx, "Tower Disc",         tc.towerDisc);
        tc.damage               = GetFloat (cols, idx, "Damage",             tc.damage);
        tc.armorPen             = GetInt   (cols, idx, "AP",                 tc.armorPen);
        tc.fireRate             = GetFloat (cols, idx, "RPS",                tc.fireRate);
        tc.minRange             = GetFloat (cols, idx, "Min Range",          tc.minRange);
        tc.range                = GetFloat (cols, idx, "Range",              tc.range);
        tc.accuracy             = GetFloat (cols, idx, "Acc %",              tc.accuracy);
        tc.cost                 = GetInt   (cols, idx, "$",                  tc.cost);
        tc.rankUnlock           = GetInt   (cols, idx, "Rank Unlock",        tc.rankUnlock);
        tc.magSize              = GetInt   (cols, idx, "Mag size",           tc.magSize);
        tc.reloadSpeed          = GetFloat (cols, idx, "Reload speed",       tc.reloadSpeed);
        tc.splashRange          = GetFloat (cols, idx, "Splash Range",       tc.splashRange);
        tc.spashDamage          = GetFloat (cols, idx, "Splash DMG",         tc.spashDamage);
        tc.overTimeDmg          = GetFloat (cols, idx, "DMG over Time",      tc.overTimeDmg);
        tc.overTimeDuration     = GetFloat (cols, idx, "Time",               tc.overTimeDuration);
        tc.critDamage           = GetFloat (cols, idx, "Crit DMG",           tc.critDamage);
        tc.critChance           = GetFloat (cols, idx, "Crit %",             tc.critChance);
        tc.critOverTimeDmg      = GetFloat (cols, idx, "Crit DMG over Time", tc.critOverTimeDmg);
        tc.critOverTimeDuration = GetFloat (cols, idx, "Crit Time",          tc.critOverTimeDuration);
        tc.critSplashRange      = GetFloat (cols, idx, "Crit Splash R",      tc.critSplashRange);
        tc.critSplashDamage     = GetFloat (cols, idx, "Crit Splash DMG",    tc.critSplashDamage);
    }

    // "Base" → 0, "Upgrade 1" → 1, "Upgrade 2" → 2, etc.
    private static int ParseTowerLevel(string level)
    {
        if (string.IsNullOrWhiteSpace(level)) return 0;
        level = level.Trim();
        if (level.Equals("Base", StringComparison.OrdinalIgnoreCase)) return 0;
        // "Upgrade N"
        var parts = level.Split(' ');
        if (parts.Length >= 2 && int.TryParse(parts[^1], out int n))
            return n;
        return 0;
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private static string Get(string[] cols, Dictionary<string, int> idx, string header, string fallback = "")
    {
        if (!idx.TryGetValue(header, out int i) || i >= cols.Length) return fallback;
        string v = cols[i].Trim();
        return string.IsNullOrEmpty(v) ? fallback : v;
    }

    private static float GetFloat(string[] cols, Dictionary<string, int> idx, string header, float fallback = 0f)
    {
        string v = Get(cols, idx, header);
        return float.TryParse(v, System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out float r) ? r : fallback;
    }

    private static int GetInt(string[] cols, Dictionary<string, int> idx, string header, int fallback = 0)
    {
        string v = Get(cols, idx, header);
        return int.TryParse(v, out int r) ? r : fallback;
    }

    // Handles quoted fields with commas inside
    private static string[] ParseCSVLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        var current = new System.Text.StringBuilder();

        foreach (char c in line)
        {
            if (c == '"') { inQuotes = !inQuotes; continue; }
            if (c == ',' && !inQuotes) { result.Add(current.ToString()); current.Clear(); continue; }
            current.Append(c);
        }
        result.Add(current.ToString());
        return result.ToArray();
    }
}
