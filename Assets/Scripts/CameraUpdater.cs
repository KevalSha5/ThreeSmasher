﻿using UnityEngine;

[ExecuteInEditMode]
public class CameraUpdater : MonoBehaviour {

	public SpriteRenderer smasheeShape;
	public SmasheeGenerator smasheeGenerator;
	int gridWidth;

	void Update () {

		gridWidth = smasheeGenerator.maxColumns;

		float smasheeWidth = smasheeShape.bounds.extents.x * 2;

		float requiredHorizontalExtent = (smasheeWidth * gridWidth) / 2f;
		float requiredVeritcalExtent = requiredHorizontalExtent * Screen.height / Screen.width;

		Camera.main.orthographicSize = requiredVeritcalExtent;

	}
}
