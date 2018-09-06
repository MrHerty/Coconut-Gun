using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public float damage = 10f;
    public float range = 100f;
    public float hitForce = 20f;
    public float fireRate = 15f;
    public int maxAmmo = 25;
    public float reloadTime = 3f;
    public float unSprintTime = .3f;
    
    [HideInInspector]
    public int currentAmmo;

    public GameObject graphics;
    
    private ParticleSystem muzzleFlash;

    private void Start()
    {
        currentAmmo = maxAmmo;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }

    public ParticleSystem getMuzzleFlash()
    {
        return muzzleFlash;
    }
}
