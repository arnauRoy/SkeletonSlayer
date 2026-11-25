using UnityEngine;
using UnityEngine.Events;

public class PurchasePoint : MonoBehaviour
{
    [SerializeField]private ShopOffer offer;
    private bool purchased=false;
    [SerializeField] private UnityEvent onPurchased;
    public ShopOffer Offer => offer;
    public bool IsSoldOut => offer != null && offer.oneTimePurchase && purchased;

    public bool purchasedFailedByFounds=false;

    public bool TryBuy()
    {   
        if (offer == null || IsSoldOut) return false;

        if (GameSession.Instance == null)
        {
            Debug.LogWarning("No hay GameSession activa en la escena.");
            return false;
        }
        
        Inventory inventory = GameSession.Instance.Inventory;

        if (offer.item != null)
        {   
            if (inventory.FreeSpace(offer.item) < offer.amount)
            {
                Debug.Log("No cabe más de: " + offer.item.displayName);
                return false;
            }

        }

        if (!GameSession.Instance.Wallet.SpendMoney(offer.cost))
        {
            Debug.Log("Dinero insuficiente para: " + offer.displayName);
            purchasedFailedByFounds=true;
            return false;
        }

        if (offer.item != null)
        {
            inventory.Add(offer.item,offer.amount);
            Debug.Log($"{offer.item}: {inventory.Count(offer.item)}");
        }
        purchasedFailedByFounds=false;
        purchased=true;
        onPurchased?.Invoke();

        return true;
        
    }

}
