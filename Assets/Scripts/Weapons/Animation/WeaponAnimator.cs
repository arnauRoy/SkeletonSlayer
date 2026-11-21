using System;
using UnityEngine;

    public class WeaponAnimator : MonoBehaviour
    {
        [Header("Prefab Refrences")]
        public GameObject casingPrefab;
        public GameObject muzzleFlashPrefab;

        [Header("Location Refrences")]
        [SerializeField] private Animator gunAnimator;
        [SerializeField] private Transform barrelLocation;
        [SerializeField] private Transform casingExitLocation;

        [Header("Settings")]
        [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
        [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

        private const String FIRE_TRIGGER="Fire";  
        void Start()
        {
            if (barrelLocation == null)
                barrelLocation = transform;

            if (gunAnimator == null)
                gunAnimator = GetComponentInChildren<Animator>();
        }

        //This function creates the bullet behavior

        public void shootAnimation()
        {   
            gunAnimator.SetTrigger(FIRE_TRIGGER);//La animacion se encarga de llamar a las funciones
            //AnimShoot();
            //CasingRelease();
        }
        void AnimShoot()
        {
            if (muzzleFlashPrefab)
            {
                //Create the muzzle flash
                GameObject tempFlash;
                tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

                //Destroy the muzzle flash effect
                Destroy(tempFlash, destroyTimer);
            }
        }

        //This function creates a casing at the ejection slot
        void CasingRelease()
        {
            //Cancels function if ejection slot hasn't been set or there's no casing
            if (!casingExitLocation || !casingPrefab)
            { return; }

            //Create the casing
            GameObject tempCasing;
            tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
            //Add force on casing to push it out
            tempCasing.GetComponent<Rigidbody>().AddExplosionForce(UnityEngine.Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
            //Add torque to make casing spin in random direction
            tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, UnityEngine.Random.Range(100f, 500f), UnityEngine.Random.Range(100f, 1000f)), ForceMode.Impulse);

            //Destroy casing after X seconds
            Destroy(tempCasing, destroyTimer);
        }

    }
