using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncRotation : NetworkBehaviour {

	[SyncVar]
	private Quaternion syncPlayerRotation;
	[SyncVar]
	private Quaternion syncCamRotation;

	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private Transform camTransform;
	[SerializeField]
	private float lerpRate = 15;

	private Quaternion lastPlayerRot;
	private Quaternion lastCam;
	private float threshold = 5f;

	// Use this for initialization
	void Start () {
	
	}

	void Update() {
		LerpRotations ();
	}

	void FixedUpdate () {
		TransmitRotations ();
	}


	void LerpRotations() {
		if (!isLocalPlayer) {
			playerTransform.rotation = Quaternion.Lerp(
				playerTransform.rotation,
				syncPlayerRotation,
				Time.deltaTime* lerpRate);
			camTransform.rotation = Quaternion.Lerp(
				camTransform.rotation,
				syncCamRotation,
				Time.deltaTime * lerpRate);
		}
	}

	[Command]
	void CmdProvideRotataionsToServer(Quaternion playerRot, Quaternion camRot) {
		syncPlayerRotation = playerRot;
		syncCamRotation = camRot;
	}

	[Client]
	void TransmitRotations() {
		if (isLocalPlayer && (Quaternion.Angle(playerTransform.rotation,lastPlayerRot) > threshold  
		                      || Quaternion.Angle(camTransform.rotation, lastCam) > threshold)) {
			CmdProvideRotataionsToServer(playerTransform.rotation, camTransform.rotation);
			lastCam = camTransform.rotation;
			lastPlayerRot = playerTransform.rotation;
		}
	}
}
