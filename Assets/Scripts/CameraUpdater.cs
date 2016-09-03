using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraUpdater : MonoBehaviour {

	public GameObject backdrop;
	public SpriteRenderer smasheeShape; 
	public SmasheeGenerator smasheeGenerator;
	int gridWidth; //number of smashee horizontally
	int gridHeight; //number of smashee vertically

	void Start () {
		
	}
	
	void Update () {

		gridWidth = smasheeGenerator.numSmasheeInRow;
		gridHeight = smasheeGenerator.numSmasheeInColumn;

		float smasheeWidth = smasheeShape.bounds.extents.x * 2;

		float requiredHorizontalExtent = (smasheeWidth * gridWidth) / 2f;
		float requiredVeritcalExtent = requiredHorizontalExtent * Screen.height / Screen.width;
		Camera.main.orthographicSize = requiredVeritcalExtent;

	}
}
