using UnityEngine;
using System;


public class Weapon : MonoBehaviour
{ 

    [SerializeField] private Entity owner;
    //---------------Posiciones---------------
    [SerializeField] private Transform origin;
    [SerializeField] private Transform spawnPoint;
    
    
    //---------------Prefab bala---------------
    [SerializeField] private GameObject bullet;

    //---------------[Stats del arma]---------------
    [SerializeField] private int damage=20;
   [SerializeField] private float maxDistance=15f;

    //---------------[Stats de la municion]---------------

    [SerializeField] private int ammo=120;

    [SerializeField] private int maxAmmoInCharger=7;//Cuantas balas caben en el cargador
    [SerializeField] private int ammoInMagazine = 7; //Cuantas tiene actualmente

    [SerializeField] private int maxAmmo=300;

    //---------------[Stats del disparo]---------------

    [SerializeField]private float bulletSpeed=60f;
    private float shootRate=0.5f;
    private float shotRateTime=0;

    //Avisa sobre la accion los subscriptores pueden hacer cosas en funcion de eso
    public event Action<int,int> AmmoUpdate; //Pasamos la municion total y la que tiene en el cargador

    private bool infiniteAmmo=false;

    //---------------[Animator]---------------
    [SerializeField]private WeaponAnimator weaponAnimator;


    void Awake()
    {
        owner=GetComponentInParent<Entity>();
        weaponAnimator=GetComponent<WeaponAnimator>();
    }
    private void ShowAmmoEvent(int ammo, int ammoInMagazine)
    {
        AmmoUpdate?.Invoke(ammo,ammoInMagazine);
    }

    public void Recharge()
    {
        if(ammoInMagazine>=maxAmmoInCharger || ammo<=0) return;

        int ammoNeeded = maxAmmoInCharger - ammoInMagazine;
        int ammoToLoad = Math.Min(ammoNeeded, ammo);//Coge el valor mas pequeño, si hay pocas balas las de ammo si no las que necesita

        ammoInMagazine += ammoToLoad;
        ammo -= ammoToLoad;

        ShowAmmoEvent(ammo,ammoInMagazine);
    }

    public void RecibeAmmo(int munition)
    {
        int ammoNeeded=maxAmmo-ammo;
        int ammoToLoad=Math.Min(ammoNeeded,munition);

        ammo+=ammoToLoad;

        ShowAmmoEvent(ammo,ammoInMagazine);
    }
    public void Shoot()
    {   
        Vector3 direction = origin.transform.forward;

        bool hasInfiniteAmmo=Time.time>shotRateTime&&infiniteAmmo;//Comprueba si ha pasada el tiempo y si es una entidad no tiene limite de balas
        bool hasBulletsAndTimePassed=Time.time>shotRateTime&&ammoInMagazine>0;//Comprueba si ha pasado el shotRate y tiene balas entonces en el cargador dispara la bala

        if ( hasBulletsAndTimePassed|| hasInfiniteAmmo) 
        {   

            GameObject newBullet;

            Vector3 aimPoint = origin.position + direction * maxDistance;

            if (Physics.Raycast(origin.position, direction, out RaycastHit hit, maxDistance))
            {
                aimPoint=hit.point;
            }

            Vector3 bulletDirection = (aimPoint - spawnPoint.position).normalized;
            //instanciación de una bala
            //Recibe el objeto,el origen y la rotacion
            newBullet=Instantiate(bullet,spawnPoint.position,Quaternion.LookRotation(bulletDirection)*bullet.transform.rotation);
            //Quaternion.LookRotation nos dala bala orientada a la dirección del disparo
            //newBullet.GetComponent<Rigidbody>().AddForce(bulletDirection*shotForce,ForceMode.Impulse);
            Bullet bulletScript=newBullet.GetComponent<Bullet>();
            bulletScript.Initialize(owner,damage);
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();

            rb.linearVelocity=bulletDirection*bulletSpeed;

            if(weaponAnimator!=null)weaponAnimator.shootAnimation();//Animmamos el disparo

            shotRateTime=Time.time+shootRate; //le suma el rate al tiempo actual limitando el disparo
            Destroy(newBullet,2);
            if(!infiniteAmmo) ShowAmmoEvent(ammo,--ammoInMagazine);
        }
    }
     
    //GETTERS
    public float MaxDistance
    {
        get{return maxDistance;}
    }

    public int Ammo
    {
        get{return ammo;}
    }

    public int MaxAmmo
    {
        get{return maxAmmo;}
    }


    public int AmmoInMagazine
    {
        get{return ammoInMagazine;}
    }

    public bool InfiniteAmmo
    {
        set{infiniteAmmo=value;}
    }
  
}
