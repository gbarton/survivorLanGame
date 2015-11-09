using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GameManagerZombieSpawner : NetworkBehaviour {

	[SerializeField]
	GameObject zombiePrefab;

	GameObject[] zombieSpawns;

	private int counter;
	private int numberOfZombies = 10;
	private int maxNumberOfZombies = 80;
	private float waveRate = 10;
	private bool isSpawnActivated = true;

	public override void OnStartServer () {
		zombieSpawns = GameObject.FindGameObjectsWithTag("ZombieSpawn");
		Debug.Log ("started");
		StartCoroutine (ZombieSpawner ());

	}

	IEnumerator ZombieSpawner() {
		for(;;) {
			yield return new WaitForSeconds(waveRate);
			Debug.Log ("Time passed");
			GameObject[] zombies = GameObject.FindGameObjectsWithTag	 ("Zombie");
			if(zombies.Length < maxNumberOfZombies) {
				CommenceSpawn();
			}
		}
	}

	void CommenceSpawn() {
		if (isSpawnActivated) {
			for( int i = 0; i < numberOfZombies; i++) {
				int rand = Random.Range (0,zombieSpawns.Length);
				SpawnZombies(zombieSpawns[rand].transform.position);
			}
		}
	}

	void SpawnZombies(Vector3 spawnPos) {
		GameObject go = GameObject.Instantiate (zombiePrefab, spawnPos, Quaternion.identity) as GameObject;
		counter++;

		go.GetComponent<ZombieId> ().zombieID = "Zombie " + counter;
		NetworkServer.Spawn (go);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
