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

		startPoint = SmasheeGenerator.SG.GetWorldPoint(pattern.GetLastAdded());
		startPoint.z = patternDepth;

		currentPoints = new Vector3[] {startPoint, startPoint};

		SetEndpoints(SmasheeGenerator.SG.GetWorldPoint(pattern.first),
					 SmasheeGenerator.SG.GetWorldPoint(pattern.last));


		lr.SetVertexCount(currentPoints.Length);
		lr.SetPositions(currentPoints);

		foreach (Smashee smashee in pattern.GetSmashees())
			smashee.FillShape();
	}

	public void AdjustHighlight (Pattern pattern) {

		this.pattern = pattern;
 		
		SetEndpoints(SmasheeGenerator.SG.GetWorldPoint(pattern.first),
					 SmasheeGenerator.SG.GetWorldPoint(pattern.last));

		foreach (Smashee smashee in pattern.GetSmashees())
			smashee.FillShape();

	}

	public void Clear () {
		clearing = true;
		recedePoint = (endA + endB) / 2f;

		foreach (Smashee smashee in pattern.GetSmashees())
			smashee.UnfillShape();

		Destroy(this.gameObject);
	}

	void SetEndpoints (Vector3 endA, Vector3 endB) {
		this.endA = endA;
		this.endB = endB;

		this.endA.z = patternDepth;
		this.endB.z = patternDepth;
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
