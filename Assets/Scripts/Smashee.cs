﻿using UnityEngine;
using System.Collections;

public class Smashee : MonoBehaviour {

	PointsManager pointsManager;

	public TextMesh number;
	public Color smashColor;
	public Color pointsLostColor;
	public Color pointsGainedColor;
	public GameObject floatingTextPrefab;

	public int numToSmash = 1;
	public int numRandomRange = 5; // how off random number can be

	int currentNum;

	float timer = 0f;

	void Start() {

		pointsManager = GameObject.Find("PointsManager").GetComponent<PointsManager>();

		// set starting number
		currentNum = Random.Range(numToSmash + 1, numToSmash + numRandomRange); //current number = starting number

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

	void OnMouseDown() { // checks if mouse pressed on collider
		CheckHit();
	}

	void UpdateNumber() {

		if (currentNum < numToSmash) currentNum++;
		else currentNum--;

		if (currentNum == numToSmash) number.color = smashColor; // if number is numToSmash, color it red

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
		GameObject floatingText = (GameObject)Instantiate(floatingTextPrefab, transform.position, transform.localRotation);
		floatingText.GetComponent<FloatingText>().textMesh.text = text;
		floatingText.GetComponent<FloatingText>().textMesh.color = color;
	}
}
