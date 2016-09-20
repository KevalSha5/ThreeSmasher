using UnityEngine;

public class Shape : MonoBehaviour {

	public string Name;
	public SpriteRenderer render;
	public Sprite shapeOutline;
	public Sprite shapeSolid;
	public Vector3 shapeScale;	Color shapeColor;

	public static GameObject shapeFillerPrefab;
	public Sprite fillerShape;
	ShapeFiller shapeFiller;

	void Awake () {
		if (shapeFillerPrefab == null)
			shapeFillerPrefab = Resources.Load("ShapeFiller") as GameObject;

		shapeColor = GetComponent<SpriteRenderer>().color;
	}


	public void Hide () {
		gameObject.SetActive(false);
	}

	public void Show () {
		gameObject.SetActive(true);
	}

	public void RequestFill () {

		GameObject sFillerObj = Instantiate(shapeFillerPrefab, transform.position, transform.rotation) as GameObject;
		shapeFiller = sFillerObj.GetComponent<ShapeFiller>();

		shapeFiller.transform.parent = this.transform.parent;

		Vector3 scale = transform.localScale;

		shapeFiller.Setup(Vector3.zero, fillerShape, Vector3.zero, shapeSolid, scale, shapeColor);
		shapeFiller.Fill();

	}

	public void RequestUnfill () {
		if (shapeFiller) shapeFiller.Unfill();
	}

}
