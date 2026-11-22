using UnityEngine;

public abstract class Pickup : MonoBehaviour
{

    private bool isActive;
    private bool isUnlocked;//Si es verdadero el timer de respawn no funcionara

    //----------------[Respawn del pickup]----------------

    [SerializeField] private bool canRespawn;

    [SerializeField]private float respawnTime=5f;//Tiempo que tarda en reaparecer
    private float respawnTimer;//El timer que controla el respawn

    //----------------[Posicion y Rotacion]----------------

    [SerializeField]private float rotationSpeed=100f;
    private float startPositionY;
    [SerializeField]private float floatAmplitude = 0.3f; // Altura del movimiento
    [SerializeField]private float floatFrequency = 2f;   // Velocidad de subida y bajada


    //----------------[Componentes a desabilitar]----------------
    private Collider[] pickupCollider;

    private Renderer[] pickupRenderer;
    
    [SerializeField] private Light pickupLight;


    void Awake()
    {
        startPositionY=transform.position.y;
        isActive=true;
        pickupCollider=GetComponentsInChildren<Collider>();
        pickupRenderer=GetComponentsInChildren<Renderer>();

    }

   public void Activate()
    {
        isUnlocked = true;
        SetState(true);
    }

    public void Deactivate()
    {
        isUnlocked = false;
        SetState(false);
    }

    private void SetState(bool state)
    {
        isActive = state;
        ToggleRenderer(state);
        ToggleCollider(state);
        TogglePickupLight(state);
    }
    private void RespawnPickup()
    {   
        if(isActive) return;

        if (Time.time > respawnTimer)  
        {   
          SetState(true);
              
        }
    }

    private void ToggleRenderer(bool toogle)
    {   
        if(pickupRenderer==null||pickupRenderer.Length<=0)return;
        foreach(Renderer r in pickupRenderer)
        {
            r.enabled=toogle;
        }
    }

     private void ToggleCollider(bool toogle)
    {   
        if(pickupCollider==null||pickupCollider.Length<=0)return;

        foreach(Collider r in pickupCollider)
        {
            r.enabled=toogle;
        }
    }
    
    private void TogglePickupLight(bool state)
    {
        if(pickupLight==null)return;

        pickupLight.enabled=state;

    }
    protected virtual void collectPickup(Player player)
    {
        if (!isActive) return;
        isActive=false;

        ToggleRenderer(false);

        ToggleCollider(false);

        TogglePickupLight(false);

        respawnTimer=Time.time+respawnTime;
    }
    void Update()
    {   
        if (isActive)
            Spin();
        else if(canRespawn&&isUnlocked)
            RespawnPickup();
    }
    protected virtual void Spin()
    {
      transform.Rotate(new Vector3(0, rotationSpeed*Time.deltaTime, 0),Space.World);//Rotamos solo la y
      
      //Le añadimos la posicion inicial al valor dado
      float newY = startPositionY + Mathf.Sin(Time.time * floatFrequency)//Multiplicamos el tiempo pasado por la frequencia(velocidad de subida)
                     * floatAmplitude;//Multiplicamos por la altura maxima que llegara
    
        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player == null|| !isActive) return;

         collectPickup(player);             
            
    }
}
