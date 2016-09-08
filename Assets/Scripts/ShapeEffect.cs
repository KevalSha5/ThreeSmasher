using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ShapeEffect : MonoBehaviour {

	public UnityEvent unityEvent;
	public GameObject floatingTextPrefab;

	public Color pointsLostColor; // floating text
	public Color pointsGainedColor; // floating text color

	public void ExecuteUnityEvent() {
		unityEvent.Invoke();
	}

	public void LosePoints(int points) {
		PointsManager.Points.Lose(points);
		PointsFloatingText("-" + points, pointsLostColor);
		DestorySmashee();
	}

	public void GainPoints(int points) {
		PointsManager.Points.Gain(points);
		PointsFloatingText("+" + points, pointsGainedColor);
		DestorySmashee();
	}

	public void DestorySmashee() {
		Destroy(transform.parent.gameObject);
	}

	void PointsFloatingText(string text, Color color) {
		GameObject floatingText = (GameObject)Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
		floatingText.GetComponent<FloatingText>().textMesh.text = text;
		floatingText.GetComponent<FloatingText>().textMesh.color = color;
	}

}
