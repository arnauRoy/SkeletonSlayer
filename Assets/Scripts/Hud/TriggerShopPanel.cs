using UnityEngine;

public class TriggerShopPanel : MonoBehaviour
{
    [SerializeField] private ShopPanel shopPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>() == null) return;

        shopPanel.OpenShop();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Player>() == null) return;

        shopPanel.CloseShop();
    }
}