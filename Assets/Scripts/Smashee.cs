using UnityEngine;
using System.Collections;

public class Smashee : MonoBehaviour {

	PointsManager pointsManager;

	public TextMesh number;
	public Color smashColor; // number color when it should be smashed
	public Color pointsLostColor; // floating text
	public Color pointsGainedColor; // floating text color
	public GameObject floatingTextPrefab;

	public int numToSmash = 1;
	public int numRandomRange = 5; // how off random number can be

	int currentNum;
	int startNum;
	int restarted = 0;
	Color startColor;
	float timer = 0f;

	void Start() {

		pointsManager = GameObject.Find("PointsManager").GetComponent<PointsManager>();

		// set starting number
		currentNum = Random.Range(numToSmash + 1, numToSmash + numRandomRange); //current number = starting number
		startNum = currentNum;
		number.text = currentNum.ToString();
		startColor = number.color;
	}
	
	void Update () {

		timer += (Time.deltaTime + Time.deltaTime*(0.5f*restarted)); // increases countdown interval evertime currentNum reaches 0

		if (timer >= 1) {
			//LosePointsOnFailure();
			KeepNumberPointedUp();
			UpdateNumber();
			timer = 0;
		}

	}

	void OnMouseDown() { // checks if mouse pressed on collider
		CheckHit();
	}

	void KeepNumberPointedUp() {
		number.transform.rotation = Quaternion.identity;
	}

	void UpdateNumber() {
		if (currentNum < numToSmash)
		{
			restarted++;
			number.color = startColor;
			currentNum = startNum + 3; // if number gets to zero, countdown starts with a higher value
			startNum = currentNum;
		} 
		else currentNum--;

		if (currentNum == numToSmash) number.color = smashColor; // if number is numToSmash, color it red
		else number.color = startColor;

		number.text = currentNum.ToString(); //update textmesh to new number

	}

	void LosePointsOnFailure() {
		if (currentNum == numToSmash) HitFailure(smasheeExpired: true); // failed to press in time
	}

	void CheckHit() {
		if (currentNum == numToSmash) HitSuccess();
		else HitFailure(smasheeExpired: false);
	}

	void HitSuccess() {
		pointsManager.AddPoints(numToSmash);
		PointsFloatingText("+" + numToSmash, pointsGainedColor);

		Destroy(this.gameObject);
	}

	void HitFailure(bool smasheeExpired) {
		int pointsLost = smasheeExpired ? numToSmash : Mathf.Abs(numToSmash - currentNum);

		pointsManager.LosePoints(pointsLost);
		PointsFloatingText("-" + pointsLost, pointsLostColor);

		Destroy(this.gameObject);
	}

	void PointsFloatingText(string text, Color color) {
		GameObject floatingText = (GameObject)Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
		floatingText.GetComponent<FloatingText>().textMesh.text = text;
		floatingText.GetComponent<FloatingText>().textMesh.color = color;
	}
}
