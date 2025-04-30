using Portfolio.SceneLoad;
using UnityEngine;

namespace Portfolio.InitialLoad
{
    public class SceneLoaderCommand : LoadCommand
    {
        [SerializeField] private SceneLoader _sceneLoader;

        private float _percentageComplete;
        public override float PercentageComplete => _percentageComplete;

        public override void StartCommand()
        {
            _percentageComplete = 0f;
            _sceneLoader.OnSceneLoadCompleted += OnSceneLoadComplete;
            _sceneLoader.LoadScene();
        }
        private void OnSceneLoadComplete()
        {
            _percentageComplete = 1f;
            CompleteCommand();
        }
        public override void ResetCommand()
        {

        }
    }
}
