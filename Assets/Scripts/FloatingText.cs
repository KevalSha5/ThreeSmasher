using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

	Color textColor;
	public TextMesh textMesh;
	public float fadeSpeed;
	public float floatSpeed;

	void Update () {
	
		textColor = textMesh.color;

		textColor.a -= Time.deltaTime * fadeSpeed;
		textMesh.color = textColor;
		if (textColor.a <= 0) Destroy(this.gameObject);

		transform.position += Vector3.up * floatSpeed;

	}
}
