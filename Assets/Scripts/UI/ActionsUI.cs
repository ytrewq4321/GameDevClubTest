using UnityEngine;
using UnityEngine.UI;

public class ActionsUI : MonoBehaviour
{
    [SerializeField] private Button shootButton;
    [SerializeField] private Button addPatronsButton;
    [SerializeField] private Button addItemsButton;
    [SerializeField] private Button removeItemButton;
    [SerializeField] private Button unlockSlotButton;
    private Actions actions;

    void Start()
    {
        actions = new Actions();
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        shootButton.onClick.AddListener(actions.Shoot);
        addPatronsButton.onClick.AddListener(actions.AddPatrons);
        addItemsButton.onClick.AddListener(actions.AddItems);
        removeItemButton.onClick.AddListener(actions.RemoveItem);
        unlockSlotButton.onClick.AddListener(actions.UnlockNewSlot);
    }
}
