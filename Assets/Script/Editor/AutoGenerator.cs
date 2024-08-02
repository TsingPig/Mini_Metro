using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TsingPigSDK
{
    public class AutoGenerator : Editor
    {
        private const string fileName = "Str";

        [MenuItem("我的工具/自动生成AddressableConfig #W")]
        public static void AddressableAutoGenView()
        {
            Object selectedObject = Selection.activeObject;
            if(selectedObject != null && selectedObject is GameObject)
            {
                GameObject prefabObj = (GameObject)selectedObject;

                string objectName = prefabObj.name;

                GenerateCode(objectName);
            }
            else
            {
                Log.Warning("请选择一个预制体再生成代码");
            }
        }

        [MenuItem("我的工具/自动生成AddressableConfig(View视图) #V")]
        public static void AddressableAutoGen()
        {
            Object selectedObject = Selection.activeObject;
            if(selectedObject != null && selectedObject is GameObject)
            {
                GameObject prefabObj = (GameObject)selectedObject;

                string objectName = prefabObj.name;

                GenerateCode(objectName, true);
            }
            else
            {
                Log.Warning("请选择一个预制体再生成代码");
            }
        }

        private static void GenerateCode(string objectName, bool isView = false)
        {
            string scriptPath = $"Assets/Script/Config/{fileName}.cs";

            string codeLine = $"    public const string {Split(objectName)}_DATA_PATH = \"{objectName}\";";

            if(!File.Exists(scriptPath))
            {
                using(StreamWriter writer = File.CreateText(scriptPath))
                {
                    writer.WriteLine($"public static class {fileName}");
                    writer.WriteLine("{");
                    writer.WriteLine(codeLine);
                    writer.WriteLine("}");
                    Log.Info($"创建脚本: {scriptPath}");
                }
            }
            else
            {
                string[] contexts = File.ReadAllLines(scriptPath);
                if(!contexts.Contains(codeLine))
                {
                    contexts[contexts.Length - 1] = codeLine + "\n}";
                    File.WriteAllLines(scriptPath, contexts);
                    Log.Info($"增加代码:{codeLine} 到 {scriptPath}");
                }
                else
                {
                    Log.Warning($"{scriptPath} 已经存在{codeLine}");
                }
            }

            if(isView)
            {
                objectName = objectName.Substring(0, objectName.IndexOf("View"));

                string folderPath = $"Assets/Script/UI/{objectName}View";

                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                // 生成 INameObjPresenter.cs
                string presenterCode = $"using MVPFrameWork;\n" +
                    $"public interface I{objectName}Presenter : IPresenter {{}}";
                File.WriteAllText(Path.Combine(folderPath, $"I{objectName}Presenter.cs"), presenterCode);

                // 生成 INameView.cs
                string viewCode = $"using MVPFrameWork;\n" +
                    $"public interface I{objectName}View : IView {{}}";
                File.WriteAllText(Path.Combine(folderPath, $"I{objectName}View.cs"), viewCode);

                // 生成 NamePresenter.cs
                string presenterImplCode = $"using MVPFrameWork;\n" +
                    $"public class {objectName}Presenter : PresenterBase<I{objectName}View>, I{objectName}Presenter {{}}";

                File.WriteAllText(Path.Combine(folderPath, $"{objectName}Presenter.cs"), presenterImplCode);

                // 生成 NameView.cs
                string viewImplCode = $"using MVPFrameWork;\n" +
                                      $"[ParentInfo(FindType.FindWithName, Str.CANVAS)]\n" +
                                      $"public class {objectName}View : ViewBase<I{objectName}Presenter>, I{objectName}View {{ protected override void OnCreate() {{ throw new System.NotImplementedException(); }} }}";

                File.WriteAllText(Path.Combine(folderPath, $"{objectName}View.cs"), viewImplCode);
            }

            AssetDatabase.Refresh();
        }

        private static string Split(string name)
        {
            return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper();
        }
    }
}