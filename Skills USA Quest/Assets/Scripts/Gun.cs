using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(SwitchingWeapon))]
public class Gun : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";

    public Camera fpsCam;
    public GameObject impactEffect;
    [HideInInspector]
    public AudioManager aud;

    
    
    private bool cantFire = false;
    private float nextTimeToFire = 0f;

    public Animator animator;
    //Put Ammo Counting UI text here

    private string ammoCount = "#AMMO";
    public string shootSound = "Shoot";

    private PlayerWeapon currentWeapon;
    private SwitchingWeapon weaponManager;
    

    [SerializeField]
    private LayerMask mask;

    private void Awake()
    {
        aud = FindObjectOfType<AudioManager>();//finds and assigns the scene's AudioManager GameObject
        weaponManager = GetComponent<SwitchingWeapon>();
    }

    private void OnEnable()
    {
        cantFire = false;
        animator.SetBool("Reloading", false);
        animator.SetBool("Sprinting", false);
    }

    // Update is called once per frame
    void Update () {

        currentWeapon = weaponManager.GetCurrentWeapon();

        ammoCount = currentWeapon.currentAmmo.ToString();

        if((currentWeapon.currentAmmo <= 0 || Input.GetKey("r")) && !cantFire && currentWeapon.currentAmmo != currentWeapon.maxAmmo)//checks if able to reload
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("MainFire") && Time.time >= nextTimeToFire && !cantFire)//checks for firing
        {
            nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;
            aud.Play(shootSound);
            Shoot();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Sprinting", true);
            cantFire = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StartCoroutine(StopSprinting());
        }
    }

    IEnumerator StopSprinting()
    {
        animator.SetBool("Sprinting", false);

        yield return new WaitForSeconds(currentWeapon.unSprintTime);
        
        cantFire = false;
    }

    IEnumerator Reload()
    {
        cantFire = true;
        Debug.Log("Reloading my gun...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(currentWeapon.reloadTime -.25f);
        currentWeapon.currentAmmo = currentWeapon.maxAmmo;

        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);

        cantFire = false;
        Debug.Log("Reloaded!");
    }

    [Client]
    void Shoot()//where the shooting is performed
    {
        currentWeapon.getMuzzleFlash().Play();
        currentWeapon.currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, currentWeapon.range, mask))
        {
            Debug.Log("you hit " + hit.transform.name);
            Target targ = hit.transform.GetComponent<Target>();
            if(targ != null)
            {
                targ.takeDamage(currentWeapon.damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * currentWeapon.hitForce);
            }

            if(hit.collider.tag == PLAYER_TAG && !hit.transform.GetComponent<PlayerManager>().isDead)
            {
                CmdPlayerShot(hit.transform.name, currentWeapon.damage);
                aud.Play("hitSound");
            }

            GameObject impactobj= Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactobj, 2f);
        }
    }

    [Command]
    void CmdPlayerShot (string _PlayerID, float damage)
    {
        Debug.Log(_PlayerID +" has been shot");

        PlayerManager targetPlayer = GameManager.GetPlayer(_PlayerID);

        targetPlayer.RpcTakeDamage(damage);
    }
}
