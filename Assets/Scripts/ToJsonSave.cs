using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class ToJsonSave : MonoBehaviour
{
    [SerializeField]
    public List<SavingData> SavingDataList;
    
    [Button(ButtonSizes.Gigantic)]
    [GUIColor(0f, 1f, 0f)]
    public void SaveAsJsonToFil(string fileName  ="deneme.json")
    {
        string datas = "[";
        
        for (int i = 0; i < SavingDataList.Count; i++)
        {
            var data = SavingDataList[i];
            Debug.Log(data.CoinGenSpeed.ToString("0.0f"));
            datas += JsonUtility.ToJson(SavingDataList[i]);

            var sss = Convert.ToDouble(data.CoinGenSpeed);
            var result = System.Math.Round(data.CoinGenSpeed, 2);

            
            if (i != SavingDataList.Count - 1)
            {
                datas += ",";
            }
        }
        
        datas += "]";
        
        string path = GetPathFromSaveFile(fileName);
        
        Debug.Log("JSON STRING : " + datas);

        File.WriteAllText(path, datas);
    }

    private string GetPathFromSaveFile(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName + ".json");
    }
}

[System.Serializable]
public class SavingData
{
    public int Level;
    public int UpgradeCost;
    public float CoinGenSpeed;
    public int Damage;
    public int Attack;
    public int Defense;
}
