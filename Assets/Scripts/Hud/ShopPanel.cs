using UnityEngine;
using System;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public event Action<bool> OnShopOpen;

    public void OpenShop()
    {
        panel.SetActive(true);
        OnShopOpen?.Invoke(true);
       
    }

    public void CloseShop()
    {
        panel.SetActive(false);
        OnShopOpen?.Invoke(false);

        
    }
}