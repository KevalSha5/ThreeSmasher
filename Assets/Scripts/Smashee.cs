using UnityEngine;

public class Smashee : MonoBehaviour {

	public static float width = -1f;
	public static float height = -1f;

	public SpriteRenderer backgroundRenderer;
	public Color staticSquareColor;
	public Color nonstaticSquareColor;


	public Shape[] shapes;
	public int currentShapeCounter = 0;
	int nextShapeCounter = 0;

	SmasheeFall sf;

	SmasheeGenerator SG = SmasheeGenerator.SG;
	PatternManager PM = PatternManager.PM;

	public int column;
	public int row;

	public bool isStaticShape;

	public static int order = 0;
	public int orderAdded;

	void Awake() {

		if (width == -1) {
			width = backgroundRenderer.bounds.extents.x * 2f;
			height = backgroundRenderer.bounds.extents.y * 2f;
		}

		sf = GetComponent<SmasheeFall>();

		shapes = GetComponentsInChildren<Shape>();
		foreach (Shape shape in shapes)
			shape.Hide();
		
		SetRandomShape();
	}

	void Start () {
		if (isStaticShape) backgroundRenderer.material.color = staticSquareColor;
		else backgroundRenderer.material.color = nonstaticSquareColor;
	}
	
	void Update () {

		if (sf.currentState != sf.lastState) { //only if state changed		

			if (sf.currentState == SmasheeFall.State.Settled) Settle();
			else Unsettle();

		}	

	}

	void Settle () {
		orderAdded = order++;
		CalculateRow();
		SG.AddToGrid(this);
		PM.RequestPatternCheck(this);
	}

	void Unsettle () {
		SG.RemoveFromGrid(this);
		ClearRow();
	}

	void ClearRow() {
		this.row = -1;
	}

	void SetOpacity(float opacity) {
		Color color = backgroundRenderer.color;
		color.a = opacity;
		backgroundRenderer.color = color;
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
		shapes[currentShapeCounter].Hide();
		shapes[nextShapeCounter].Show();
		currentShapeCounter = nextShapeCounter;
	}


	void CalculateRow() {
		Vector2 start = transform.position;

		Vector2 end = transform.position;
		end.y = -Camera.main.orthographicSize; // bottom of screen

		RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, LayerMask.GetMask("Smashee"));

		row = hits.Length - 1; //the raycast will also hit its own collider, so decrement by 1
	}

	public void FillShape () {
		shapes[currentShapeCounter].RequestFill();
	}

	public void UnfillShape () {
		shapes[currentShapeCounter].RequestUnfill();
	}

	public void RequestShape (int newShape) {
		if (newShape < 0 || newShape >= shapes.Length) return;
		SetShape(newShape);
	}

	public void Toggle () {
		if (isStaticShape) return;
		UnfillShape();
		CycleShape();
	}

	public bool HasSameShape (Smashee smashee) {
		return this.currentShapeCounter == smashee.currentShapeCounter;
	}

	public override string ToString() {
		return string.Format("[{0:D}, {1:D}]", column, row);
	}

	void OnDestroy() {
		SG.RemoveFromGrid(this);
	}

}
