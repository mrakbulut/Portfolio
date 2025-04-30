using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Portfolio.SceneLoad.Editor
{
    [CustomEditor(typeof(SceneLoader))]
    public class SceneLoaderEditor : OdinEditor
    {
        private static List<string> _sceneNames = new List<string>();
        private static List<string> _sceneUINames = new List<string>();

        protected override void OnEnable()
        {
            base.OnEnable();

            CacheSceneNamesByIndexes();
        }

        private void CacheSceneNamesByIndexes()
        {
            _sceneNames.Clear();
            _sceneUINames.Clear();

            var regex = new Regex(@"([^/]*/)*([\w\d\-]*)\.unity");

            var scenes = EditorBuildSettings.scenes;
            for (int i = 0; i < scenes.Length; i++)
            {
                string scenePath = scenes[i].path;
                string sceneName = regex.Replace(scenePath, "$2");
                if (_sceneNames.Contains(sceneName))
                {
                    Debug.LogError(sceneName + " already exists. There should not be more than one scene with this name.");
                    _sceneNames.Clear();
                    _sceneUINames.Clear();
                    return;
                }

                _sceneUINames.Add($"{_sceneNames.Count} - {sceneName}");
                _sceneNames.Add(sceneName);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var sceneLoader = (SceneLoader)target;

            if (_sceneNames.Count != EditorSceneManager.sceneCountInBuildSettings)
            {
                CacheSceneNamesByIndexes();
            }

            int selectedIndex = 0;
            if (sceneLoader.LoadWithName)
            {
                selectedIndex = _sceneNames.IndexOf(sceneLoader.SceneName);
            }
            else
            {
                if (_sceneNames.Count == 0) return;
                selectedIndex = Mathf.Clamp(sceneLoader.SceneIndex, 0, _sceneNames.Count - 1);
            }

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Select Scene To Load");
            int selected = EditorGUILayout.Popup(selectedIndex, _sceneUINames.ToArray());
            if (selected != selectedIndex)
            {
                var so = new SerializedObject(sceneLoader);

                so.FindProperty("_sceneName").stringValue = _sceneNames[selected];
                so.FindProperty("_sceneIndex").intValue = selected;

                so.ApplyModifiedProperties();
            }
        }
    }
}
