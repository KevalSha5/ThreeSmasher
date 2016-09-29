using UnityEngine;

public class Shape : MonoBehaviour {

	public string Name;
	public Sprite shapeOutline;
	public Sprite shapeSolid;
	public Vector3 shapeScale;
	Color shapeColor;

	public static GameObject shapeFillerPrefab;
	public Sprite fillerShape;
	ShapeFiller shapeFiller;

	void Awake () {
		if (shapeFillerPrefab == null)
			shapeFillerPrefab = Resources.Load("ShapeFiller") as GameObject;

		shapeColor = GetComponent<SpriteRenderer>().color;

		InitShapeFiller();
	}


	public void Hide () {
		gameObject.SetActive(false);
	}

	public void Show () {
		gameObject.SetActive(true);
	}

	public void RequestFill () {
		if (shapeFiller) shapeFiller.Fill();
	}

	public void RequestUnfill () {
		if (shapeFiller) shapeFiller.Unfill();
	}

	void InitShapeFiller () {

		Vector3 pos = transform.position;
		pos.z = -.7f;

		GameObject sFillerObj = Instantiate(shapeFillerPrefab, pos, transform.rotation) as GameObject;

		shapeFiller = sFillerObj.GetComponent<ShapeFiller>();

		Vector3 scale = transform.parent.localScale;
		sFillerObj.transform.parent = transform;

		shapeFiller.Setup(Vector3.zero, fillerShape, Vector3.zero, shapeSolid, scale, shapeColor);
	}

}
