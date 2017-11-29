using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {


	public AudioSource audioSource;


	void onCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			Debug.Log ("Button Pressed");
			audioSource.Play ();
		}
	}
}
