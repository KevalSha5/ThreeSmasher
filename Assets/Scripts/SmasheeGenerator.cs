using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmasheeGenerator : MonoBehaviour {

	public static SmasheeGenerator SG;

	public float instantiationDelay;
	public float staticShapeProbability;
	public GameObject smasheePrefab;

	public int maxColumns; //x
	public int maxRows;	//y

	float[] instantiationXPoints;
	float horizontalExtent;
	//orthographic width of screen

	public bool generate = true;
	float timer = 1f;	// also acts as delay for when to start generating

	public Smashee[,] settledSmasheeGrid;

	void Awake () {

		if (SG == null) SG = this;
		else if (SG != this) Destroy(this);

		DontDestroyOnLoad(this);		

		settledSmasheeGrid = new Smashee[maxColumns, maxRows];

		instantiationXPoints = new float[maxColumns];
		horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

		// temporarily calculating smashee width here because Smashee is not yet initialized
		float smasheeWidth = smasheePrefab.GetComponentInChildren<SpriteRenderer>().bounds.extents.x * 2f;

		for (int i = 0; i < maxColumns; i++) {
			instantiationXPoints[i] = -horizontalExtent + i * smasheeWidth + smasheeWidth / 2f;
		}

	}

	void Start () {

	}

	void Update () {

		timer -= Time.deltaTime;

		if (timer <= 0 && generate) {
			
			int column = Random.Range(0, maxColumns);
			NewSmashee(column);

			timer = instantiationDelay;

		}

	}

	public void NewSmashee (int column, int shape = -1) {
		float x = instantiationXPoints[column];
		float y = Camera.main.orthographicSize * .6f; // 80% height?? (orthographicSize is half of screen)

		Vector3 pos = new Vector3(x, y, 0);
		GameObject smasheeGObj = Instantiate(smasheePrefab, pos, Quaternion.identity) as GameObject;

		Smashee smashee = smasheeGObj.GetComponent<Smashee>();
		smashee.column = column;
		smashee.RequestShape(shape);

		if (Random.value <= staticShapeProbability) 
			smashee.isStaticShape = true;
	}

	public List<Smashee> GetSmasheesOnGrid () {

		List<Smashee> list = new List<Smashee>();
		for (int x = 0; x < settledSmasheeGrid.GetLength(0); x++) { //For all smashes
			for (int y = 0; y < settledSmasheeGrid.GetLength(1); y++) {

				if (settledSmasheeGrid[x, y] != null)
					list.Add(settledSmasheeGrid[x, y]);

			}
		}

		return list;

	}

	public Smashee GetSmashee (int column, int row) {
		if (CheckGridBounds(column, row)) return settledSmasheeGrid[column, row];
		else return null;
	}

	public Smashee GetSmasheeFromPixelCoord (Vector2 point) {
		Collider2D collider = Physics2D.OverlapPoint(point, LayerMask.GetMask("Smashee"));

		if (collider != null) return collider.gameObject.GetComponent<Smashee>();
		else return null;
	}

	public Vector3 GetWorldPoint (Smashee smashee) {
		return smashee.gameObject.transform.position;
	}

	public void RemoveFromGrid (Smashee smashee) {
		int column = smashee.column;
		int row = smashee.row;

		if (CheckGridBounds(column, row)) {
			settledSmasheeGrid[column, row] = null;
		}
	}

	public void AddToGrid (Smashee smashee) {
		int column = smashee.column;
		int row = smashee.row;

		if (CheckGridBounds(column, row)) {
			settledSmasheeGrid[column, row] = smashee;
		}
	}

	public bool CheckGridBounds (int x, int y) {
		if (x < 0 || x >= maxColumns || y < 0 || y >= maxRows) {
			return false;
		}

		return true;
	}

	public void CheckForDuplicates () {

		foreach (Smashee smashee in settledSmasheeGrid) {
			foreach (Smashee other in settledSmasheeGrid) {

				if (smashee.column == other.column && smashee.row == other.row) continue;

				if (smashee.gameObject == other.gameObject)
					Debug.Log("Duplcate found! " + smashee.column + ", " + smashee.row);

			}
		}
			

	}

	public void DebugPrintGrid () {

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

}
