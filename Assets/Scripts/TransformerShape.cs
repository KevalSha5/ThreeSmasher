using UnityEngine;
using System;

[RequireComponent (typeof(LineRenderer))]
public class TransformerShape : MonoBehaviour {

	LineRenderer lr;
	
	public int numSides = 3;
	public float radius = 2;
	
	Vector3[] vertices;
	Vector3[] transformToVertices;

	Vector3 rotation;
	Quaternion destRotation;

	void Awake () {
		lr = GetComponent<LineRenderer>();
	}

	void Start () {
		
		// AssignVertices(ref vertices, numSides);
	 	// transformToVertices = GetVertices(sides);
		// TransformSides(5);

		vertices = new Vector3[numSides + 1];
		transformToVertices = new Vector3[numSides + 1];

		SetTransformationVertices(numSides);

		lr.SetVertexCount(numSides + 1);
		lr.SetPositions(vertices);

		rotation = Vector3.zero;
		destRotation = Quaternion.Euler(rotation);

	}

	void TransformShape (int newNumSides) {

		if (newNumSides < 0) return;

		bool increased = IncreaseArraySizeIfNecessary(newNumSides);

		if (increased) {
			for (int i = numSides; i < newNumSides + 1; i++) {
				vertices[i] = vertices[numSides];
			}
		}

		SetTransformationVertices(newNumSides);

		rotation.z = Mathf.PI / Mathf.Pow(2, (numSides - 2)) * Mathf.Rad2Deg;
		destRotation = Quaternion.Euler(rotation) * Quaternion.Euler(new Vector3(0, 0, 180)) * transform.rotation;

		numSides = newNumSides;

	}

	void SetTransformationVertices (int newNumSides) {

		float thetaStep = Mathf.PI * 2 / newNumSides;

		for (int i = 0; i < newNumSides; i++) {

			Vector3 vertex = new Vector3();
			vertex.x = Mathf.Cos(thetaStep * i) * radius;
			vertex.y = Mathf.Sin(thetaStep * i) * radius;

			transformToVertices[i] = vertex;
		}

		transformToVertices[newNumSides] = transformToVertices[0];

	}

	bool IncreaseArraySizeIfNecessary (int newNumSides) {
		if (newNumSides + 1 > vertices.Length) { 
			Array.Resize<Vector3>(ref vertices, newNumSides + 1);
			transformToVertices = new Vector3[newNumSides + 1];
			lr.SetVertexCount(vertices.Length);
			return true;
		}
		return false;
	}
	
	void Update () {

		if (Input.GetButtonDown("Fire1")) TransformShape(numSides + 1);
		else if (Input.GetButtonDown("Fire2")) TransformShape(numSides - 1);

		if (vertices.Length != transformToVertices.Length)
			Debug.LogError("vertices side length do not match");

		for (int i = 0; i < vertices.Length; i++) {
			vertices[i] = Vector3.Lerp(vertices[i], transformToVertices[i], Time.deltaTime * 5);
		}

		lr.SetPositions(vertices);
		transform.rotation = Quaternion.Lerp( transform.rotation, destRotation, Time.deltaTime * 5);

	}
}
