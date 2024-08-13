using System.Collections.Generic;
using System.Linq;
using AsmdefHelper.UnityInternal;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AsmdefHelper.MultipleEdit.Editor {
    public class AsmdefMultiEditWindow : EditorWindow {
        static IList<InspectorWindowWrapper> windows = new List<InspectorWindowWrapper>();

        [MenuItem("Assembly Definitions/Find All Assembly Definitions In Project")]
        public static void Search() {
            var browser = CreateInstance<ProjectBrowserWrapper>();
            browser.GetProjectBrowser();
            browser.SetSearch("t:AssemblyDefinitionAsset");
        }

        [MenuItem("Assembly Definitions/Open Selected Assembly Definition In Inspector View")]
        [MenuItem("Assets/Open Selected Assembly")]
        public static void Open() {
            var asmdefs = Selection.GetFiltered(typeof(AssemblyDefinitionAsset), SelectionMode.TopLevel);
            if (!asmdefs.Any()) {
                Debug.Log("no AssemblyDefinitionAsset");
                return;
            }

            CloseWindows();
            foreach (var adf in asmdefs) {
                Selection.objects = new[] { adf };
                var w = CreateInstance<InspectorWindowWrapper>();
                w.GetInspectorWindow();
                // LockすることでInspectorWindowの表示を固定する
                w.Lock(true);
                windows.Add(w);
            }
        }

        [MenuItem("Assembly Definitions/Apply All Assembly Definitions & Close")]
        public static void Apply() {
            foreach (var w in windows) {
                w.AllApply();
                w.CloseInspectorWindow();
            }
            windows.Clear();
        }

        static void CloseWindows() {
            foreach (var w in windows) {
                w.CloseInspectorWindow();
            }
            windows.Clear();
        }
    }
}
