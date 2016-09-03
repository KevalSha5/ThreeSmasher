using UnityEngine;
using System.Collections;

public class SmasheeGenerator : MonoBehaviour {

	public float instantiationSpeed;
	public GameObject smashee;
	public int numSmasheeInRow = 5;
	public int numSmasheeInColumn;

	float[] instantiationXPoints;
	float horizontalExtent; //orthographic width of screen

	float timer = 1f; // also acts as delay for when to start generating

	void Start () {
		
		instantiationXPoints = new float[numSmasheeInRow];
		horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

		float smasheeWidth = smashee.GetComponentInChildren<SpriteRenderer>().bounds.extents.x * 2f;
		for (int i = 0; i < numSmasheeInRow; i++) {
			instantiationXPoints [i] = -horizontalExtent + i * smasheeWidth + smasheeWidth / 2f;
//			Debug.Log(instantiationXPoints[i]);
		}
		
	}	
	
	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0) {

			float x = instantiationXPoints[Random.Range (0, instantiationXPoints.Length)];
			float y = Camera.main.pixelHeight * .8f; // 80% height

			Vector3 pos = new Vector3(x, y, 0);
			pos = Camera.main.ScreenToWorldPoint(pos); // TODO: fix this
			pos.z = 0f;
			pos.x = x;

			Instantiate(smashee, pos, Quaternion.identity);

			timer = instantiationSpeed;
		}
		
	}
}
