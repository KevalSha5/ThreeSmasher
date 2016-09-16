using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointsManager : MonoBehaviour {

	//Singleton
	public static PointsManager PM;
	public Text pointsText;
	int points = 0;

	void Awake () {
	
		if (PM == null)	PM = this;
		else if (PM != this) Destroy(this);

		DontDestroyOnLoad(this);
	}

	public void Gain(int points) {
		this.points += points;
		UpdateUI();
	}

	public void Lose(int points) {
		this.points -= points;
		UpdateUI();
	}

	void UpdateUI() {		
		pointsText.text = points.ToString();
	}
	
	void Update () {
	
	}
}
