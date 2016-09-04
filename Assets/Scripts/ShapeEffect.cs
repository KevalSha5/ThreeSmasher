using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ShapeEffect : MonoBehaviour {

	public UnityEvent unityEvent;
	public PointsManager pointsManager;
	public GameObject floatingTextPrefab;

	public Color pointsLostColor; // floating text
	public Color pointsGainedColor; // floating text color

	void Start () {
		pointsManager = GameObject.Find("PointsManager").GetComponent<PointsManager>();
	}

	public void ExecuteUnityEvent() {
		unityEvent.Invoke();
	}

	public void LosePoints(int points) {
		pointsManager.LosePoints(points);
		PointsFloatingText("-" + points, pointsLostColor);
		DestorySmashee();
	}

	public void GainPoints(int points) {
		pointsManager.GainPoints(points);
		PointsFloatingText("+" + points, pointsGainedColor);
		DestorySmashee();
	}

	public void DestorySmashee() {
		Destroy(transform.parent.gameObject);
		GetComponentInParent<Smashee>().RemoveFromColumnsList();
	}

	void PointsFloatingText(string text, Color color) {
		GameObject floatingText = (GameObject)Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
		floatingText.GetComponent<FloatingText>().textMesh.text = text;
		floatingText.GetComponent<FloatingText>().textMesh.color = color;
	}

}
