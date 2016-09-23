using UnityEngine;

public class FloatingTextFactory {

	public static GameObject prefab;

	public static void NewText (string message, Color color, float floatSpeed,
								float fadeSpeed, Vector3 pos, Quaternion rot) {

		if (prefab == null) prefab = Resources.Load("FloatingText") as GameObject;

		GameObject floatingText = MonoBehaviour.Instantiate(prefab, pos, rot) as GameObject;
		
		floatingText.GetComponent<FloatingText>().textMesh.text = message;
		floatingText.GetComponent<FloatingText>().textMesh.color = color;

		floatingText.GetComponent<FloatingText>().floatSpeed = floatSpeed;
		floatingText.GetComponent<FloatingText>().fadeSpeed = fadeSpeed;

	}

}
