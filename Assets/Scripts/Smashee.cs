using UnityEngine;
using System.Collections;

public class Smashee : MonoBehaviour {

	public TextMesh number;

	public int numToSmash = 5;
	public int numRandomRange = 4; // one sided range on how off random number can be
	public int numSafeRange = 1; // one sided range on 'safe zone' So if numToSmash = 5, then the number can't start from 4 to 6

	int max;
	int min;
	int currentNum;

	float timer = 0f;

	void Start() {
		
		int max = numToSmash + numRandomRange;
		int min = numToSmash - numRandomRange;

		// set starting number
		do currentNum = Random.Range(min, max);
		while (currentNum >= numToSmash - numSafeRange && currentNum <= numToSmash + numSafeRange);

		number.text = currentNum.ToString();
	}
	
	void Update () {

		timer += Time.deltaTime;

		if (timer >= 1) {
			UpdateNumber();
			timer = 0;
		}

	}

	void UpdateNumber() {

		if (currentNum == numToSmash) LoseSmashee();

		if (currentNum < numToSmash) currentNum++;
		else currentNum--;

		number.text = currentNum.ToString();
	}

	void LoseSmashee() {

	}
}
