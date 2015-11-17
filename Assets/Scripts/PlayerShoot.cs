using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerShoot : NetworkBehaviour {

	private float fireRate = 0.2f;
	private int gunAmmoConsumption = 1;
	private float lastShot = 0.0f;
	private int damage = 2;
	private float range = 200f;
	[SerializeField]
	private Transform camTransform;
	private RaycastHit hit;

	//managed server side
	[SyncVar]
	private int ammo = 100;
	private Text ammoText;

	// Use this for initialization
	public override void OnStartLocalPlayer () {
		ammoText = GameObject.Find ("Ammo Text").GetComponent<Text> ();
		StartCoroutine (Replenish ());

	}


	// only started on local player
	IEnumerator Replenish() {
		for (;;) {
			yield return new WaitForSeconds(6.0f);
			AddAmmo();
		}
	}

	// only started on server
	public void AddAmmo() {
		if (ammo < 100) {
			ammo += 1;
			SetAmmoText();
		}
	}

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
		// can i shoot?
		if (lastShot + fireRate <= Time.time) {
			// update shot time
			lastShot = Time.time;
			ammo -= gunAmmoConsumption;
			SetAmmoText ();

			if (Physics.Raycast (camTransform.TransformPoint (0, 0, 0.5f), camTransform.forward, out hit, range)) {

				if (hit.transform.tag == "Player") {
					string identity = hit.transform.name;
					CmdTellServerWhoWasShot (identity, damage);
				} else if (hit.transform.tag == "Zombie") {
					string identity = hit.transform.name;
					CmdTellServerWhichZombieWasShot (identity, damage);
				}
			}
		} else {
			Debug.Log ((lastShot + fireRate) + " " + Time.time);
		}
	}


	void SetAmmoText() {
		if (isLocalPlayer) {
			ammoText.text = "Ammo: " + ammo.ToString();
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
