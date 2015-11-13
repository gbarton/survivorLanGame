using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

/**
 * This is server side global syncing at work
 * 1. Player hits an object, if its a coin, freeze hit checking
 * (to avoid dupes)
 * 2. Then send notice to server to inc collected coins and destroy coin
 * 3. Server updates its known collected count
 * which triggers a function that updates every player server side
 * 4. Since the var is a syncvar, it replicates out to the clients
 * 5. The clients have a trigger that checks for if they are the local player
 * and updates the text if they are.
 * 
 */
[RequireComponent(typeof (CharacterController))]
public class PlayerCollectCoin : NetworkBehaviour {

	Transform myTransform;

	private Text coinText;

	[SyncVar (hook="OnCoinsChanged")]
	private int coinsCollected = 0;

	GameManagerReferences refs;


	// Use this for initialization
	public override void OnStartClient () {
		base.OnStartClient ();

		myTransform = transform;
		refs = GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ();
		coinText = GameObject.Find ("Coin Text").GetComponent<Text> ();
		SetCoinText ();
		Debug.Log ("coin spawner start");
		
	}

	public bool isColliding = false;


	// basically we only update on a coin change.
	public void OnCoinsChanged(int coins) {
//		Debug.Log ("OncoinsChange " + myTransform.name + " " + isServer);

		coinsCollected = coins;
		SetCoinText ();
	}
	
	public void SetCoinText() {
		// update display for local player
		if (isLocalPlayer) {
//			Debug.Log("set coin text " + myTransform.name + " " + isServer);
			coinText.text = "Coins: " + coinsCollected.ToString();
		}
	}
	
	// when we hit a trigger
	private void OnTriggerEnter (Collider other) {

		if ("Coin" == other.tag) {
			if (isColliding)
				return;
			isColliding = true;

			//Debug.Log ("HIT: " + other.transform.name + "  " + myTransform.name + " " + isServer); 
			// collect coin server side
			CmdTellServerCoinWasCollected(other.transform.name,myTransform.name);
			
		}
	}

	// increments the coin and removes from the game on the server
	[Command]
	public void CmdTellServerCoinWasCollected(string coinId, string playerId) {
		Debug.Log (playerId + " collected coin: " + coinId + " " + isServer);
		GameObject go = GameObject.Find (coinId);
		Destroy (go);

		refs.coinsCollected = refs.coinsCollected + 1;
		// free up collider check, object is gone now so wont collide again
		isColliding = false;
	}

	[Server]
	public void AddCoin(int coin) {
		coinsCollected  = coin;
	}


}
