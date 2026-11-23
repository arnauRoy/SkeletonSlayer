using UnityEngine;

public class ExplosiveGrenade : MonoBehaviour
{   

    //----------------[GrenadeStats]----------------
    private float countDown;
    private bool exploded=false;
    [SerializeField] private float explosionForce=700f;

    [SerializeField] private float delayExplosion=5f;
    [SerializeField] private float radiusExplosion=5f;

    [SerializeField]private int damage=60;


    void Start()
    {
        countDown=delayExplosion;
    }


    void Update()
    {   
        if(exploded)return;

        countDown-=Time.deltaTime;

        if (countDown <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        Collider[] colliders=Physics.OverlapSphere(transform.position,radiusExplosion);

        foreach(Collider c in colliders)
        {
            Rigidbody rb=c.GetComponent<Rigidbody>();
            Entity e = c.GetComponent<Entity>();
            
            //Comprobamos si estan en el child los componentes
            if (rb == null)rb = c.GetComponentInParent<Rigidbody>();
            
            if (e == null)e=c.GetComponentInChildren<Entity>();

            //CAplicamos el efecto de la granada a los componentes

            if (rb!=null)rb.AddExplosionForce(explosionForce,transform.position,radiusExplosion);
          
            if (e != null)e.TakeDamage(damage,transform.position);
           
        }
        exploded=true;

        Destroy(gameObject);
    }
}
