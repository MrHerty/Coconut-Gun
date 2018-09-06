using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyController : MonoBehaviour {

    public float lookRadius = 10f;

    Transform target;
    NavMeshAgent agent;


	// Use this for initialization
	void Start () {

        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
		
	}
	
	// Update is called once per frame
	void Update () {

        float dist = Vector3.Distance(target.position, transform.position);

        if (dist <= lookRadius)
        {
            agent.SetDestination(target.position);
        }
		
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
