using UnityEngine;
using System;

[RequireComponent(typeof(PlayerController))]
public class Player : Entity
{

    private PlayerController playerController;

    [SerializeField]private int maxShield=100;
    [SerializeField]private int shield=100;

    public event Action<int,int> ShieldUpdated; //Avisa sobre la accion los subscriptores pueden hacer cosas en funcion de eso


    private bool deadHandled=false;

    protected void Awake () 
    {
        playerController=GetComponent<PlayerController>();
    }

    void Update()
    {   
        if(deadHandled) return;
        CheckIsDead();
    }
    private void CheckIsDead()
    {
        if (IsDead) OnEntityDead();
    }

    public override void TakeDamage(int damage,Vector3 source)
    {
        if (IsDead) return;

        if (shield>=1)
        {   
            shield = Mathf.Clamp(shield, 0, maxShield);
            int damageToTake=Math.Min(shield,damage);
            int remainingDamage=damage-damageToTake;

            shield-=damageToTake;
            ShieldUpdated?.Invoke(shield,maxShield);
            if (remainingDamage > 0 )
            {
                base.TakeDamage(remainingDamage,source);
            }
            
        }
        else
        {
            base.TakeDamage(damage,source);
        }     
    }
    public override void OnEntityDead()
    {  
       if(deadHandled) return;
       playerController.enabled=false;//desactivamos el input del jugador

       deadHandled=true;
       base.OnEntityDead();

    }

    public void recibeShield(int shield)
    {
        if(IsDead) return;
        int shieldNeeded=maxShield-shield;
        int shieldToCure=Math.Min(shieldNeeded,shield);

        shield+=shieldToCure;

       ShieldUpdated?.Invoke(shield,maxShield);
        
    }

    public int Shield{
        get{return shield;}
    }
    public int MaxShield{
        get{return maxShield;}
    }
    
}