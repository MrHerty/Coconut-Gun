using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	//The Static property of this variable means that the
	//value will be shared across all instances of the Coin class
	public static int coinCount = 0;

	void Awake()
	{
		//Reset the CoinCount each time a new scene is loaded
		Coin.coinCount = 0;
	}

	void Start () 
	{
		//Object created, increment coin count
		++Coin.coinCount;
	}

	void OnTriggerEnter(Collider col)
	{
		//If player collected coin, then destroy object
		if(col.gameObject.tag == "Player")
			Destroy(gameObject);
	}

	void OnDestroy()
	{
		//Object Destroyed, remove one from total coin count
		--Coin.coinCount;

		//Check remaining coins
		if(Coin.coinCount <= 0)
		{
			//Game is won. Collected all coins
			//Destroy Timer and launch fireworks
			GameObject timer = GameObject.Find("LevelTimer");
			Destroy(timer);

			GameObject[] FireworkSystems = GameObject.FindGameObjectsWithTag("Fireworks");
			foreach(GameObject GO in FireworkSystems)
			{
				GO.GetComponent<ParticleSystem>().Play();
			}
		}
	}
}


