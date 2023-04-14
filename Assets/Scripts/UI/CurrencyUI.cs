using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currency;

    void Start()
    {
        User.Current.CurrencyChanged+=UpdateCurrencyUI;
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        currency.text = $"Валюта: {User.Current.Currency}";
    }
}
