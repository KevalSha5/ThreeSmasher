using UnityEngine;
using System.Collections;

public class SmasheeGenerator : MonoBehaviour {

	public float instantiationSpeed;
	public GameObject smashee;

	float timer = 1f; // also acts as delay for when to start generating

	void Start () {
	}	
	
	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0) {

			//TODO: Add grid-like snapping. The image is 256x256

			float x = Random.value * Camera.main.pixelWidth;
			float y = Random.value * Camera.main.pixelHeight;

			Vector3 pos = new Vector3(x, y, 0);
			pos = Camera.main.ScreenToWorldPoint(pos);
			pos.z = 0f;

			Instantiate(smashee, pos, Quaternion.identity);

			timer = instantiationSpeed;
		}
		
	}
}
