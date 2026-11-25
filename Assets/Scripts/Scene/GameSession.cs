using UnityEngine;

public class GameSession : MonoBehaviour
{   

    public static GameSession Instance { get; private set; }
    public Wallet Wallet { get; private set; } = new Wallet();

    public Inventory Inventory{ get; private set; }=new Inventory();

    //Al arrancar el juego este se crea automaticamente
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]//BeforeSceneLoad garantiza que se cree antes de cualquier onEnable
    private static void Create()
    {
        if (Instance != null) return;
        new GameObject("GameSession").AddComponent<GameSession>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // ya existe una: esta sobra
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);//Mantiene vivo el objeto entre escenas
    }
}
