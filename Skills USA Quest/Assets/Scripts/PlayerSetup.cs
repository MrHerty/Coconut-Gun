using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerManager))] 
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    Camera SceneCamera;
    string netID;
    PlayerManager playerManager;

    [SerializeField]
    string dontDrawLayerName = "DontDraw";

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUI;

    private GameObject playerUIInstance;

    private void Awake()
    {

        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {

        if (!isLocalPlayer)//if you are not controling this player...
        {
            DisableComponents();//disable what is in the list
            AssignRemoteLayer();//put other players on the remote layer
        }
        else
        {
            SceneCamera = Camera.main;
            if(SceneCamera != null)
            {
                SceneCamera.gameObject.SetActive(false);
            }

            //disable player graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //create player UI

            playerUIInstance = Instantiate(playerUI);
            playerUIInstance.name = playerUI.name;
        }

        GetComponent<PlayerManager>().Setup();
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach(Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        netID = GetComponent<NetworkIdentity>().netId.ToString();
        //GameManager.RegisterPlayer(netID, playerManager);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }
    private void OnDisable()
    {
        Destroy(playerUIInstance);
        if(SceneCamera != null)
        {
            SceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }
}
