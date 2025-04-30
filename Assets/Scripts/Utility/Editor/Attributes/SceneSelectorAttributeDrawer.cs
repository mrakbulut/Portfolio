using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Portfolio.Utility.Editor
{
    public class SceneSelectorAttributeDrawer : OdinAttributeDrawer<SceneSelectorAttribute, string>
    {
        private readonly List<string> _sceneNames = new List<string>();
        private readonly List<string> _scenePaths = new List<string>();
        private readonly List<string> _sceneNamesViewArray = new List<string>();

        private bool _locateSceneToggle;

        private const string _searchPattern = "*.unity";

        protected override void Initialize()
        {
            LoadScenes();
        }

        private void LoadScenes()
        {
            _sceneNames.Clear();
            _scenePaths.Clear();
            _sceneNamesViewArray.Clear();

            var sceneIndexes = new List<int>();
            var sceneNamesHolder = new List<string>();
            var scenePathsHolder = new List<string>();

            var buildScenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToList();


            if (!string.IsNullOrEmpty(Attribute.FolderPath))
            {
                string fullPath = Path.Combine("Assets", Attribute.FolderPath);
                if (Directory.Exists(fullPath))
                {
                    string[] sceneFiles = Directory.GetFiles(fullPath, _searchPattern, SearchOption.AllDirectories);
                    foreach (string scenePath in sceneFiles)
                    {
                        string unityPath = scenePath.Replace('\\', '/');
                        // Sadece Build Settings'de olan sahneleri ekle
                        if (buildScenes.Contains(unityPath))
                        {
                            string sceneName = Path.GetFileNameWithoutExtension(unityPath);
                            sceneNamesHolder.Add(sceneName);
                            scenePathsHolder.Add(unityPath);
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Specified folder path does not exist: {fullPath}");
                }
            }
            else
            {
                // Folder path belirtilmemişse tüm build scenes'leri kullan
                foreach (string scenePath in buildScenes)
                {
                    string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                    sceneNamesHolder.Add(sceneName);
                    scenePathsHolder.Add(scenePath);
                }
            }

            /*var sortedList = _sceneNames.Select((name, index) => new { name, path = _scenePaths[index] })
                .OrderBy(x => x.name)
                .ToList();*/

            // None seçeneğini koruyarak listeyi güncelle
            if (Attribute.IncludeNone)
            {
                _sceneNames.Add("None");
                _scenePaths.Add("");
            }

            foreach (string scenePath in scenePathsHolder)
            {
                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    var scene = EditorBuildSettings.scenes[i];
                    if (scene.path == scenePath)
                    {
                        sceneIndexes.Add(i);
                        break;
                    }
                }
            }

            _sceneNames.AddRange(sceneNamesHolder.Select(x => x));
            _scenePaths.AddRange(scenePathsHolder.Select(x => x));
            _sceneNamesViewArray.AddRange(_sceneNames.Select((x, i) => $"{sceneIndexes[i]} - {x}"));
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (_sceneNames == null || _scenePaths == null)
            {
                LoadScenes();
            }

            string currentValue = ValueEntry.SmartValue;
            int currentIndex = _sceneNames.IndexOf(currentValue);
            if (currentIndex == -1) currentIndex = 0;

            int newIndex = SirenixEditorFields.Dropdown(
                label,
                currentIndex,
                _sceneNamesViewArray.ToArray()
                );

            if (ValueEntry.SmartValue == string.Empty || newIndex != currentIndex)
            {
                ValueEntry.SmartValue = _sceneNames[newIndex];
            }


            if (Attribute.Locatable)
            {
                if (!string.IsNullOrEmpty(ValueEntry.SmartValue))
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    _locateSceneToggle = GUILayout.Toggle(_locateSceneToggle, "Show Locate Scene");

                    if (_locateSceneToggle)
                    {
                        int sceneNameIndex = _sceneNames.IndexOf(ValueEntry.SmartValue);
                        string scenePath = _scenePaths[sceneNameIndex];
                        EditorGUILayout.LabelField("Scene Path:", EditorStyles.miniLabel);
                        EditorGUILayout.SelectableLabel(scenePath, EditorStyles.textField,
                            GUILayout.Height(EditorGUIUtility.singleLineHeight));

                        if (GUILayout.Button("Locate Scene", EditorStyles.miniButton))
                        {
                            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                            if (sceneAsset != null)
                            {
                                EditorGUIUtility.PingObject(sceneAsset);
                                //Selection.activeObject = sceneAsset;
                            }
                        }
                    }



                    EditorGUILayout.EndVertical();
                }
            }
        }
    }
}
