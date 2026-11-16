using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask entityMask;

    [SerializeField] private int damage;

    public void Initialize(Collider ownerCollider,int damage)
    {
        Collider bulletCollider=GetComponent<Collider>();

        this.damage=damage;

        Physics.IgnoreCollision(
            bulletCollider,
            ownerCollider
        );
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
                entity.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
