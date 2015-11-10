using UnityEngine;
using System.Collections;

public class CoinRotate : MonoBehaviour {

	private Transform myTransform;
	private float rotationSpeed = 10f;

	// Use this for initialization
	void Start () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		myTransform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
	}


}
