using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	private int damage = 2;
	private float range = 200f;
	[SerializeField]
	private Transform camTransform;
	private RaycastHit hit;


	// Update is called once per frame
	void Update () {
		CheckIfShooting ();
	}

	void CheckIfShooting() {
		if (!isLocalPlayer) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Shoot();
		}
	}


	void Shoot() {
		if(Physics.Raycast(camTransform.TransformPoint(0,0,0.5f), camTransform.forward, out hit, range)) {

			if(hit.transform.tag == "Player") {
				string identity = hit.transform.name;
				CmdTellServerWhoWasShot(identity,damage);
			} else if (hit.transform.tag == "Zombie") {
				string identity = hit.transform.name;
				CmdTellServerWhichZombieWasShot(identity,damage);
			}
		}
	}

	[Command]
	void CmdTellServerWhichZombieWasShot(string uniqueIdentity, int damage) {
		GameObject go = GameObject.Find (uniqueIdentity);
		
		go.GetComponent<ZombieHealth> ().DeductHealth (damage);
	}


	[Command]
	void CmdTellServerWhoWasShot(string uniqueIdentity, int damage) {
		GameObject go = GameObject.Find (uniqueIdentity);

		go.GetComponent<PlayerHealth> ().DeductHealth (damage);
	}

}
