using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Portfolio.Minimap
{
    public class MinimapIconUI : MonoBehaviour
    {
        [TitleGroup("Visual Components")]
        [SerializeField] private Image _iconImage;

        private RectTransform _rectTransform;

        private IObjectPool<MinimapIconUI> _pool;
        private bool _released;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(IObjectPool<MinimapIconUI> pool)
        {
            _pool = pool;
        }

        public void Setup()
        {
            _released = false;
            SetVisible(true);
        }

        public void SetPosition(Vector2 position)
        {
            _rectTransform.anchoredPosition = position;
        }

        public void SetSprite(Sprite sprite)
        {
            _iconImage.sprite = sprite;
        }

        public void Refresh()
        {
            SetVisible(false);
            _rectTransform.localScale = Vector3.one;
            _rectTransform.anchoredPosition = Vector2.zero;
            _released = false;
        }

        public void ReturnToPool()
        {
            if (_released) return;

            _released = true;
            _pool.Release(this);
        }

        private void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
