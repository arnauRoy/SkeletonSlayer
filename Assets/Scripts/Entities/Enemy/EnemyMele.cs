using UnityEngine;

public class EnemyMele : Enemy
{   
    [SerializeField]private float attackRate=3f;
    private float attackRateTime=0;

    protected override void OnEnable()
    {   
        base.OnEnable();
        enemyAnimator.OnDealDamage+=TakeDamagePlayer;
    }

    protected override void OnDisable()
    {   
        base.OnDisable();
        enemyAnimator.OnDealDamage-=TakeDamagePlayer;
    }
    protected override void Attack()
    {   
        if (Time.time < attackRateTime) return;
        Agent.isStopped=true;
        attackRateTime = Time.time + attackRate;
        enemyAnimator.PlayAttack();
    }

    

      
}
