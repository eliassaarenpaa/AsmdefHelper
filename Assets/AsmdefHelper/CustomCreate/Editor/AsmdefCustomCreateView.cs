using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AsmdefHelper.CustomCreate.Editor {
    // original: https://github.com/baba-s/UniAssemblyDefinitionCreator
    public class AsmdefCustomCreateView : EditorWindow {
        [MenuItem("Assets/Create Assembly")]
        public static void ShowWindow() {
            var asset = Selection.activeObject;
            var assetPath = AssetDatabase.GetAssetPath(asset);
            var directory = string.IsNullOrWhiteSpace(assetPath) ? "Assets/" : assetPath;
            var defaultName = directory.Replace("Assets/", "").Replace('/', '.');

            var asmdef = new AssemblyDefinitionJson {
                name = defaultName,
#if UNITY_2020_2_OR_NEWER
                rootNamespace = string.Empty,
#endif
                allowUnsafeCode = false,
                autoReferenced = false,
                overrideReferences = false,
                noEngineReferences = false,
                includePlatforms = Array.Empty<string>()
            };
            var asmdefJson = JsonUtility.ToJson(asmdef, true);
            var asmdefPath = $"{directory}/{defaultName}.asmdef";
            File.WriteAllText(asmdefPath, asmdefJson, Encoding.UTF8);
            AssetDatabase.Refresh();
        }
    }
}
