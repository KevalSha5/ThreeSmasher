using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmasheeGenerator : MonoBehaviour {

	public static SmasheeGenerator SG;

	public float instantiationDelay;
	public float staticShapeProbability;
	public GameObject smasheePrefab;

	public int maxColumns; //x
	public int maxRows; //y 

	float[] instantiationXPoints;
	float horizontalExtent; //orthographic width of screen

	public bool generate = true;
	float timer = 1f; // also acts as delay for when to start generating

	public Smashee[,] settledSmasheeGrid;

	void Awake() {

		settledSmasheeGrid = new Smashee[maxColumns, maxRows];

		if (SG != null) SG = new SmasheeGenerator();
		else SG = this;

		DontDestroyOnLoad(this);

	}

	void Start () {
		
		instantiationXPoints = new float[maxColumns];
		horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

		// set up the x-coordinates for the spawning points
		float smasheeWidth = smasheePrefab.GetComponentInChildren<SpriteRenderer>().bounds.extents.x * 2f;
		for (int i = 0; i < maxColumns; i++) {
			instantiationXPoints[i] = -horizontalExtent + i * smasheeWidth + smasheeWidth / 2f;
		}


	}	

	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0 && generate) {

			int column = Random.Range(0, instantiationXPoints.Length);
			float x = instantiationXPoints[column];
			float y = Camera.main.orthographicSize * .6f; // 80% height?? (orthographicSize is half of screen)

			Vector3 pos = new Vector3(x, y, 0);
			GameObject newSmasheeObj = (GameObject)Instantiate(smasheePrefab, pos, Quaternion.identity);
			Smashee newSmasheeScript = newSmasheeObj.GetComponent<Smashee>();
			newSmasheeScript.column = column;

			if (Random.value <= staticShapeProbability) newSmasheeScript.isStaticShape = true;

			timer = instantiationDelay;
		}

	}

	public void DebugPrintGrid() {

		string grid = "";
		for (int x = 0; x < settledSmasheeGrid.GetLength(0); x++) {
			for (int y = 0; y < settledSmasheeGrid.GetLength(1); y++) {
				if (settledSmasheeGrid[x, y] != null) grid += "S";
				else grid += "_";
			}
			grid += "\n";
		}

		Debug.Log(grid);
	}

	public void RemoveFromGrid(Smashee smashee) {
		int column = smashee.column;
		int row = smashee.row;

		if (CheckGridBounds(column, row)) {
			lock(settledSmasheeGrid)
				settledSmasheeGrid[column, row] = null;
		}
	}

	public void AddToGrid(Smashee smashee) {
		int column = smashee.column;
		int row = smashee.row;

		if (CheckGridBounds(column, row)) {
			lock(settledSmasheeGrid)
				settledSmasheeGrid[column, row] = smashee;
		}
	}

	public bool CheckGridBounds(int x, int y) {
		if (x < 0 || x >= maxColumns || y < 0 || y >= maxRows) {
			return false;
			Debug.LogError("Something went wrong with CheckBounds, this shouldn't be happening");
		}

		return true;

	}

	public void CheckForDuplicates() {

		foreach (Smashee smashee in settledSmasheeGrid) {
			foreach (Smashee other in settledSmasheeGrid) {

				if (smashee.column == other.column && smashee.row == other.row) continue;

				if (smashee.gameObject == other.gameObject) 
					Debug.Log("Duplcate found! " + smashee.column + ", " + smashee.row);

			}
		}
			

	}

}
