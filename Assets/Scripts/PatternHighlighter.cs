using UnityEngine;

public class PatternHighlighter : MonoBehaviour {

	Pattern pattern;

	LineRenderer lr;

	Vector3[] currentPoints;

	Vector3 startPoint;
	Vector3 recedePoint;
	
	Vector3 endA;
	Vector3 endB;

	bool clearing = false;
	float patternDepth = -1f;

	void Awake () {
		lr = GetComponent<LineRenderer>();
	}

	public void NewHighlight (Pattern pattern) {

		this.pattern = pattern;

		startPoint = SmasheeGenerator.SG.SmasheeCenterPoint(pattern.GetLastAdded());
		currentPoints = new Vector3[] {startPoint, startPoint};

		endA = SmasheeGenerator.SG.SmasheeCenterPoint(pattern.first);
		endB = SmasheeGenerator.SG.SmasheeCenterPoint(pattern.last);

		endA.z = patternDepth;
		endB.z = patternDepth;
		startPoint.z = patternDepth;

		lr.SetVertexCount(currentPoints.Length);
		lr.SetPositions(currentPoints);

		foreach (Smashee smashee in pattern.GetSmashees())
			smashee.FillShape();
	}

	public void AdjustHighlight (Pattern pattern) {

		this.pattern = pattern;

		endA = SmasheeGenerator.SG.SmasheeCenterPoint(pattern.first);
		endB = SmasheeGenerator.SG.SmasheeCenterPoint(pattern.last);

		foreach (Smashee smashee in pattern.GetSmashees())
			smashee.FillShape();

	}

	public void Clear () {
		clearing = true;
		recedePoint = (endA + endB) / 2f;

		foreach (Smashee smashee in pattern.GetSmashees())
			smashee.UnfillShape();
	}

	void Update ()	 {

		float expandSpeed = Time.deltaTime * 5f;
		float recedeSpeed = Time.deltaTime * 10f;

		if (!clearing) {
			currentPoints[0] = Vector3.Lerp(currentPoints[0], endA, expandSpeed);
			currentPoints[1] = Vector3.Lerp(currentPoints[1], endB, expandSpeed);

		} else {
			currentPoints[0] = Vector3.Lerp(currentPoints[0], recedePoint, recedeSpeed);
			currentPoints[1] = Vector3.Lerp(currentPoints[1], recedePoint, recedeSpeed);

		}

		lr.SetPositions(currentPoints);

	}

}
