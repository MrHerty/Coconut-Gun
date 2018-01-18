using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {


	public AudioSource audioSource;
	public bool willToggleOnActive = false;
	public GameObject gate;

	public bool forPlayer = true;
	public bool forPuzzle = false;


	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Player" && forPlayer)
		{
			Debug.Log ("Button Pressed");

			audioSource.Play ();
			gate.SetActive (false);

			if (willToggleOnActive) 
			{
				gameObject.SetActive (false);
			}
		}
	}

	void OnTriggerStay(Collider collider)
	{
		if(collider.gameObject.tag == "Puzzle" && forPuzzle)
		{
			audioSource.Play ();
			gate.SetActive (false);
			gameObject.SetActive (false);
		}
	}
}
