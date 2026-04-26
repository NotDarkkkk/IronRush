using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    public int money;

    public TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("Money: " + money);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: " + money;
        }
    }
}