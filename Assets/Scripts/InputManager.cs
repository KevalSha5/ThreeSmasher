using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public static InputManager IM;

	Smashee down;
	Smashee up;

	void Awake() {

		if (IM == null) IM = this;
		else if (IM != this) Destroy(this);

		DontDestroyOnLoad(this);

	}

	public void SetDown(Smashee smashee) {
		down = smashee;
	}

	public void SetUp(Smashee smashee) {
		up = smashee;

		CheckPress();
		ResetPress();
	}

	void CheckPress() {

		if (down == up) down.TriggerPress();

	}

	void ResetPress() {

	}

	void Update() {

		if (Input.GetButtonDown("Fire1")) {

			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 point2D = new Vector2(point.x, point.y);

			Collider2D collider = Physics2D.OverlapPoint(point2D, LayerMask.GetMask("Smashee"));
			SetDown(collider.gameObject.GetComponent<Smashee>());

		}

		if (Input.GetButtonUp("Fire1")) {

			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 point2D = new Vector2(point.x, point.y);

			Collider2D collider = Physics2D.OverlapPoint(point2D, LayerMask.GetMask("Smashee"));
			SetUp(collider.gameObject.GetComponent<Smashee>());

		}

	}

}
