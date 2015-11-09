using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

	[SerializeField] Camera FPSCharacterCam;
	[SerializeField] AudioListener audioListener;

	// Use this for initialization
	void Start () {
		if (isLocalPlayer) {
			GameObject.Find("SceneCamera").SetActive(false);

			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
			//GetComponent<CharacterController>().enabled = true;
			FPSCharacterCam.enabled = true;
			audioListener.enabled = true;
		}
	}

}
