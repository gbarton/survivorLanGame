using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CoinId : NetworkBehaviour {

	[SyncVar]
	public string coinID;
	
	private Transform myTransform;
	
	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		SyncIdentity ();
	}
	
	void SyncIdentity() {
		if (myTransform.name == "" || myTransform.name == "Coin(Clone)") {
			myTransform.name = coinID;
		}
	}
}
