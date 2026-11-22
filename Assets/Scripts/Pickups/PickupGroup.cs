using UnityEngine;

public class PickupGroup : MonoBehaviour
{
    private Pickup[] pickups;

    [SerializeField] private bool startHidden=true;

    void Awake()
    {
        pickups=GetComponentsInChildren<Pickup>();
    }

    void Start()
    {
        if (startHidden) DeactivateAll();
    }

    public void DeactivateAll()
    {
        if(pickups==null||pickups.Length<=0)return;

        foreach(Pickup p in pickups) p.Deactivate();
    }
    public void ActivateAll()
    {   
        if(pickups==null||pickups.Length<=0)return;

        foreach(Pickup p in pickups) p.Activate();
        
    }
}
