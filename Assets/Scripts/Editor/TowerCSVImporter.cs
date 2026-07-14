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
        tc.towerDisc    = Get(cols, idx, "Tower Disc", tc.towerDisc);
        tc.damage       = GetFloat(cols, idx, "Damage",       tc.damage);
        tc.fireRate     = GetFloat(cols, idx, "RPS",          tc.fireRate);
        tc.range        = GetFloat(cols, idx, "Range",        tc.range);
        tc.cost         = GetInt  (cols, idx, "$",            tc.cost);
        tc.magSize      = GetInt  (cols, idx, "Mag size",     tc.magSize);
        tc.reloadSpeed  = GetFloat(cols, idx, "Reload speed", tc.reloadSpeed);
        tc.splashRange  = GetFloat(cols, idx, "Splash Range", tc.splashRange);
        tc.spashDamage  = GetFloat(cols, idx, "Splash DMG",   tc.spashDamage);
        tc.overTimeDmg  = GetFloat(cols, idx, "DMG over Time",   tc.overTimeDmg);
        tc.overTimeDuration     = GetFloat(cols, idx, "Time",               tc.overTimeDuration);
        tc.critDamage           = GetFloat(cols, idx, "Crit DMG",        tc.critDamage);
        tc.critChance           = GetFloat(cols, idx, "Crit %",          tc.critChance);
        tc.critOverTimeDmg      = GetFloat(cols, idx, "Crit DMG over Time", tc.critOverTimeDmg);
        tc.critOverTimeDuration = GetFloat(cols, idx, "Crit Time",          tc.critOverTimeDuration);
        tc.critSplashRange      = GetFloat(cols, idx, "Crit Splash R",   tc.critSplashRange);
        tc.critSplashDamage     = GetFloat(cols, idx, "Crit Splash DMG", tc.critSplashDamage);
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
