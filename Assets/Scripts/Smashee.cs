using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Smashee : MonoBehaviour {

	PointsManager pointsManager;

	public ShapeEffect[] shapes;
	int currentShapeCounter = 0;
	int nextShapeCounter = 0;

	public float shapeChangeSpeed = 1f;
	float timer = 0f;

	Rigidbody2D rb;
	public bool isSettled = false;

	List<Smashee> smasheeColumn; //the list of smashees for column the smashee is in

	void Start() {

		pointsManager = GameObject.Find("PointsManager").GetComponent<PointsManager>();

		rb = GetComponent<Rigidbody2D>();

		shapes = GetComponentsInChildren<ShapeEffect>();
		foreach (ShapeEffect effect in shapes)
			effect.gameObject.SetActive(false);

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

		if (rb.IsSleeping ()) {
			isSettled = true;
		}

	}

	void SetRandomShapeCounter() {
		nextShapeCounter = Random.Range(0, shapes.Length);
	}

	void CycleShapeCounter() {
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

	public void SetColumnList(List<Smashee> list) {
		this.smasheeColumn = list;
	}

	public void RemoveFromColumnsList() {
		smasheeColumn.Remove(this);
	}

}
