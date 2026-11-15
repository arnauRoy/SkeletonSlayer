using UnityEngine;

//Clase encargada del movimiento del jugador
public class Movement : MonoBehaviour
{   

    [SerializeField]private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float movementSpeed=5f;
    [SerializeField] private float sprintSpeed=7f;

    
    
    //Funcion que genera todo el movimiento
    public void MoveEntity(float horizontal, float vertical, bool isSprinting)
    {
        Vector3 direction = (transform.right*horizontal+transform.forward*vertical).normalized;//Creem el vector amb les dades registrades de les accions

        //Afegim la velocitat de moviment
        float yDirection=rb.linearVelocity.y; 
        float Xdirection=direction.x*movementSpeed;
        float Zdirection=direction.z*movementSpeed;
        
        if(isSprinting)//Si estroba corrent es multiplica per una altre velocitat
        {
            Xdirection=direction.x*sprintSpeed;
            Zdirection=direction.z*sprintSpeed;
        
        }
        
        rb.linearVelocity= new Vector3(
        Xdirection,
        yDirection,
        Zdirection
        );

    }
    
}

