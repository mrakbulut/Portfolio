using Sirenix.OdinInspector;
using UnityEngine;
namespace Portfolio.Pages
{
    public class TestInfoPopupOpener : MonoBehaviour, IPopupManager
    {
        [SerializeField] private Transform _popupParent;

        private IPopup _currentPopup; // TODO: Set it for your structure "T"

        [Title("Test Variables")]
        private TestPopup _testPopup;

        public void OpenPopup(IPopup popup) // TODO: Send parameter as "T"
        {
            ClosePopup();

            _currentPopup = _testPopup;
            _currentPopup.Open();
        }

        public void ClosePopup()
        {
            if (_currentPopup == null) return;

            _currentPopup.Close();
            // TODO: Refresh Variables if necessary..
        }


        public void OpenPopup() // TODO: FOR TESTING
        {
            // TODO: Fill UI datas from parameter "T"

            OpenPopup(_testPopup);
        }
    }
}
