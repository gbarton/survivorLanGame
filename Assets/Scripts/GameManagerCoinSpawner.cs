using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManagerCoinSpawner : NetworkBehaviour {

	// how big is our field/area
	public int range;
	private int numCoins = 10;

	[SerializeField]
	GameObject coinPrefab;



	// Use this for initialization
	public override void OnStartServer () {
//		coinPrefab = GameObject.FindGameObjectWithTag ("Coin");
		Debug.Log (coinPrefab);
		SpawnCoins ();
	}


	// spawns coins into the game
	public void SpawnCoins() {
		for (int i=0; i < numCoins; i++) {
			int ranX = Random.Range(-1*range,range);
			int ranZ = Random.Range(-1*range,range);
			//TODO: check for collisions, if so try again
			//Debug.Log ("spawning at: " + ranX + " " + ranZ);
			GameObject go = GameObject.Instantiate(coinPrefab,
			                                       new Vector3(ranX, 1 ,ranZ), 
			                                       Quaternion.AngleAxis(90, Vector3.back)) as GameObject;
			go.GetComponent<CoinId>().coinID = "Coin " + i;
			// this broadcasts it to the network
			NetworkServer.Spawn (go);
		}

	}

}
