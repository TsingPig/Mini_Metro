using System.IO;
using UnityEditor;
using UnityEngine;

namespace TsingPigSDK
{
    public class ScriptableObjectExporter : Editor
    {
        [MenuItem("我的工具/SO导出Json #E")]
        public static void ExportScriptableObjectToJson()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject != null && selectedObject is ScriptableObject)
            {
                ScriptableObject scriptableObject = (ScriptableObject)selectedObject;
                string outputPath = AssetDatabase.GetAssetPath(scriptableObject).Replace(".asset", ".json");
                string jsonData = JsonUtility.ToJson(scriptableObject, true);
                File.WriteAllText(outputPath, jsonData);
                Debug.Log("ScriptableObject exported to JSON: " + outputPath);
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogWarning("Select a ScriptableObject asset to export.");
            }
        }

        [MenuItem("我的工具/Json导入SO #I")]
        public static void ImportJsonToScriptableObject()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject != null && selectedObject is TextAsset)
            {
                TextAsset jsonAsset = (TextAsset)selectedObject;
                string scriptableObjectPath = AssetDatabase.GetAssetPath(jsonAsset).Replace(".json", ".asset");
                ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(scriptableObjectPath);

                if (scriptableObject != null)
                {
                    JsonUtility.FromJsonOverwrite(jsonAsset.text, scriptableObject);
                    Debug.Log("JSON imported to ScriptableObject: " + scriptableObjectPath);
                    AssetDatabase.Refresh();
                }
                else
                {
                    Debug.LogWarning("No matching ScriptableObject found for the selected JSON.");
                }
            }
            else
            {
                Debug.LogWarning("Select a JSON asset to import.");
            }
        }
    }
}
