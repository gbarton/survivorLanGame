using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkManagerCustom :NetworkManager {

	public void StartupHost() {
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinGame() {
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();
	}

	void SetIPAddress() {
		string ipAddress = GameObject.Find ("InputFieldIPAddress").transform.FindChild ("Text").GetComponent<Text> ().text;
		if (ipAddress == null || ipAddress.Length < 1) {
			ipAddress = "localhost";
		}
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	void SetPort() {
		NetworkManager.singleton.networkPort = 7777;
	}

	void OnLevelWasLoaded(int level) {
		if (level == 0) {
			SetupMenuSceneButtons ();
		} else {
			SetupOtherSceneButtons();
		}
	}

	void SetupMenuSceneButtons() {
		GameObject.Find ("StartHostButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("StartHostButton").GetComponent<Button> ().onClick.AddListener (StartupHost);

		GameObject.Find ("JoinGameButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("JoinGameButton").GetComponent<Button> ().onClick.AddListener (JoinGame);
	}

	void SetupOtherSceneButtons() {
		GameObject.Find ("QuitButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("QuitButton").GetComponent<Button> ().onClick.AddListener (NetworkManager.singleton.StopHost);
	}


}
