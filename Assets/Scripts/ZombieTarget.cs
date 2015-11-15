using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ZombieTarget : NetworkBehaviour {

	private NavMeshAgent agent;
	private Transform myTransform;
	public Transform targetTransform;
	private bool wandering = false;
	private Vector3 wanderPoint;

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
				// stop wandering
				ToggleWandering();
			}

		}

		// check target out of range or dead
		// if target is dead (by the collider being disabled, reset target)
		if (targetTransform != null) {
			if(targetTransform.GetComponent<BoxCollider> ().enabled == false) {
				targetTransform = null;
			}
			if(Vector3.Distance(myTransform.position, targetTransform.position) > radius) {
				targetTransform = null;
			}

		}

		// if we are still null, lets wander in a random direction
		if (targetTransform == null) {
			// if we reached our wanderTarget, null it out
			if(wandering && Vector3.Distance(myTransform.position, wanderPoint) <= 1.0f) {
				ToggleWandering();
			}

			if(!wandering) {
				ToggleWandering();
			}

		}
	}

	void MoveToTarget() {
		if (!isServer) {
			return;
		}
		 // only move on server, and if current target.
		if (targetTransform != null) {
			SetNavDestination (targetTransform.position);
		} else if (wanderPoint != null) {
			SetNavDestination (wanderPoint);
		}

	}

	void SetNavDestination(Vector3 dest) {
		agent.SetDestination (dest);
	}

	void ToggleWandering() {
		// turn off
		if (wandering) {
			agent.speed = 3.5f;
			wandering = false;
		} else {
			wandering = true;
			agent.speed = 2.0f;
			wanderPoint = new Vector3(Random.Range (-100,100),0,Random.Range (-100,100));
		}
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
