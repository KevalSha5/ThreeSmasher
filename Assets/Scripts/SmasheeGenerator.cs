using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmasheeGenerator : MonoBehaviour {

	public float instantiationDelay;
	float instantiationSpeedAccrued;
	public GameObject smashee;
	public int numSmasheeInRow = 5;
	public int numSmasheeInColumn;

	List<Smashee>[] smasheeColumnsList; // to keep track of smashees in the column
	float[] instantiationXPoints;
	float horizontalExtent; //orthographic width of screen

	float timer = 1f; // also acts as delay for when to start generating

	void Start () {
		
		instantiationXPoints = new float[numSmasheeInRow];
		horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

		float smasheeWidth = smashee.GetComponentInChildren<SpriteRenderer>().bounds.extents.x * 2f;
		for (int i = 0; i < numSmasheeInRow; i++) {
			instantiationXPoints[i] = -horizontalExtent + i * smasheeWidth + smasheeWidth / 2f;
		}

		smasheeColumnsList = new List<Smashee>[numSmasheeInRow];
		for (int i = 0; i < numSmasheeInRow; i++)
			smasheeColumnsList[i] = new List<Smashee>();

		GameObject.Find("GameoverManager").GetComponent<GameoverManager>().SetSmasheeColumnsList(smasheeColumnsList);
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
			newSmasheeScript.SetColumnList(smasheeColumnsList[column]);
			smasheeColumnsList[column].Add(newSmasheeScript);

			timer = instantiationDelay;
		}
		
	}
}
