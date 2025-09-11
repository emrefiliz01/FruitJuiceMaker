using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int currentMoney = 0;
    [SerializeField] private TextMeshProUGUI moneyText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        UpdateMoney();
    }

    public void IncreaseMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoney();
    }

    public void DecreaseMoney(int amount)
    {
        currentMoney -= amount;
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        moneyText.text = "Money: " + currentMoney;
    }
}
