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

		if (down == null || up == null) return;
		if (down == up) down.TriggerPress();
		else PatternChecker.PC.CheckUserSwipedPattern(down, up);

	}

	void ResetPress() {

	}

	void Update() {

		if (Input.GetButtonDown("Fire1")) {

			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 point2D = new Vector2(point.x, point.y);
			SetDown(SmasheeGenerator.SG.GetSmasheeFromPixelCoord(point2D));

		}

		if (Input.GetButtonUp("Fire1")) {

			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 point2D = new Vector2(point.x, point.y);
			SetUp(SmasheeGenerator.SG.GetSmasheeFromPixelCoord(point2D));

		}

		if (Input.GetKeyDown(KeyCode.F1)) {

			SmasheeGenerator.SG.DebugPrintGrid();

		}

		if (Input.GetKeyDown(KeyCode.F2)) {

			SmasheeGenerator.SG.generate = !SmasheeGenerator.SG.generate;

		}

	}

}
