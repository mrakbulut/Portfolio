using Portfolio.NavigationBars;
using Portfolio.Pages;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Portfolio.PageNavigation
{
    public class PageNavigationManager : MonoBehaviour
    {
        [Title("Essentials", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private PageManager _pageManager;
        [SerializeField] private NavigationBar _navigationBar;

        private void Start()
        {
            _navigationBar.OnSelectedButtonChanged += OnPageChanged;
            _pageManager.NavigateTo(_navigationBar.SelectedButtonIndex);
        }

        private void OnDestroy()
        {
            _navigationBar.OnSelectedButtonChanged -= OnPageChanged;
        }

        private void OnPageChanged(NavigationBarButton navigationBarButton, int pageIndex)
        {
            _pageManager.NavigateTo(pageIndex);
        }
    }
}
