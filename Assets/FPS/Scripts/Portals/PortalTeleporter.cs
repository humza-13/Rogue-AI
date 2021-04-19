using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {

	public Transform player;
	public Transform reciever;

	private bool playerIsOverlapping = false;

	// Update is called once per frame
	void Update () {
		if (playerIsOverlapping)
		{
			player.transform.TransformPoint(new Vector3(730.5f, 0.0109f, -513.2f));
			playerIsOverlapping = false;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		
			Debug.Log("ENTER");
			Debug.Log(player.transform.position);
			playerIsOverlapping = true;
			
	}

		void OnTriggerExit (Collider other)
		{
			if (other.tag == "Player")
			{
				playerIsOverlapping = false;
				Debug.Log("EXIT");
				Debug.Log(player.transform.position);
			
			}
		}
}
