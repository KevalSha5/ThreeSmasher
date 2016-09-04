using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameoverManager : MonoBehaviour {

	List<Smashee>[] smasheeColumnsList;

	public void SetSmasheeColumnsList (List<Smashee>[] listArray) {
		smasheeColumnsList = listArray;
	}
	
	void Update () {

		int asleepCount = 0;

		for (int i = 0; i < smasheeColumnsList.Length; i++) {
			foreach (Smashee smashee in smasheeColumnsList[i])
				if (smashee.isSettled) asleepCount++;

			if (asleepCount >= 6) Debug.Log("GameOver");

			asleepCount = 0;

		}
	
	}
}
