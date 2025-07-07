using System;
using System.Collections.Generic;
using UnityEngine;
namespace Portfolio.Pages
{
    public class PageManager : MonoBehaviour
    {
        [SerializeField] private List<Page> _pages = new List<Page>();

        public Action<Page> OnPageChanged = _ => { };

        public Page CurrentPage { get; private set; }
        public int CurrentPageIndex => CurrentPage?.PageIndex ?? -1;

        private void Awake()
        {
            InitializePages();
        }

        public void InitializePages()
        {
            for (int i = 0; i < _pages.Count; i++)
            {
                if (_pages[i] == null) continue;

                _pages[i].Initialize(i);
            }
        }

        public void NavigateTo(int pageIndex)
        {
            var newPage = GetPageWithIndex(pageIndex);
            NavigateTo(newPage);
        }

        public void NavigateTo(Page newPage)
        {
            if (newPage == null) return;

            if (CurrentPage != null)
            {
                CurrentPage.Close();
            }

            CurrentPage = newPage;
            CurrentPage.Open();

            OnPageChanged.Invoke(CurrentPage);
        }

        public Page GetPageWithIndex(int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex < _pages.Count)
            {
                return _pages[pageIndex];
            }

            Debug.LogError($"Invalid page index: {pageIndex}");
            return null;
        }
    }
}
