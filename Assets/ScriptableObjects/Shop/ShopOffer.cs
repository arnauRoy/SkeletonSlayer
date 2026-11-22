using UnityEngine;

[CreateAssetMenu(fileName = "ShopOffer", menuName = "Scriptable Objects/ShopOffer")]
public class ShopOffer : ScriptableObject
{
    public string displayName;
    public int cost;
    public bool oneTimePurchase;
}
