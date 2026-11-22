using UnityEngine;

public class ShieldPickup : Pickup
{
    [SerializeField] private int amountShield=20;

    protected override void collectPickup(Player player)
    {
        if(player.Shield>=player.MaxShield)return;

        player.recibeShield(amountShield);

        base.collectPickup(player);
    }
}
