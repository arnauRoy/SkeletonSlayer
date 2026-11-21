using UnityEngine;
using UnityEngine.AI;
    public abstract class Enemy : Entity
    {
//------------------------------Nav Mesh Variables--------------------------------
        private NavMeshAgent navMeshAgent;
        [SerializeField] private float rangeDetection=10f;

        [SerializeField] private int damage=20;
        [SerializeField] private float chasingSpeed = 5f;
        [SerializeField] private float patrolingSpeed = 3f;

        [SerializeField] private float investigatingSpeed = 2f;



//------------------------------Object Variables--------------------------------
        private Collider collider;

        [SerializeField] protected EnemyAnimator enemyAnimator; //Encargado de las animaciones

        private int reward=10;
        public int Reward => reward;

        [SerializeField] private int rewardMin=5;

        [SerializeField] private int rewardMax=10;



//------------------------------WayPoints--------------------------------

        [SerializeField] Transform[] wayPoints;

        private int lastWayPoint;
        private int currentWaypoint;

//------------------------------States/Movements/Attack--------------------------------

        private EnemyState stateEnemy;
        
        private float updatePathRate = 0.2f;
        private float nextPathUpdate=0;
        [SerializeField] private float attackRange=2f;
        private float DistanceToChangeWayPoint=2f;
        private Player player;

        private Vector3 investigatePosition;


        
        //Constructor

        protected virtual void Awake()
        {
            reward = Random.Range(rewardMin, rewardMax + 1);
        }
        public void Initialize(Transform[] wayPoints)
        {
            this.wayPoints=wayPoints;

        }

        void Start()
        {   
            collider=GetComponentInChildren<Collider>();
            navMeshAgent=GetComponent<NavMeshAgent>();
            player=FindAnyObjectByType<Player>();
            enemyAnimator=GetComponent<EnemyAnimator>();


            if (player == null||enemyAnimator==null)//Si no encuentra al player desactivamos los update
            {
                Debug.LogWarning("No se ha encontrado ningun jugador o componente animator",this);
                enabled = false;
                return;
            }
            stateEnemy=EnemyState.Patrol;
            navMeshAgent.speed=chasingSpeed;


        }
    /*___________________________________________________________________________________
    |                                                                                    |
    |                                                                                    |
    |--------------------------------[updates]-------------------------------------------|
    |                                                                                    |
    |____________________________________________________________________________________|
    */
    protected virtual void OnEnable()
    {
        Damaged+=OnDamaged;
    }

    protected virtual void OnDisable()
    {
        Damaged-=OnDamaged;
        
    }

    private void OnDamaged(Entity e, Vector3 sourceDamage)
    {
        if (IsDead || stateEnemy == EnemyState.Dead) return;

        // Si ya está persiguiendo o atacando al jugador, ignoramos la investigación
        if (stateEnemy == EnemyState.Chasing || stateEnemy == EnemyState.Attack) return;

        investigatePosition = sourceDamage;
        ChangeState(EnemyState.Investigating);
    }
    void Update()
    {  
        CheckIsDead();
        UpdateState();
        ExecuteState();
        UpdateAnimation();

    }

    protected virtual bool CanSeePlayer()//Comprueba con raycast si tiene vision del target
    {       
        return true; 
    }

    private void UpdateAnimation() //Con el estado actual sabemos la animacion a reproducir
    {
        enemyAnimator.SetState(stateEnemy);
        
    }
    protected  void UpdateState()
    {
        if(StateEnemy==EnemyState.Dead||player==null) return;  

            float DistanceFromPlayer=CalculateDistanceFromAPoint(PlayerTarget.transform);
            if(Time.time>=NextPathUpdate){
                if(DistanceFromPlayer<= AttackRange&&CanSeePlayer()){
                    ChangeState(EnemyState.Attack);                              
                }
                else if (DistanceFromPlayer<=RangeDetection)
                {
                    ChangeState(EnemyState.Chasing);
                }
                else if (stateEnemy != EnemyState.Investigating)
                {
                    ChangeState(EnemyState.Patrol);                              

                    
                }
                NextPathUpdate=Time.time+UpdatePathRate;
            }   
    }


    protected float CalculateDistanceFromAPoint(Transform point)//Nos devuelve la distancia entre el enemigo y el player
    {
        return Vector3.Distance(transform.position,point.position);

    }

    /*___________________________________________________________________________________
    |                                                                                    |
    |                                                                                    |
    |--------------------------------[Ejecucion de los estados]--------------------------|
    |                                                                                    |
    |____________________________________________________________________________________|
    */  
        protected void ExecuteState()
        {
            switch (stateEnemy)
            {
                case EnemyState.Attack: 
                    Attack();
                    break;
                case EnemyState.Chasing:Chasing(); 
                    break;
                case EnemyState.Patrol: 
                    navMeshAgent.stoppingDistance=DistanceToChangeWayPoint; 
                    Patrol(); break;
                case EnemyState.Investigating:
                    Investigating();
                    break;
                case EnemyState.Dead:  
                    break;

            }
        }

        private void Investigating()
        {   
            navMeshAgent.isStopped=false;
            navMeshAgent.stoppingDistance = 0.5f;
            navMeshAgent.destination=investigatePosition;
            navMeshAgent.speed=investigatingSpeed;

            bool hasArrived =!navMeshAgent.pathPending&&navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;

            if(hasArrived) ChangeState(EnemyState.Patrol);
        }
        private void Chasing()
        {   
            navMeshAgent.isStopped=false;
            navMeshAgent.destination=player.transform.position;
            navMeshAgent.stoppingDistance = attackRange * 0.8f;
            navMeshAgent.speed=chasingSpeed;
            
        }

        protected abstract void Attack();//Cada enemigo implementa su forma de ataque
       

        public void TakeDamagePlayer()
        {
            if (IsDead || player == null || player.IsDead) return;
            if (CalculateDistanceFromAPoint(player.transform) > attackRange) return;

            player.TakeDamage(damage,transform.position);
        }
        

       private void Patrol()
       {
            if (wayPoints==null||wayPoints.Length == 0) return;

            navMeshAgent.isStopped = false;
            navMeshAgent.stoppingDistance = DistanceToChangeWayPoint;
            navMeshAgent.speed = patrolingSpeed;

            bool hasArrived = !navMeshAgent.pathPending
                            && navMeshAgent.remainingDistance <= DistanceToChangeWayPoint;

            if (!hasArrived) return;
            
            currentWaypoint = Random.Range(0,wayPoints.Length);

            if(wayPoints.Length>1){
                while (currentWaypoint == lastWayPoint)
                {
                    currentWaypoint=Random.Range(0,wayPoints.Length);
                }
            }
                navMeshAgent.destination = wayPoints[currentWaypoint].position;

                lastWayPoint=currentWaypoint;

       }

        //Cambiar el estado actual
        protected void ChangeState(EnemyState state)
        {
            if(stateEnemy==state) return;

            stateEnemy=state;

            if(state==EnemyState.Dead) OnEntityDead();
        }

    /*___________________________________________________________________________________
    |                                                                                    |
    |                                                                                    |
    |--------------------------------[Muerte]---------------------------------|
    |                                                                                    |
    |____________________________________________________________________________________|
    */  
        private void CheckIsDead()
        {
            if (IsDead) ChangeState(EnemyState.Dead);
            
        }  
        public override void OnEntityDead()
        {   
            base.OnEntityDead();
            navMeshAgent.isStopped=true;
            //Desactivamos el collider y le cambiamos el color
            collider.enabled=false;
            Destroy(gameObject,2);  
        }

    /*___________________________________________________________________________________
    |                                                                                    |
    |                                                                                    |
    |--------------------------------[Getters]---------------------------------|
    |                                                                                    |
    |____________________________________________________________________________________|
    */ 
        public float AttackRange
        {
            get{return attackRange;}
        }

        public float RangeDetection
        {
            get {return rangeDetection;}   
        }

        public Player PlayerTarget
        {
            get{return player;}
        }

        public float NextPathUpdate
        {
            get{return nextPathUpdate;}
            set{nextPathUpdate=value;}
        }

        public EnemyState StateEnemy
        {
            get{return stateEnemy;}
        }

        public float UpdatePathRate
        {
            get{return updatePathRate;}
        } 

        public NavMeshAgent Agent
        {
           get{return navMeshAgent;} 
        }
}
