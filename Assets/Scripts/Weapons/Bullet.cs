using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask entityMask;

    [SerializeField] private int damage;
    private Vector3 shotOrigin;

    public void Initialize(Entity owner, int damage)
    {
        this.damage = damage;
        shotOrigin = owner.transform.position;   // se guarda ahora: el dueño se moverá después

        Collider bulletCollider = GetComponentInChildren<Collider>();

        foreach (Collider c in owner.GetComponentsInChildren<Collider>())//Por cada componente con collider en el owner lo ignora en la collision
        {
            if (c.enabled) Physics.IgnoreCollision(bulletCollider, c);
        }
    }

    void OnCollisionEnter(Collision collision)
    {   /*La expresión completa hace tres cosas:
        1. Obtiene las Layers que has seleccionado en el Inspector (entityMask.value).
        2. Crea una máscara temporal con únicamente la Layer del objeto golpeado (1 << collision.gameObject.layer).
        3. Compara ambas máscaras con & para saber si comparten algún bit activado.*/
        if((entityMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            
            if (collision.gameObject.TryGetComponent<Entity>(out Entity entity))
            {
                entity.TakeDamage(damage,shotOrigin);
                Destroy(gameObject);
            }
        }
    }
}
