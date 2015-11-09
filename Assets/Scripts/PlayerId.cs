﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerId : NetworkBehaviour {

	[SyncVar]
	public string playerUniqueIdentity;

	private NetworkInstanceId playerNetID;
	private Transform myTransform;

	public override void OnStartLocalPlayer() {
		GetNetIdentity ();
		SetIdentity ();
	}

	// Use this for initialization
	void Awake () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (myTransform.name == "" || myTransform.name == "Player(Clone)") {
			SetIdentity();
		}
	}

	[Client]
	void GetNetIdentity() {
		playerNetID = GetComponent<NetworkIdentity> ().netId;
		CmdTellServerMyIdentity (MakeUniqueIdentity());
	}

	void SetIdentity() {
		if (!isLocalPlayer) {
			myTransform.name = playerUniqueIdentity;
		} else {
			myTransform.name = MakeUniqueIdentity();
		}

	}

	string MakeUniqueIdentity() {
		return "Player " + playerNetID.ToString ();
	}

	[Command]
	void CmdTellServerMyIdentity(string name) {
		playerUniqueIdentity = name;
	}
}
