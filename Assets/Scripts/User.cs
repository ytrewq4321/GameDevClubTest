using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

[Serializable]
public class User
{
    public static User Current { get; private set; }
    [JsonProperty] public Inventory Inventory { get; private set; }

    [JsonProperty] public int Currency { get; private set; }
   
    public event Action CurrencyChanged;

    public void SetCurrency(int value)
    {
        Currency -= value;
        CurrencyChanged.Invoke();
    }

    public static bool Save()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        string jsonString = JsonConvert.SerializeObject(User.Current, Formatting.Indented);
        File.WriteAllText(path, jsonString);
        return true;
    }

    public static bool Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            User saveData = JsonConvert.DeserializeObject<User>(jsonString);
            Current = saveData;
            return true;
        }
        return false;
    }

    public static void CreateUser(int size,int weight, int unlockSlotCost,int currency)
    {
        Current = new User();
        Current.Currency = currency;
        Current.Inventory = new Inventory(size,weight, unlockSlotCost);
    }
}
