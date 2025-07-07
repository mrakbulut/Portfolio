using Portfolio.Pages;
namespace Portfolio.PageNavigation
{
    public class TestPage : Page
    {
        public override void Open()
        {
            gameObject.SetActive(true);
            //Debug.Log("On Page Open : " + gameObject, gameObject);
        }
        public override void Close()
        {
            gameObject.SetActive(false);
//            Debug.Log("On Page Close : " + gameObject, gameObject);
        }
    }
}
