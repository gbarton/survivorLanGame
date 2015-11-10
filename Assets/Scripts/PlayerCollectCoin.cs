using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof (CharacterController))]
public class PlayerCollectCoin : NetworkBehaviour {

	GameManagerCoinSpawner coin;
	Transform myTransform;

	private Text coinText;

	[SyncVar (hook="OnCoinsChanged")]
	private int coinsCollected = 0;


	// Use this for initialization
	public override void OnStartClient () {
		base.OnStartClient ();
		coin = GetComponent<GameManagerCoinSpawner> ();
		myTransform = transform;
		coinText = GameObject.Find ("Coin Text").GetComponent<Text> ();
		SetCoinText ();
		Debug.Log ("coin spawner start");
		
	}

	// basically we only update on a coin change.
	void OnCoinsChanged(int coins) {
		coinsCollected = coins;
		SetCoinText ();
	}
	
	void SetCoinText() {
		// update display
		if (isLocalPlayer) {
			Debug.Log("set coin text");
			coinText.text = "Coins: " + coinsCollected.ToString();
		}
	}
	
	// when we hit a trigger
	private void OnTriggerEnter (Collider other) {
		
		if ("Coin" == other.tag) {
			Debug.Log ("HIT: " + other.name + "  " + myTransform.name); 
			// collect coin
			//TODO: BUG
			CmdTellServerCoinWasCollected(other.name,myTransform.name);
			
		}
	}

	// increments the coin and removes from the game
	[Command]
	public void CmdTellServerCoinWasCollected(string coinId, string playerId) {
		Debug.Log (playerId + " collected coin: " + coinId);
		GameObject go = GameObject.Find (coinId);
		Destroy (go);
		coinsCollected++;
	}

}
