using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Portfolio.SceneLoad
{
    public class SceneLoader : MonoBehaviour
    {
        [VerticalGroup("Scene Load Types", 0.2f)] [PropertyOrder(0)]
        [Button("$_sceneButtonName", ButtonSizes.Small)]
        private void TriggerSceneLoadType()
        {
            _loadWithName = !_loadWithName;
            _sceneButtonName = _loadWithName ? "LOAD SCENE WITH : Name" : "LOAD SCENE WITH : Index";
        }

        [SerializeField] [HideInInspector] private bool _loadWithName;
        public bool LoadWithName => _loadWithName;
        private string _sceneButtonName;

        [VerticalGroup("Scene Load Types")]
        [ShowIf("_loadWithName")] [Required] [PropertyOrder(1)] [DisableIf("$_disable")]
        [SerializeField] private string _sceneName;

        public string SceneName => _sceneName;

        [VerticalGroup("Scene Load Types")]
        [HideIf("_loadWithName")] [Required] [PropertyOrder(1)] [DisableIf("$_disable")]
        [SerializeField] private int _sceneIndex;

        public int SceneIndex => _sceneIndex;

        private const bool _disable = true;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            _sceneButtonName = _loadWithName ? "LOAD SCENE WITH : Name" : "LOAD SCENE WITH : Index";
        }
        #endif

        public Scene LoadedScene { get; private set; }
        private Coroutine _sceneLoadRoutine;

        public Action OnSceneLoadCompleted = () => { };
        public Action OnSceneLoadFailed = () => { };

        public void LoadScene()
        {
            if (_sceneLoadRoutine != null) StopCoroutine(_sceneLoadRoutine);
            _sceneLoadRoutine = StartCoroutine(LoadSceneAsync());
        }

        public IEnumerator LoadSceneAsync()
        {
            yield return null;

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            var asyncOperation = _loadWithName ?
                SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive) :
                SceneManager.LoadSceneAsync(_sceneIndex, LoadSceneMode.Additive);

            if (asyncOperation == null)
            {
                Debug.LogError("Scene loading failed : " + _sceneName);
                OnSceneLoadFailed.Invoke();
                SceneManager.sceneLoaded -= OnSceneLoaded;
                yield break;
            }

            if (!asyncOperation.isDone)
            {
                yield return null;
            }

            yield return null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            LoadedScene = scene;
            //Debug.Log("LOADED SCENE NAME : " + LoadedScene.name, this);
            OnSceneLoadCompleted.Invoke();

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void UnloadScene()
        {
            if (_sceneLoadRoutine != null) StopCoroutine(_sceneLoadRoutine);
            _sceneLoadRoutine = StartCoroutine(UnloadSceneAsync());
        }

        public IEnumerator UnloadSceneAsync()
        {
            yield return _loadWithName ? SceneManager.UnloadSceneAsync(_sceneName) : SceneManager.UnloadSceneAsync(_sceneIndex);
        }
    }
}
