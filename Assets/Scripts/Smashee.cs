using UnityEngine;
using System.Collections;

public class Smashee : MonoBehaviour {

	Points pointsManager;

	public TextMesh number;
	public Color smashColor;
	public GameObject floatingTextPrefab;

	public int numToSmash = 5;
	public int numRandomRange = 4; // one sided range on how off random number can be
	public int numSafeRange = 1; // one sided range on 'safe zone' So if numToSmash = 5, then the number can't start from 4 to 6

	int max;
	int min;
	int currentNum;

	float timer = 0f;

	void Start() {

		pointsManager = GameObject.Find("PointsManager").GetComponent<Points>();

		int max = numToSmash + numRandomRange;
		int min = numToSmash - numRandomRange;

		// set starting number
		do currentNum = Random.Range(min, max); //current number = starting number
		while (currentNum >= numToSmash - numSafeRange && currentNum <= numToSmash + numSafeRange);

		number.text = currentNum.ToString();
	}
	
	void Update () {

		timer += Time.deltaTime;

		if (timer >= 1) {
			LosePointsOnFailure();
			UpdateNumber();
			timer = 0;
		}

	}

	void UpdateNumber() {

		if (currentNum < numToSmash) currentNum++;
		else currentNum--;

		if (currentNum == numToSmash) number.color = smashColor; // if number is numToSmash, color it red

		number.text = currentNum.ToString(); //update textmesh to new number

	}

	void LosePointsOnFailure() {

		if (currentNum == numToSmash) LosePoints(); // failed to press in time

	}

	void OnMouseDown() {
		CheckHit();
	}

	void CheckHit() {

		string pointsDiff = "";

		if (currentNum == numToSmash) {
			GetPoints ();
			pointsDiff = "+" + numToSmash;
		} else {
			LosePoints();
			pointsDiff = "-" + Mathf.Abs (numToSmash - currentNum);
		}
			
		GameObject floatingText = (GameObject)Instantiate(floatingTextPrefab, transform.position, transform.localRotation);
		floatingText.GetComponent<FloatingText>().textMesh.text = pointsDiff;

		Destroy(this.gameObject);
	}

	void GetPoints() {
		pointsManager.AddPoints(5);
	}

	void LosePoints() {
		pointsManager.LosePoints(Mathf.Abs(numToSmash - currentNum));
	}
}
