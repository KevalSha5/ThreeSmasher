using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PointsManager : MonoBehaviour {

	public static PointsManager PM;
	
	public Text UIText;
	float UIExpandRate = .8f;
	float UIContractRate = .1f;
	float UIScalingRate = 0;
	Vector3 UIInitialScale;

	int points = 0;

	private Dictionary<int, int> pointsDictionary = new Dictionary<int, int>()
	{
		{3, 3},
		{4, 6},
		{5, 15}
	};

	void Awake () {
	
		if (PM == null)	PM = this;
		else if (PM != this) Destroy(this);

		DontDestroyOnLoad(this);

		UIInitialScale = UIText.transform.localScale;
	}

	public int GetPointsForSmashing (int numSmashed) {
	
		if (!pointsDictionary.ContainsKey(numSmashed)) {
			Debug.LogError("Points Dictionary did not contain given key");
			return -1;
		}

		int gain = pointsDictionary[numSmashed];
		points += gain;
		UpdateUI();

		return gain;
	}

	void UpdateUI() {		
		UIText.text = points.ToString();
		UIScalingRate = UIExpandRate;
	}

	void Update () {

		UIScalingRate -= UIContractRate;
		Vector3 newScale = UIText.transform.localScale + Vector3.one * UIScalingRate * Time.deltaTime;		
		if (newScale.magnitude < UIInitialScale.magnitude || newScale.x < 0) newScale = UIInitialScale;
		UIText.transform.localScale = newScale;

	}

}
