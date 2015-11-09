using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ZombieAttack : NetworkBehaviour {

	private float attackRate = 3f;
	private float nextAttack;
	private int damage = 10;
	private float minDistance = 2f;
	private float currentDistance;
	private Transform myTransform;
	private ZombieTarget targetScript;

	[SerializeField]
	private Material zombieGreen;
	[SerializeField]
	private Material zombieRed;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		targetScript = GetComponent<ZombieTarget> ();


		// start thread
		if (isServer) {
			StartCoroutine (Attack ());
		}
	}

	IEnumerator Attack() {
		for (;;) {

			yield return new WaitForSeconds(0.2f);
			CheckIfTargetInRange();
		}
	}

	void CheckIfTargetInRange() {
		if (targetScript.targetTransform != null) {
			currentDistance = Vector3.Distance(targetScript.targetTransform.position,myTransform.position);
			if(currentDistance < minDistance && Time.time > nextAttack) {
				nextAttack = Time.time + attackRate;

				targetScript.targetTransform.GetComponent<PlayerHealth>().DeductHealth(damage);

				StartCoroutine(ChangeZombieMat());
				RpcChangeZombieAppearance();
			}
		}
	}

	IEnumerator ChangeZombieMat() {
		GetComponent<Renderer> ().material = zombieRed;
		yield return new WaitForSeconds (attackRate / 2f);
		GetComponent<Renderer> ().material = zombieGreen;
	}

	[Client]
	void RpcChangeZombieAppearance() {
		StartCoroutine (ChangeZombieMat ());
	}


}
