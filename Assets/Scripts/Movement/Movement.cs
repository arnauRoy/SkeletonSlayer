    using UnityEngine;

    //Clase encargada del movimiento del jugador
    public class Movement : MonoBehaviour
    {   

        [SerializeField]private Rigidbody rb;

        [SerializeField] private Transform groundCheck;

        [SerializeField] private float groundRadius=0.25f;

        [SerializeField] private LayerMask groundMask;

        [Header("Movement")]
        [SerializeField] private float movementSpeed=5f;
        [SerializeField] private float sprintSpeed=7f;
        [SerializeField] private float jumpSpeed=5f;

        private bool onGround;

        
        
        //Funcion que genera todo el movimiento
        public void MoveEntity(float horizontal, float vertical, bool isSprinting, bool jumpRequested)
        {
            // ¿hay suelo bajo los pies?
            onGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

            // dirección horizontal, relativa a hacia dónde mira el jugador
            Vector3 direction = (transform.right * horizontal + transform.forward * vertical).normalized;

            float speed = isSprinting ? sprintSpeed : movementSpeed;

            // se conserva la velocidad vertical que ya tiene el Rigidbody (gravedad)
            float yVelocity = rb.linearVelocity.y;

            if (jumpRequested && onGround && yVelocity <= 0.1f)
            {
                yVelocity = jumpSpeed;
            }

            rb.linearVelocity = new Vector3(
                direction.x * speed,
                yVelocity,
                direction.z * speed
            );
        }
        
    }

