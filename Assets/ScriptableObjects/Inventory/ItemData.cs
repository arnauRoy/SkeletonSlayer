using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string id;              // "grenade", para guardar en disco más adelante
    public string displayName;     // lo que ve el jugador
    public Sprite icon;            // para el HUD
    public int maxStack = 5;       // cuántas caben
    public GameObject prefab;      // lo que se crea al usarlo
}
