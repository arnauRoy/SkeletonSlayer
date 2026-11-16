using UnityEngine;

public class GameSession : MonoBehaviour
{   

    public static GameSession Instance { get; private set; }
    private Wallet wallet = new Wallet();

    void Awake()
    {
        
    }
}
