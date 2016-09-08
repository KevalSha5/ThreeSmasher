﻿using UnityEngine;
using System.Collections;

public class SmasheeFall : MonoBehaviour {

	public float speed;
	Rigidbody2D rb;
	BoxCollider2D bc;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<BoxCollider2D>();
	}
	
	void Update () {
	
		Vector3 displacement = Vector3.down * Time.deltaTime * speed;
		// Debug.Log(displacement);

		float horizontalTolerance = .9f;
		float extentX = bc.bounds.extents.x * horizontalTolerance;
		float extentY = bc.bounds.extents.y;
		Vector3 center = bc.bounds.center + displacement;

		Vector3 topLeftCorner = center - new Vector3(extentX, extentY, 0);
		Vector3 bottomRightCorner = center + new Vector3(extentX, extentY, 0);

		Collider2D otherCollider = null;
		Collider2D[] overlaps = Physics2D.OverlapAreaAll(topLeftCorner, bottomRightCorner);

		foreach (Collider2D collider in overlaps) {
			if (collider.gameObject != this.gameObject) {

				if (collider.bounds.center.y < center.y)
					otherCollider = collider;
			}
		}

		if (otherCollider) {
			Vector3 otherColliderCenter = otherCollider.bounds.center;
			float otherColliderExtentY = otherCollider.bounds.extents.y;

			float adjustedYPos = otherColliderCenter.y + otherColliderExtentY + extentY;

			Vector3 newPos = transform.position;
			newPos.y = adjustedYPos;
			transform.position = newPos;
		} else {
			transform.position += displacement;
		}
	}
}
