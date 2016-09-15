using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Smashee : MonoBehaviour {

	public static float width = -1f;
	public static float height = -1f;

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
	PatternChecker PC = PatternChecker.PC;

	public int column;
	public int row;

	bool currentlySleeping;
	bool previouslySleeping;

	public bool isStaticShape;
	public bool debugging = false;

	void Awake() {
		//rb = GetComponent<Rigidbody2D>();
		sf = GetComponent<SmasheeFall>();

		shapes = GetComponentsInChildren<ShapeEffect>();
		foreach (ShapeEffect effect in shapes)
			effect.gameObject.SetActive(false);
		
		SetRandomShape();
	}

	void Start () {
		if (isStaticShape) square.color = staticSquareColor;
		else square.color = nonstaticSquareColor;
	}
	
	void Update () {

		if (sf.currentState != sf.lastState) { //only if state changed
			
			if (sf.currentState == SmasheeFall.State.Settled) {
				CalculateRow();
				SG.AddToGrid(this);
				PC.RequestPatternCheck(this);
			} else {
				SG.RemoveFromGrid(this);
				NullifyRow();
			}

		}	

	}

	void ChangeOpacity(float opacity) {
		Color color = square.color;
		color.a = opacity;
		square.color = color;
	}

	void NullifyRow() {
		this.row = -1;
	}

	public void RequestShape(int newShape) {
		if (newShape < 0 || newShape >= shapes.Length) return;
		SetShape(newShape);
	}

	void SetRandomShape() {
		nextShapeCounter = Random.Range(0, shapes.Length);
		ActivateNextShape();
	}

	void SetShape(int shape) {
		nextShapeCounter = shape;
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

	public void TriggerPress() {
		if (isStaticShape) return;
		CycleShape();
		PC.RequestPatternCheck(this);
	}

	void OnMouseDown() { // checks if mouse pressed on collider
		// shapes[currentShapeCounter].ExecuteUnityEvent();
//		InputManager.IM.SetDown(this);
	} 

	void OnMouseUp() {
//		InputManager.IM.SetUp(this);
	}

	void CalculateRow() {
		Vector2 start = transform.position;

		Vector2 end = transform.position;
		end.y = -Camera.main.orthographicSize; // bottom of screen

		RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, LayerMask.GetMask("Smashee"));

		row = hits.Length - 1; //the raycast will also hit its own collider, so decrement by 1
	}

	public static void SetDimensions(float x, float y) {
		width = x;
		height = y;
	}

	public bool HasSameShape(Smashee smashee) {
		return this.currentShapeCounter == smashee.currentShapeCounter;
	}

	public override string ToString() {
		return string.Format("[{0:D}, {1:D}]", column, row);
	}

	void OnDestroy() {
		SG.RemoveFromGrid(this);
	}

}
