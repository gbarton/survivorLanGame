using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManagerReferences : NetworkBehaviour {

	// dunno why this has to be outside player
	public GameObject respawnButton;

	// global range we can use for spawning purposes
	public static int levelSize = 500;

	[SyncVar (hook="UpdatePlayersVal")]
	public int coinsCollected = 0;

	// from server side update all players
	[Server]
	void UpdatePlayersVal(int coins) {
		coinsCollected = coins;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject p in players) {
			p.GetComponent<PlayerCollectCoin> ().AddCoin(coinsCollected);
		}
	}

}
