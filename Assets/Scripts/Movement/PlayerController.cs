using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement), typeof(Look))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference sprintAction;

    [SerializeField] private InputActionReference jumpAction;

    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference rechargeAction;



    [Header("Movement")]
    private Movement movement;
    private Look look;

    private float horizontal;
    private float vertical;
    private bool isSprinting;
    private bool requestJump;

    private bool requestShoot;


    [Header("Weapon")] 
    private Weapon weapon;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        look = GetComponent<Look>();
        weapon= GetComponentInChildren<Weapon>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //EL cursor desapareix i bloquejem el moviment del ratolí a la finestra
        Cursor.visible = false;
    }


    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        sprintAction.action.Enable();
        jumpAction.action.Enable();
        shootAction.action.Enable();
        rechargeAction.action.Enable();

    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        sprintAction.action.Disable();
        jumpAction.action.Disable();
        shootAction.action.Disable();
        rechargeAction.action.Disable();

        Cursor.lockState=CursorLockMode.None;//Al acabar resetejem tot
        Cursor.visible=true;

    }

    private void Update()
    {   
        //Llegim els inputs de totes les accions
        Vector2 move = moveAction.action.ReadValue<Vector2>();//Llegim els valors dels inputs d'aquesta acció
        horizontal = move.x;
        vertical = move.y;

        isSprinting = sprintAction.action.IsPressed();

            
        if (jumpAction.action != null && jumpAction.action.WasPressedThisFrame())requestJump = true;
        
        look.Rotate(lookAction.action.ReadValue<Vector2>());

        if(rechargeAction.action.WasPressedThisFrame())weapon.Recharge();

        if(shootAction.action!=null&&shootAction.action.WasPressedThisFrame())requestShoot=true;
        
    }


    private void FixedUpdate()
    {
        //Pasem tota la informació dels inputs a la clase encarregada de gestionar el moviment
        movement.MoveEntity(horizontal, vertical,isSprinting,requestJump);
        if(requestShoot)weapon.Shoot();
        requestJump=false;
        requestShoot=false;
    }
}