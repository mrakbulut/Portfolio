using UnityEngine;
namespace Portfolio.Pages
{
    public abstract class Page : MonoBehaviour
    {
        public int PageIndex { get; private set; }

        public virtual void Initialize(int pageIndex)
        {
            PageIndex = pageIndex;
            Close();
        }

        public abstract void Open();
        public abstract void Close();
    }
}
