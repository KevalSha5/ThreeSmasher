using UnityEngine;

public class SmasheeSmashed : MonoBehaviour {

	Vector3 velocity;
	Quaternion rotation;
	Vector3 gravity;

	void Start () {

		velocity = Vector3.up * Random.value / 2f;
		velocity.x = (Random.value -.5f) / 5f;

		// rotation = Quaternion.Euler(Random.value, Random.value, 0);

		gravity = Vector3.down * 2f;

	}
	
	void Update () {
		
		velocity += gravity * Time.deltaTime;
		transform.position = transform.position + velocity;

		// transform.rotation *= rotation;

		// TODO: Destory gameobject once out of camera bounds

	}
}
