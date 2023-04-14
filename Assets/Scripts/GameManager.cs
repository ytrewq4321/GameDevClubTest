using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private int inventorySize;
    [SerializeField] private int inventoryWeight;
    [SerializeField] private int slotCost;
    [SerializeField] private int currency;

    void Awake()
    {
        itemDatabase.Initialize();
        if (User.Current==null)
        {
            if (!User.Load())
            {
                Debug.Log("CREATE NEW USER");
                User.CreateUser(inventorySize, inventoryWeight, slotCost, currency);

            }
        }
    }

    private void OnApplicationQuit()
    {
        User.Save();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            User.Save();
        }
    }
}
