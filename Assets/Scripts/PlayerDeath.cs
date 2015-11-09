using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerDeath : NetworkBehaviour {

	private PlayerHealth healthScript;

	private Image crossHairImg;

	// runs for all clients
	public override void PreStartClient () {
		base.PreStartClient ();
		healthScript = GetComponent<PlayerHealth> ();
		
		// subscribe to event... funky syntax
		healthScript.EventDie += DisablePlayer;
	}

	// Use this for initialization
	public override void OnStartLocalPlayer () {
		crossHairImg = GameObject.Find ("CrossHairImage").GetComponent<Image> ();
	}

	// unsubscribing
	public override void OnNetworkDestroy() {
		healthScript.EventDie -= DisablePlayer;
	}


	void DisablePlayer() {
		GetComponent<CharacterController> ().enabled = false;
		GetComponent<PlayerShoot> ().enabled = false;
		GetComponent<BoxCollider> ().enabled = false;

		Renderer[] renderers = GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in renderers) {
			r.enabled = false;
		}

		healthScript.isDead = true;

		if (isLocalPlayer) {
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = false;
			crossHairImg.enabled = false;
			// enable respawn button
			GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().respawnButton.SetActive(true);

		}

	}

}
