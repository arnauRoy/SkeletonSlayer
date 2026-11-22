using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private PurchasePoint purchasePoint;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI errorBuyText;


    [SerializeField] private float durationFailedPurchasedText=1f;

    private void Start()
    {
        if (purchasePoint == null || purchasePoint.Offer == null) return;
        errorBuyText.enabled=false;
        nameText.text = purchasePoint.Offer.displayName;
        priceText.text = purchasePoint.Offer.cost + " $";

        buyButton.onClick.AddListener(OnBuyClicked);
        RefreshState();
    }

    private void OnEnable()
    {
        RefreshState();
    }

    private void OnBuyClicked()
    {
        if (purchasePoint.TryBuy())
        {
            RefreshState();
        }
        else
        {
            onBuyFailed();
        }
    }

    private void onBuyFailed()
    {
        if(errorBuyText==null)return;
        StartCoroutine(SetFailedText());
        
    }

    private IEnumerator SetFailedText()
    {
        buyButton.interactable=false;
        errorBuyText.enabled=true;

        errorBuyText.text="insuficient founds";
        errorBuyText.color=Color.red;

        yield return new WaitForSeconds(durationFailedPurchasedText);
        
        buyButton.interactable=true;
        errorBuyText.enabled=false;

    } 

    public void RefreshState()
    {
        if (purchasePoint == null || purchasePoint.Offer == null) return;

        bool soldOut = purchasePoint.IsSoldOut;
        buyButton.interactable = !soldOut;

        if (soldOut)
        {
            priceText.text = "COMPRADO";
        }
    }
}   