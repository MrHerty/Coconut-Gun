using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private bool hasJumped = false;
	private bool isOnGround = false;

	void Update()
	{
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);

		if (!Input.GetKey ("space")) //if the space isnt pressed...
		{
			hasJumped = false;// ...you have not jumped
		}

		if (!hasJumped && Input.GetKeyDown("space") && isOnGround) //jumping requires you to not have jumped and press space while on the ground
		{
			transform.Translate (0, 3, 0);
			hasJumped = true;
		}
	}

	
}