using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Smashee : MonoBehaviour {

	public ShapeEffect[] shapes;
	int currentShapeCounter = 0;
	int nextShapeCounter = 0;

	public SpriteRenderer square;
	public Color staticSquareColor;
	public Color nonstaticSquareColor;

	public float shapeChangeSpeed = 1f;
	float timer = 0f;

	Rigidbody2D rb;
	public bool isSettled = false;

	System.Action<int> onDestoryCallback; //callback used to decrement an array that keeps track of number of smashees
	System.Action<int> onSettleCallback; //callback used to decrement an array that keeps track of number of smashees
	int column;

	public bool isStatic;

	List<Smashee> smasheeColumn; //the list of smashees for column the smashee is in

	void Start() {

		rb = GetComponent<Rigidbody2D>();

		shapes = GetComponentsInChildren<ShapeEffect>();
		foreach (ShapeEffect effect in shapes)
			effect.gameObject.SetActive(false);

		if (isStatic) square.color = staticSquareColor;
		else square.color = nonstaticSquareColor;

		SetRandomShapeCounter();
		ActivateNextShape();

	}
	
	void Update () {

		timer += Time.deltaTime; // increases countdown interval evertime currentNum reaches 0

		if (timer >= shapeChangeSpeed && !isStatic) {
			//CycleShapeCounter();
			SetRandomShapeCounter();
			ActivateNextShape();
			timer = 0;
		}

		if (rb.IsSleeping() && !isSettled) {
			isSettled = true;
			onSettleCallback(column);
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

	public void SetOnDestoryCallback(System.Action<int> callback) {
		onDestoryCallback = callback;
	}

	public void SetOnSettleCallback(System.Action<int> callback) {
		onSettleCallback = callback;
	}

	public void SetColumn(int column) {
		this.column = column;
	}

	void OnDestroy() {
		onDestoryCallback(column);
	}

}
