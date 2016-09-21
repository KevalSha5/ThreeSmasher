using UnityEngine;

public class ShapeFiller : MonoBehaviour {

	enum FillState {Paused, Filling, Unfilling};
	FillState fillState;

	public SpriteRenderer shapeToFill;
	public SpriteRenderer fillerShape;

	Vector3 maxScale;

	public static int counter = 2;
	int stencilId;

	void Start () {

	}

	void Update () {

		if (fillState == FillState.Filling) {
			AnimateFill();
		} else if (fillState == FillState.Unfilling) {
			AnimateUnfill();
		} 



	}

	public void Setup (Vector3 fillerShapePos, Sprite fillerShape,
	            	   Vector3 shapeToFillPos, Sprite shapeToFill,
					   Vector3 scale, Color color) {

		stencilId = counter++;
		transform.localScale = scale;

		this.fillerShape.sprite = fillerShape;
		this.fillerShape.material = new Material(Shader.Find("Masking/Mask")); 
		this.fillerShape.material.SetInt( "_StencilID", stencilId);
		this.fillerShape.transform.localPosition = fillerShapePos;
		this.fillerShape.transform.localScale = Vector3.zero;

		this.shapeToFill.sprite = shapeToFill;
		this.shapeToFill.material = new Material(Shader.Find("Masking/Masked")); 
		this.shapeToFill.material.SetInt( "_StencilID", stencilId);
		this.shapeToFill.material.color = color;
		this.shapeToFill.transform.localPosition = shapeToFillPos;
	}

	public void Fill () {
		this.fillState = FillState.Filling;
	}

	public void Unfill () {
		this.fillState = FillState.Unfilling;
	}

	public bool Unfilled () {
		return fillerShape.transform.localScale == Vector3.zero;
	}

	void AnimateFill () {

		Vector3 localScale = fillerShape.transform.localScale;
		localScale = Vector3.Lerp(localScale, Vector3.one, Time.deltaTime * 2f);
		fillerShape.transform.localScale = localScale;

	}

	void AnimateUnfill () {

		Vector3 localScale = fillerShape.transform.localScale;
		localScale = Vector3.Lerp(localScale, Vector3.zero, Time.deltaTime * 5f);
		fillerShape.transform.localScale = localScale;

	}

}
