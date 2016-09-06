using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmasheeGenerator : MonoBehaviour {

	public static SmasheeGenerator SG;

	public float instantiationDelay;
	public GameObject smashee;
	public int numSmasheeInRow = 5;

	public List<Smashee>[] smasheeListArray; // to keep track of smashees in the column
	float[] instantiationXPoints;
	float horizontalExtent; //orthographic width of screen

	float timer = 1f; // also acts as delay for when to start generating

	void Awake() {

		if (SG != null) SG = new SmasheeGenerator();
		else SG = this;

		DontDestroyOnLoad(this);
	}

	void Start () {
		
		instantiationXPoints = new float[numSmasheeInRow];
		horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

		// set up the x-coordinates for the spawning points
		float smasheeWidth = smashee.GetComponentInChildren<SpriteRenderer>().bounds.extents.x * 2f;
		for (int i = 0; i < numSmasheeInRow; i++) {
			instantiationXPoints[i] = -horizontalExtent + i * smasheeWidth + smasheeWidth / 2f;
		}

		// instantiate the lists in the array
		smasheeListArray = new List<Smashee>[numSmasheeInRow];
		for (int i = 0; i < numSmasheeInRow; i++)
			smasheeListArray[i] = new List<Smashee>();
		GameoverManager.Manager.smasheeListArray = this.smasheeListArray;

	}	
	
	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0) {

			int column = Random.Range(0, instantiationXPoints.Length);
			float x = instantiationXPoints[column];
			float y = Camera.main.orthographicSize * .6f; // 80% height?? (orthographicSize is half of screen)

			Vector3 pos = new Vector3(x, y, 0);
			GameObject newSmashee = (GameObject)Instantiate(smashee, pos, Quaternion.identity);
			Smashee newSmasheeScript = newSmashee.GetComponent<Smashee>();

			// add a reference to the smasheeScript to the columns list
			newSmasheeScript.SetColumnList(smasheeListArray[column]);
			smasheeListArray[column].Add(newSmasheeScript);

			timer = instantiationDelay;
		}
		
	}
}
