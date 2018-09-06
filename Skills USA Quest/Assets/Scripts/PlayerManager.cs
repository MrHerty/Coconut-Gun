using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerManager : NetworkBehaviour {

    #region Setting up

    public static PlayerManager instance;
    public GameObject player;

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private float maxHealth = 150f;
    [SyncVar]
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();

        GameManager.RegisterPlayer(transform.GetComponent<NetworkIdentity>().netId.ToString(), this);
    }

    public void SetDefaults()
    {
        isDead = false;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        instance = this;
        currentHealth = maxHealth;

        Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = true;
        }
    }

    #endregion

    [ClientRpc]
    public void RpcTakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;

        Debug.Log(transform.name + " current health: " + currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        //DISABLE COMPONENTS

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Debug.Log(transform.name +" got fragged");
        
        //Call respawn method
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        SetDefaults();
        Setup();

        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log(transform.name + " Respawned");
    }
}
