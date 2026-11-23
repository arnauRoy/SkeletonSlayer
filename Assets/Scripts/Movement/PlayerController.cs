using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Movement), typeof(Look))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]private Hud hud;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference sprintAction;

    [SerializeField] private InputActionReference jumpAction;

    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference rechargeAction;
    [SerializeField] private InputActionReference throwGrenadeAction;


    [Header("Movement")]
    private Movement movement;
    private Look look;
    private bool blockByUi;

    private float horizontal;
    private float vertical;
    private bool isSprinting;
    private bool requestJump;

    [Header("Weapon")] 
    private Weapon weapon;

    [SerializeField] private GameObject grenade;
    [SerializeField] private float throwGrenadeForce=10f;

    [SerializeField] private Transform holderGrenade;



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
        throwGrenadeAction.action.Enable();

        hud.OnInteractionUI+=OnPlayerInUi;

    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        sprintAction.action.Disable();
        jumpAction.action.Disable();
        shootAction.action.Disable();
        rechargeAction.action.Disable();
        throwGrenadeAction.action.Disable();


        hud.OnInteractionUI-=OnPlayerInUi;


        Cursor.lockState=CursorLockMode.None;//Al acabar resetejem tot
        Cursor.visible=true;

    }

    private void OnPlayerInUi(bool inUi)
    {
        blockByUi = inUi;
        // Negamos inUi para que isActive signifique "activo en juego" y no "en UI"
        SetLookAndWeaponActive(!inUi);
    }

    //-------------------[Actualizaciones de input]-------------------
    private void Update()
    {   
        HandleMovementInput();

        if(!blockByUi)HandleWeaponInput();

        if(!blockByUi)HandleGrenadeInput();
        
    }

    private void HandleMovementInput()
    {
        //Llegim els inputs de totes les accions
        Vector2 move = moveAction.action.ReadValue<Vector2>();//Llegim els valors dels inputs d'aquesta acció
        horizontal = move.x;
        vertical = move.y;

        isSprinting = sprintAction.action.IsPressed();

        if (jumpAction.action != null && jumpAction.action.WasPressedThisFrame())requestJump = true;

        if(!blockByUi)look.Rotate(lookAction.action.ReadValue<Vector2>());
    }


    private void HandleWeaponInput()
    {
        if (weapon != null)
        {
            if (rechargeAction.action != null && rechargeAction.action.WasPressedThisFrame())
                weapon.Recharge();

            if (shootAction.action != null && shootAction.action.WasPressedThisFrame())
                weapon.Shoot();
        }
    }

    private void HandleGrenadeInput()
    {
        if (throwGrenadeAction.action.WasPressedThisFrame())
        {   
           GameObject explosiveGrenade = Instantiate(
                grenade,
                holderGrenade.position,
                holderGrenade.rotation
            );
            Rigidbody rb=explosiveGrenade.GetComponent<Rigidbody>();

            rb.linearVelocity=holderGrenade.transform.forward*throwGrenadeForce;
        }
    }

    private void SetLookAndWeaponActive(bool isActive)
    {
        if (look != null) look.enabled = isActive;
        if (weapon != null) weapon.enabled = isActive;

        Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isActive;
    }


    private void FixedUpdate()
    {
        //Pasem tota la informació dels inputs a la clase encarregada de gestionar el moviment
        movement.MoveEntity(horizontal, vertical,isSprinting,requestJump);
        requestJump=false;
    }
}