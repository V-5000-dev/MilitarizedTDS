// using UnityEngine;
// using UnityEditor;

// public class TagDuplicator : EditorWindow
// {
//     GameObject objectToDuplicate;
//     string targetTag = "Untagged"; // Default Unity tag

//     static void Init()
//     {
//         TagDuplicator window = (TagDuplicator)GetWindow(typeof(TagDuplicator));
//         window.titleContent = new GUIContent("Tag Duplicator");
//         window.Show();
//     }

//     void OnGUI()
//     {
//         GUILayout.Label("Duplicate to All Objects With a Tag", EditorStyles.boldLabel);

//         // Drag-and-drop field to choose what to copy
//         objectToDuplicate = (GameObject)EditorGUILayout.ObjectField("Object To Duplicate", objectToDuplicate, typeof(GameObject), false);

//         // Input field for tag
//         targetTag = EditorGUILayout.TagField("Target Tag", targetTag);

//         if (GUILayout.Button("Duplicate to All Tagged Objects"))
//         {

//             GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

//             foreach (GameObject obj in taggedObjects)
//             {
//                 GameObject clone = (GameObject)PrefabUtility.InstantiatePrefab(objectToDuplicate);
//                 Undo.RegisterCreatedObjectUndo(clone, "Duplicate Object");
//                 clone.transform.position = obj.transform.position;
//                 float randomY = Random.Range(0f, 360f);
//                 clone.transform.rotation = Quaternion.Euler(0, randomY, 0);
//                 clone.transform.SetParent(obj.transform);
//                 clone.SetActive(false);
//             }

//             Debug.Log($"Duplicated to {taggedObjects.Length} object(s) with tag '{targetTag}'");
//         }
//     }
// }
