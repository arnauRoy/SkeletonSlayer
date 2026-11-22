using UnityEngine;
using UnityEngine.Events;

public class PurchasePoint : MonoBehaviour
{
    [SerializeField]private ShopOffer offer;
    private bool purchased=false;
    [SerializeField] private UnityEvent onPurchased;


    private bool tryBuy()
    {
        if(offer.oneTimePurchase&&purchased)return false;

        if (!GameSession.Instance.Wallet.SpendMoney(offer.cost))
        {
            Debug.Log("Dinero insuficiente para: " + offer.displayName);
            return false;
        }

        purchased=true;
        onPurchased?.Invoke();

        return true;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>() == null) return;

        tryBuy();
    }
}
