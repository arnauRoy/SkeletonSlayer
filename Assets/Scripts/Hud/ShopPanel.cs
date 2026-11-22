using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void OpenShop()
    {
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseShop()
    {
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}