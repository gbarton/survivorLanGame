using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ZombieTarget : NetworkBehaviour {

	private NavMeshAgent agent;
	private Transform myTransform;
	public Transform targetTransform;

	private LayerMask raycastLayer;
	private float radius = 30f;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		myTransform = transform;
		raycastLayer = 1 << LayerMask.NameToLayer("Player");

		// start the thread
		if (isServer) {
			StartCoroutine (DoCheck ());
		}
	}
	
	void FixedUpdate () {

	}

	void SearchForTarget() {
		// only runs on server
		if (!isServer) {
			return;
		}

		// find target
		if (targetTransform == null) {
			// cast a sphere from my position, with my radius, with my filter set to players only
			Collider[] hitColliders = Physics.OverlapSphere(myTransform.position,
			                                                radius,raycastLayer);

			// got one/some
			if(hitColliders.Length > 0) {
				// pick one at random
				int randomInt = Random.Range(0,hitColliders.Length);
				targetTransform = hitColliders[randomInt].transform;
			}

		}

		// if target is dead (by the collider being disabled, reset target)
		if (targetTransform != null && targetTransform.GetComponent<BoxCollider> ().enabled == false) {
			targetTransform = null;
		}
	}

	void MoveToTarget() {
		 // only move on server, and if current target.
		if (targetTransform != null && isServer) {
			SetNavDestination(targetTransform);
		}
	}

	void SetNavDestination(Transform dest) {
		agent.SetDestination (dest.position);
	}

	// threaded for loop that will do calc check every 0.2 seconds
	IEnumerator DoCheck() {
		for (;;) {
			SearchForTarget ();
			MoveToTarget ();
			yield return new WaitForSeconds(0.2f);
		}
	}


}
