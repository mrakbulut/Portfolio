using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
namespace Portfolio.Minimap
{
    public class MinimapIconUIPool : MonoBehaviour
    {
        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private Transform _parent;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/References")]
        [SerializeField]
        private GameObject _minimapIconUIPrefab;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/Size")]
        [SerializeField]
        private int _initialPoolSize = 20;

        [FoldoutGroup("Pool")]
        [TitleGroup("Pool/Size")]
        [SerializeField]
        private int _maxPoolSize = 40;

        private readonly List<MinimapIconUI> _activeMinimapIconUIs = new List<MinimapIconUI>();

        private IObjectPool<MinimapIconUI> _pool;

        public void Initialize()
        {
            _pool = new ObjectPool<MinimapIconUI>(
                CreateMinimapIconUI,
                OnMinimapIconUIRetrieved,
                OnMinimapIconUIReturned,
                OnMinimapIconUIDestroyed,
                true,
                _initialPoolSize,
                _maxPoolSize);
        }

        private MinimapIconUI CreateMinimapIconUI()
        {
            var minimapIconGO = Instantiate(_minimapIconUIPrefab, _parent);
            var minimapIconUI = minimapIconGO.GetComponent<MinimapIconUI>();
            minimapIconUI.Initialize(_pool);
            return minimapIconUI;
        }

        private void OnMinimapIconUIRetrieved(MinimapIconUI minimapIconUI)
        {
            minimapIconUI.gameObject.SetActive(true);
            _activeMinimapIconUIs.Add(minimapIconUI);
        }

        private void OnMinimapIconUIReturned(MinimapIconUI minimapIconUI)
        {
            minimapIconUI.transform.SetParent(_parent);
            minimapIconUI.gameObject.SetActive(false);
            minimapIconUI.Refresh();
            _activeMinimapIconUIs.Remove(minimapIconUI);
        }

        private void OnMinimapIconUIDestroyed(MinimapIconUI minimapIconUI)
        {

        }

        public MinimapIconUI GetMinimapIconUI()
        {
            var minimapIconUI = _pool.Get();
            minimapIconUI.Setup();

            return minimapIconUI;
        }

        public void ReturnAllMinimapIconUIs()
        {
            for (int i = _activeMinimapIconUIs.Count - 1; i >= 0; i--)
            {
                _activeMinimapIconUIs[i].ReturnToPool();
            }
        }
    }
}
