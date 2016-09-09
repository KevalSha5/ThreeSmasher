using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Smashee : MonoBehaviour {

	public ShapeEffect[] shapes;
	public int currentShapeCounter = 0;
	int nextShapeCounter = 0;

	public SpriteRenderer square;
	public Color staticSquareColor;
	public Color nonstaticSquareColor;

	public float shapeChangeSpeed = 1f;
	float timer = 0f;

	Rigidbody2D rb;
	SmasheeFall sf;

	SmasheeGenerator SG = SmasheeGenerator.SG;

	public int column;
	public int row;

	bool currentlySleeping;
	bool previouslySleeping;

	public bool isStaticShape;
	public bool debugging = false;

	void Start() {

		//rb = GetComponent<Rigidbody2D>();
		sf = GetComponent<SmasheeFall>();

		shapes = GetComponentsInChildren<ShapeEffect>();
		foreach (ShapeEffect effect in shapes)
			effect.gameObject.SetActive(false);

		if (isStaticShape) square.color = staticSquareColor;
		else square.color = nonstaticSquareColor;

		SetRandomShape();
	}
	
	void Update () {

		timer += Time.deltaTime; // increases countdown interval evertime currentNum reaches 0

		if (timer >= shapeChangeSpeed && !isStaticShape) {
			// CycleShapeCounter();
			// SetRandomShapeCounter();
			timer = 0;
		}

//		previouslySleeping = currentlySleeping;
//		currentlySleeping = rb.IsSleeping();

//		if (currentlySleeping && !previouslySleeping) {
//
//			CalculateRow();
//			SG.AddToGrid(this);
//			Rules.RulePatterns.dirty = true;
//			ChangeOpacity(.5f);
//
//		} else if (!currentlySleeping) {
//
//			SG.RemoveFromGrid(this);	
//			ChangeOpacity(1);
//
//		}

		if (sf.currentState != sf.lastState) { //only if state changed

			if (sf.currentState == SmasheeFall.State.Settled) {
				CalculateRow();
				SG.AddToGrid(this);
				ChangeOpacity(.5f);
				Rules.RulePatterns.dirty = true;
			} else {
				SG.RemoveFromGrid(this);	
				sf.velocity = 3.24f;
				ChangeOpacity(1);
			}

		}	

	}

	void ChangeOpacity(float opacity) {
		Color color = square.color;
		color.a = opacity;
		square.color = color;
	}

	void SetRandomShape() {
		nextShapeCounter = Random.Range(0, shapes.Length);
		ActivateNextShape();
	}

	void CycleShape() {
		nextShapeCounter = (currentShapeCounter + 1) % shapes.Length;
		ActivateNextShape();
	}

	void ActivateNextShape() {
		shapes[currentShapeCounter].gameObject.SetActive(false);
		shapes[nextShapeCounter].gameObject.SetActive(true);
		currentShapeCounter = nextShapeCounter;
	}

	void OnMouseDown() { // checks if mouse pressed on collider
		// shapes[currentShapeCounter].ExecuteUnityEvent();
		if (isStaticShape) return;
		CycleShape();
		Rules.RulePatterns.dirty = true;
	}

	void CalculateRow() {
		Vector2 start = transform.position;

		Vector2 end = transform.position;
		end.y = -Camera.main.orthographicSize; // bottom of screen

		RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, LayerMask.GetMask("Smashee"));

		row = hits.Length - 1; //the raycast will also hit this smashee, so decrement by 1
	}

	public bool HasSameShape(Smashee smashee) {
		return this.currentShapeCounter == smashee.currentShapeCounter;
	}

	void OnDestroy() {
		SG.RemoveFromGrid(this);
	}

}
