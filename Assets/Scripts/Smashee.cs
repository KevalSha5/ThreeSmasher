using UnityEngine;
using System.Collections;

public class Smashee : MonoBehaviour {

	PointsManager pointsManager;

	Color startColor;
	public Color smashColor; // number color when it should be smashed
	public Color pointsLostColor; // floating text
	public Color pointsGainedColor; // floating text color

	public ShapeEffect[] shapes;
	int currentShapeCounter = 0;
	int nextShapeCounter = 0;

	public float shapeChangeSpeed = 1f;
	float timer = 0f;

	void Start() {

		pointsManager = GameObject.Find("PointsManager").GetComponent<PointsManager>();

		shapes = GetComponentsInChildren<ShapeEffect>();
		foreach (ShapeEffect effect in shapes)
			effect.gameObject.SetActive (false);
		
		SetRandomShapeCounter();
		ActivateNextShape();

	}
	
	void Update () {

		timer += Time.deltaTime; // increases countdown interval evertime currentNum reaches 0

		if (timer >= shapeChangeSpeed) {
			//UpdateShapeCounter();
			SetRandomShapeCounter();
			ActivateNextShape();
			timer = 0;
		}

	}

	void SetRandomShapeCounter() {
		nextShapeCounter = Random.Range(0, shapes.Length);
	}

	void UpdateShapeCounter() {
		nextShapeCounter = (currentShapeCounter + 1) % shapes.Length;
	}

	void ActivateNextShape() {
		shapes[currentShapeCounter].gameObject.SetActive(false);
		shapes[nextShapeCounter].gameObject.SetActive(true);
		currentShapeCounter = nextShapeCounter;
	}

	void OnMouseDown() { // checks if mouse pressed on collider
		shapes[currentShapeCounter].ExecuteUnityEvent();
	}

}
