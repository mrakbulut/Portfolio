using Portfolio.Pages;
namespace Portfolio.PageNavigation
{
    public class PipelineToPage : Page
    {
        private Page _toPage;

        public void SetPage(Page page)
        {
            if (_toPage != null) return;

            _toPage = page;
        }

        public override void Open()
        {
            if (_toPage == null) return;

            _toPage.Open();
        }
        public override void Close()
        {
            if (_toPage == null) return;

            _toPage.Close();
        }
    }
}
