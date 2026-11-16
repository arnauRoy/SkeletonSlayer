using TMPro;
using UnityEngine;

public class MoneyHud : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    public void SetMoney(int money)
    {
        if (moneyText == null) return;
        moneyText.text = money.ToString();
    }
}