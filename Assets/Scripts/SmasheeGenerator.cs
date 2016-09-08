using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmasheeGenerator : MonoBehaviour {

	public static SmasheeGenerator SG;

	public float instantiationDelay;
	public float staticSmasheeProbability;
	public GameObject smashee;
	public int numSmasheeInRow = 5;

	public int[] numSmasheeInColumn;

	float[] instantiationXPoints;
	float horizontalExtent; //orthographic width of screen

	public bool generate = true;
	float timer = 1f; // also acts as delay for when to start generating

	System.Action<int> smasheeOnDestroyCallback;
	System.Action<int> smasheeOnSettleCallback;

	void Awake() {

		numSmasheeInColumn = new int[numSmasheeInRow];

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

		smasheeOnSettleCallback = (column) => {numSmasheeInColumn[column]++;};
		smasheeOnDestroyCallback = (column) => {numSmasheeInColumn[column]--;};

	}	

	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0 && generate) {

			int column = Random.Range(0, instantiationXPoints.Length);
			float x = instantiationXPoints[column];
			float y = Camera.main.orthographicSize * .6f; // 80% height?? (orthographicSize is half of screen)

			Vector3 pos = new Vector3(x, y, 0);
			GameObject newSmasheeObj = (GameObject)Instantiate(smashee, pos, Quaternion.identity);
			Smashee newSmasheeScript = newSmasheeObj.GetComponent<Smashee>();

			newSmasheeScript.SetColumn(column);
			newSmasheeScript.SetOnSettleCallback(smasheeOnSettleCallback);
			newSmasheeScript.SetOnDestoryCallback(smasheeOnDestroyCallback);

			if (Random.value <= staticSmasheeProbability) newSmasheeScript.isStatic = true;

			timer = instantiationDelay;
		}
		
	}

}
