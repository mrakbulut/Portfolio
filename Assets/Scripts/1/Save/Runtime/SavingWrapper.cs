using UnityEngine;

namespace Portfolio.Save
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField]
        private string defaultSaveFile = "Xsave";

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
#if UNITY_EDITOR
            Debug.Log("SAVING");
#endif

            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            // if (!focusStatus) Save();
        }
    }
}
