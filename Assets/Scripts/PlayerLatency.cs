using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerLatency : NetworkBehaviour {

	private NetworkClient nClient;
	private int latency;
	private Text latencyText;

	// Use this for initialization
	public override void OnStartLocalPlayer () {
		nClient = GameObject.Find("NetworkManager").GetComponent<NetworkManager>().client;
		latencyText = GameObject.Find("Latency Text").GetComponent<Text>();

	}

	void FixedUpdate () {
		ShowLatency ();
	}

	void ShowLatency() {
		if (isLocalPlayer) {
			latency = nClient.GetRTT();
			latencyText.text = latency.ToString();
		}
	}

}
