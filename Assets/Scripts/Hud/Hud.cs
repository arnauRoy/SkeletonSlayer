using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{   
    //----------------Player----------------
    [SerializeField ]private Image healthBar;

    [SerializeField ]private Image shieldBar;

    [SerializeField] private Player player;

    //----------------Weapon----------------
    [SerializeField] private Weapon weapon;
    [SerializeField] private TMP_Text totalAmmoText;
    [SerializeField] private TMP_Text ammoInMagazineText;
    [SerializeField] private RawImage CrossHair;


    //----------------InfoWave----------------
    [SerializeField] private WaveSpawner waveSpawner;

    [SerializeField] private TMP_Text totalEnemiesText;
    [SerializeField] private TMP_Text actualWaveText;

    [SerializeField] private TMP_Text totalWavesText;
    
   //----------------Money----------------
    [SerializeField] private MoneyHud moneyHud;

   //----------------Shop----------------

   [SerializeField]private ShopPanel shopPanel;

    private Wallet wallet; 

    //----------------Actions----------------
    public event Action<bool> OnInteractionUI;

    private void OnEnable()
    {   
        if (moneyHud == null)
            moneyHud = GetComponent<MoneyHud>();
        // suscribirse a eventos
        weapon.AmmoUpdate+=UpdateAmmo;
        player.HealthUpdated+=UpdateHealth;
        player.ShieldUpdated+=UpdateShield;
        if (shopPanel != null) shopPanel.OnShopOpen += onPlayerInUI;
        // pintar el valor actual una vez, para no esperar al primer update
        UpdateAmmo(weapon.Ammo,weapon.AmmoInMagazine);
        UpdateHealth(player.Health,player.MaxHealth);
        UpdateShield(player.Shield,player.MaxShield);
     

        wallet = GameSession.Instance.Wallet;
        wallet.MoneyChanged += OnMoneyChanged;
        OnMoneyChanged(wallet.CurrentMoney); 


        if(waveSpawner==null)return;
        waveSpawner.EnemiesAliveChanged+=UpdateEnemiesWave;
        waveSpawner.WaveStarted+=UpdateWaves;

    }

    private void onPlayerInUI(bool state)
    {
        OnInteractionUI?.Invoke(state);

        if(CrossHair!=null)CrossHair.enabled=!state;//Si esta en la tienda contrario desactivamos la mirilla
    }

    private void OnDisable()
    {
        // darse de baja
        weapon.AmmoUpdate-=UpdateAmmo;
        player.HealthUpdated-=UpdateHealth;
        player.ShieldUpdated-=UpdateShield;
        shopPanel.OnShopOpen-=onPlayerInUI;
        
        
        if (wallet != null) wallet.MoneyChanged -= OnMoneyChanged;     // valor inicial

        if(waveSpawner==null)return;

        waveSpawner.EnemiesAliveChanged-=UpdateEnemiesWave;
        waveSpawner.WaveStarted-=UpdateWaves;
    }

    private void UpdateAmmo(int totalAmmo, int ammoInMagazine)
    {
        totalAmmoText.text=totalAmmo.ToString();
        ammoInMagazineText.text=ammoInMagazine.ToString();
    }

    private void UpdateHealth(int health, int maxHealth)
    {
        if(healthBar==null) return;
        healthBar.fillAmount=(float)health/maxHealth;
    }

    private void UpdateShield(int shield, int maxShield)
    {
        if(shieldBar==null) return;
        shieldBar.fillAmount=(float)shield/maxShield;
    }

    private void UpdateEnemiesWave(int enemiesInWave)
    {
        totalEnemiesText.text=enemiesInWave.ToString();

    }

    private void UpdateWaves(int actualWave,int MaxWave)
    {
        actualWaveText.text=actualWave.ToString();
        totalWavesText.text=MaxWave.ToString();
    }

    private void OnMoneyChanged(int money)
    {
        moneyHud.SetMoney(money);
    }


}
