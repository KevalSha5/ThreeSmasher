using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointsManager : MonoBehaviour {

	public Text pointsText;
	int points = 0;

	void Start () {
	
	}

	public void GainPoints(int points) {
		this.points += points;
		UpdateUI();
	}

	public void LosePoints(int points) {
		this.points -= points;
		UpdateUI();
	}

	void UpdateUI() {
		pointsText.text = points.ToString();
	}
	
	void Update () {
	
	}
}
