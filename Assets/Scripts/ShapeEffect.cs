using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ShapeEffect : MonoBehaviour {

	public UnityEvent unityEvent;
	public PointsManager pointsManager;
	public GameObject floatingTextPrefab;

	void Start () {
		pointsManager = GameObject.Find("PointsManager").GetComponent<PointsManager>();
	}
	
	void Update () {
	
	}

	public void ExecuteUnityEvent() {
		unityEvent.Invoke();
	}

	public void LosePoints(int points) {
		pointsManager.LosePoints(points);
		PointsFloatingText("-" + points, Color.red);
		DestorySmashee();
	}

	public void GainPoints(int points) {
		pointsManager.GainPoints(points);
		PointsFloatingText("+" + points, Color.blue);
		DestorySmashee();
	}

	public void BombEffect() {

	}

	void DestorySmashee() {
		Destroy(transform.parent.gameObject);
	}

	void PointsFloatingText(string text, Color color) {
		GameObject floatingText = (GameObject)Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
		floatingText.GetComponent<FloatingText>().textMesh.text = text;
		floatingText.GetComponent<FloatingText>().textMesh.color = color;
	}

}
