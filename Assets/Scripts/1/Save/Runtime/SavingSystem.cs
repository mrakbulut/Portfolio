using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Portfolio.Save
{
    public class SavingSystem : MonoBehaviour
    {
        [Header("Broadcasting On...")]
        [SerializeField] private SaveableListEventChannelSO _onSaveablesRequested;

        public void Save(string fileName)
        {
            var state = LoadFile(fileName);
            CaptureState(state);
            SaveFile(fileName, state);
        }

        public void Load(string fileName)
        {
            RestoreState(LoadFile(fileName));
        }

        public void Delete(string fileName)
        {
            string path = GetPathFromSaveFile(fileName);
            File.Delete(path);
        }

        public void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);

            using (var stream = File.Open(path, FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, state);
            }
        }

        public Dictionary<string, object> LoadFile(string loadFile)
        {
            string path = GetPathFromSaveFile(loadFile);


            if (!File.Exists(path)) return new Dictionary<string, object>();

            using (var stream = File.Open(path, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                var state = (Dictionary<string, object>)binaryFormatter.Deserialize(stream);

                return state;
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            var saveables = new List<SaveableEntity>();
            _onSaveablesRequested.RaiseEvent(saveables);

            for (int i = 0; i < saveables.Count; i++)
            {
                string saveableID = saveables[i].GetUniqueIdentifier();
                if (state.ContainsKey(saveableID))
                {
                    state[saveableID] = saveables[i].CaptureState();
                }
                else
                {
                    state.Add(saveableID, saveables[i].CaptureState());
                }
            }
//            Debug.Log("Game Saved"); 
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            if (state is null) return;

            var saveables = new List<SaveableEntity>();
            _onSaveablesRequested.RaiseEvent(saveables);

            for (int i = 0; i < saveables.Count; i++)
            {
                string saveableID = saveables[i].GetUniqueIdentifier();
                if (state.ContainsKey(saveableID))
                {
                    saveables[i].RestoreState(state[saveableID]);
                }
            }
            // print("Save Loaded");
        }

        private string GetPathFromSaveFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }

    }
}
