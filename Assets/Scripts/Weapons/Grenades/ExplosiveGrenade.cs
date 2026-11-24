using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGrenade : MonoBehaviour
{
    //----------------[GrenadeStats]----------------
    private float countDown;
    private bool exploded = false;

    [SerializeField] private float explosionForce = 15f; // Con Impulse, valores entre 10 y 25 funcionan bien
    [SerializeField] private float upwardsModifier = 1.2f; // Levanta un poco al objetivo para vencer la fricción del suelo
    [SerializeField] private float delayExplosion = 5f;
    [SerializeField] private float radiusExplosion = 5f;
    [SerializeField] private int damage = 60;

    [Header("Visual Effects")]
    [SerializeField] private GameObject explosionEffectPrefab;

    void Start()
    {
        countDown = delayExplosion;
    }

    void Update()
    {
        if (exploded) return;

        countDown -= Time.deltaTime;

        if (countDown <= 0f)
        {
            Explode();
        }
    }

    void Explode()
    {
        exploded = true;
        if(explosionEffectPrefab!=null) Instantiate(explosionEffectPrefab,transform.position,Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusExplosion);

        // Evita aplicar daño o fuerza dos veces a un mismo objeto con múltiples colliders
        //Coleccion que no guarda duplicados
        HashSet<Entity> damagedEntities = new HashSet<Entity>();
        HashSet<Rigidbody> pushedRigidbodies = new HashSet<Rigidbody>();

        foreach (Collider c in colliders)
        {
            // 1. Daño (buscamos en el propio objeto o en sus padres)
            Entity e = c.GetComponentInParent<Entity>();
            
            if (e != null && damagedEntities.Add(e))//Al añadir devuelve bool si se ha añadido y este objeto no estaba ya
            {
                e.TakeDamage(damage, transform.position);
            }

            // 2. Empuje físico
            Rigidbody rb = c.attachedRigidbody;//Devuelve directamente el rigidbody que contrala la fisica del collider
            //Evita usar GetComponentInParent y GetComponentInChildren
            if (rb != null && !rb.isKinematic && pushedRigidbodies.Add(rb))
            {
                rb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    radiusExplosion,
                    upwardsModifier,
                    ForceMode.Impulse
                );
            }
        }

        Destroy(gameObject);
    }
    private void OnDrawGizmos()
{
    // Relleno semitransparente para apreciar el volumen completo
    Gizmos.color = new Color(1f, 0.3f, 0f, 0.25f);
    Gizmos.DrawSphere(transform.position, radiusExplosion);

    // Borde exterior bien definido
    Gizmos.color = new Color(1f, 0.3f, 0f, 1f);
    Gizmos.DrawWireSphere(transform.position, radiusExplosion);
}
}