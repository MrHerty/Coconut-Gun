using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("You hit a killbox!");

            PlayerManager targetPlayer = GameManager.GetPlayer(other.transform.name);

            targetPlayer.RpcTakeDamage(9999f);
        }
    }
}
