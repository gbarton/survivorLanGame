using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ConsoleShow : NetworkBehaviour {

	public Canvas canvas;
	private CanvasGroup canvasGroup;
	// are we activated
	[SyncVar]
	private bool enabled = false;
	// who activated us
	[SyncVar]
	private string playerId;
	public bool isColliding = false;

	private float fadeSpeed = 0.4f;


	// Use this for initialization
	void Start () {
		canvasGroup = canvas.GetComponentInChildren<CanvasGroup> ();
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		DisplayCanvas ();
	}


	void DisplayCanvas() {
		if (enabled) {
			if (!canvasGroup.interactable) {
				canvasGroup.interactable = true;
			}
			if (canvasGroup.alpha < 0.9f) {
				canvasGroup.alpha = Mathf.Lerp (canvasGroup.alpha, 1.0f, Time.deltaTime * fadeSpeed);
			} else {
				canvasGroup.alpha = 1.0f;
			}
		} else {
			if (canvasGroup.interactable) {
				canvasGroup.interactable = false;
			}
			if (canvasGroup.alpha > 0.1f) {
				canvasGroup.alpha = Mathf.Lerp (canvasGroup.alpha, 0.0f, Time.deltaTime * fadeSpeed);
			} else {
				canvasGroup.alpha = 0.0f;
			}
		}
	}

	// when we hit a trigger
	private void OnTriggerEnter (Collider other) {
		
		if ("Player" == other.tag) {
			if (isColliding)
				return;
			isColliding = true;
			playerId = other.transform.name;
			enabled = true;
			
			Debug.Log ("HIT BY: " + other.transform.name + " " + isServer); 

		}
	}

	// when we hit a trigger
	private void OnTriggerExit (Collider other) {
		
		if ("Player" == other.tag && playerId == other.transform.name) {
			isColliding = false;
			enabled = false;
			
			Debug.Log ("LEFT BY: " + other.transform.name + " " + isServer); 
			
		}
	}

}
