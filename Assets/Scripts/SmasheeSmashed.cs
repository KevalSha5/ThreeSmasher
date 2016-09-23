using UnityEngine;

public class SmasheeSmashed : MonoBehaviour {

	Vector3 velocity;
	Quaternion rotation;
	Vector3 gravity;

	void Start () {

		velocity = Vector3.zero;

		velocity.x = (Random.value -.5f) / 5f;
		velocity.y = Random.value / 2f;
		velocity.z = -0.2f; // bring it forward 

		gravity = Vector3.down * 2f;

	}
	
	void Update () {
		
		velocity += gravity * Time.deltaTime;
		transform.position = transform.position + velocity;

		// TODO: Destory gameobject once out of camera bounds

	}
}
