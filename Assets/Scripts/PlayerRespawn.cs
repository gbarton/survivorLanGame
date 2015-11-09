using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerRespawn : NetworkBehaviour {

	private PlayerHealth healthScript;
	private Image crossHairImage;
	private GameObject respawnButton;

	public override void PreStartClient () {
		base.PreStartClient ();
		healthScript = GetComponent<PlayerHealth> ();
		healthScript.EventRespawn += EnablePlayer;
	}

	// Use this for initialization for local player only
	// isLocalPlayer guarenteed to be set on this method
	public override void OnStartLocalPlayer () {
		crossHairImage = GameObject.Find ("CrossHairImage").GetComponent<Image> ();
		SetRespawnButton ();
	}

	void SetRespawnButton() {
		if (isLocalPlayer) {
			respawnButton = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().respawnButton;
			respawnButton.GetComponent<Button>().onClick.AddListener(CommenceRespawn);
			respawnButton.SetActive(false);
		}
	}

	// unsubscribe when disconnected
	public override void OnNetworkDestroy() {
		healthScript.EventRespawn -= EnablePlayer;
	}

	void EnablePlayer() {
		GetComponent<CharacterController> ().enabled = true;
		GetComponent<PlayerShoot> ().enabled = true;
		GetComponent<BoxCollider> ().enabled = true;
		
		Renderer[] renderers = GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in renderers) {
			r.enabled = true;
		}
		
		if (isLocalPlayer) {
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
			crossHairImage.enabled = true;
			respawnButton.SetActive(false);
		}
	}

	void CommenceRespawn() {
		CmdRespawnOnServer ();
	}

	[Command]
	void CmdRespawnOnServer() {
		healthScript.ResetHealth ();
	}
}
