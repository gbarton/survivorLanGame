using UnityEngine;
using System.Collections;

// master manager of the entire game state
public class GameManager : MonoBehaviour {

	public float startDelay = 5f;
	public float endDelay = 5f;



	// Use this for initialization
	void OnStartServer () {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		//for(GameObject p  players) {
		//	p
		//}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
