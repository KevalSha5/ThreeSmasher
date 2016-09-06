using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameoverManager : MonoBehaviour {

	public static GameoverManager Manager;
	public List<Smashee>[] smasheeListArray;

	void Start() {
//
//		this.smasheeListArray = SmasheeGenerator.SG.smasheeListArray;

	}

	void Awake() {
		
		if (Manager != null) Manager = new GameoverManager();
		else Manager = this;

		DontDestroyOnLoad(this);
	}
	
	void Update () {

		int asleepCount = 0;

		for (int i = 0; i < smasheeListArray.Length; i++) {
			foreach (Smashee smashee in smasheeListArray[i])
				if (smashee.isSettled) asleepCount++;

			if (asleepCount >= 6) Debug.Log("GameOver");

			asleepCount = 0;

		}
	
	}
}
