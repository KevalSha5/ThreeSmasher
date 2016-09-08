using UnityEngine;
using System.Collections;

public class FloorPositioner : MonoBehaviour {

	public BoxCollider2D bc;

	void Start () {

		bc = GetComponent<BoxCollider2D>();

		Vector2 size = Vector2.zero;
		size.x = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
		size.y = 5;
		bc.size = size;

		Vector2 newPos = transform.position;
		newPos.y = -(Camera.main.orthographicSize + bc.size.y/2f);
		transform.position = newPos;

	}

}
