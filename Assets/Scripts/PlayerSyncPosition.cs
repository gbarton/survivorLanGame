using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[NetworkSettings (channel = 0, sendInterval = 0.033f)]
public class PlayerSyncPosition : NetworkBehaviour {

	[SerializeField] 
	Transform myTransform;

	[SerializeField] 
	float lerpRate = 15;

	[SyncVar] // auto sync this var to all clients
	private Vector3 syncPos;
	private Vector3 lastPos;
	private float threshhold = 0.25f; // how much movement before sending

	private NetworkClient nClient;
	private int latency;
	private Text latencyText;

	void Start() {
		if (isLocalPlayer) {
			nClient = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().client;
			latencyText = GameObject.Find("Latency Text").GetComponent<Text>();
		}
	}

	void Update() {
		LerpPos ();
	}
	
	void FixedUpdate () {
		// send my position to server
		TransmitPosition ();
		ShowLatency ();
	}

	// smooth moving other players, not ourselves
	void LerpPos() {
		if (!isLocalPlayer) {
			myTransform.position = Vector3.Lerp(
				myTransform.position
				,syncPos, Time.deltaTime * lerpRate);
		}
	}

	[Command] // runs on server
	void CmdProvidePositionToServer(Vector3 pos) {
		syncPos = pos;
	}

	[ClientCallback] // client side
	void TransmitPosition() {
		if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshhold) {
			CmdProvidePositionToServer (myTransform.position);
			lastPos = myTransform.position;
		}
	}

	void ShowLatency() {
		if (isLocalPlayer) {
			latency = nClient.GetRTT();
			latencyText.text = latency.ToString();
		}
	}
}
