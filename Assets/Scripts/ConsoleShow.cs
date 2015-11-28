using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ConsoleShow : NetworkBehaviour {

	public Canvas canvas;
	private CanvasGroup canvasGroup;
	private Image loadingImage;

	[SyncVar]
	private float loadedAmt = 0.0f;
	private float loadSpeed = 0.01f;

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
		loadingImage = canvas.GetComponentInChildren<Image> ();
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
			// update loading part
			Debug.Log ("loading " + loadedAmt);

			//loadedAmt = Mathf.Lerp(loadedAmt,1.0f,Time.deltaTime*loadSpeed);
			loadedAmt = Mathf.MoveTowards(loadedAmt,1.0f, Time.deltaTime*loadSpeed);
			loadingImage.fillAmount = loadedAmt;

		} else {
			if (canvasGroup.interactable) {
				canvasGroup.interactable = false;
				loadingImage.fillAmount = 0.0f;
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
