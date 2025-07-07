namespace Portfolio.Pages
{
    public interface IPopupManager
    {
        void OpenPopup(IPopup popup);
        void ClosePopup();
    }
}
