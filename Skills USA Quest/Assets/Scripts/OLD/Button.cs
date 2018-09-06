using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Button : MonoBehaviour {

    public AudioManager aud;
	public AudioSource buttonSound;
	public bool willToggleOnActive = false;
	public GameObject gate;

	public bool forPlayer = true;
	public bool forPuzzle = false;

    private void Awake()
    {
        aud = FindObjectOfType<AudioManager>();
    }


    void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Player" && forPlayer)
		{
			Debug.Log ("Button Pressed");

            aud.Play("activateButton");
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
            aud.Play("activateButton");
			gate.SetActive (false);
			gameObject.SetActive (false);
		}
	}
}
