using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugManager : MonoBehaviour
{
    private GameSession gameSession;

    [SerializeField]private InputActionReference gainMoney; 

    void Awake()
    {
        gameSession=GameSession.Instance;
    }
    void Update()
    {
        // Quitar vida con la tecla H
        if (gainMoney.action.WasPressedThisFrame())
        {
            gameSession.Wallet.GainMoney(1000);
        }
    }
}