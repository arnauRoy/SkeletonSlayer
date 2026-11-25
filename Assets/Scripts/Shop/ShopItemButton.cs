using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private PurchasePoint purchasePoint;
    [SerializeField] private Image itemImage;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI errorBuyText;
    [SerializeField] private TextMeshProUGUI amountText;



    [SerializeField] private float durationFailedPurchasedText=1f;

    private void Start()
    {
        if (purchasePoint == null || purchasePoint.Offer == null) return;
        errorBuyText.enabled=false;
        nameText.text = purchasePoint.Offer.displayName;
        priceText.text = purchasePoint.Offer.cost + " $";

        //Comprobamos si tiene item y si tiene icono para añadirselo a la imagen
        Sprite spriteImage = purchasePoint?.Offer?.item?.icon;
        if(itemImage!=null &&spriteImage!=null)itemImage.sprite=spriteImage;

        int? amount=purchasePoint?.Offer?.amount;
        if(amountText!=null &&amount!=null)amountText.text=amount.ToString();

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
        else if(purchasePoint.purchasedFailedByFounds)
        {
            onBuyFailed();
        }
        else
        {
            onMaxAmount();
        }
    }

    private void onBuyFailed()
    {
        if(errorBuyText==null)return;
        StartCoroutine(SetFailedText("insuficient founds"));
        
    }

    private void onMaxAmount()
    {
        if(errorBuyText==null)return;
        StartCoroutine(SetFailedText("Max amoun reached"));
    }

    private IEnumerator SetFailedText(string text)
    {
        buyButton.interactable=false;
        errorBuyText.enabled=true;

        errorBuyText.text=text;
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