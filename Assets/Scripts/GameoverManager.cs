using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameoverManager : MonoBehaviour {

	public static GameoverManager Manager;
	int[] numSmasheeInColumn;

	void Start() {

		numSmasheeInColumn = SmasheeGenerator.SG.numSmasheeInColumn;

	}

	void Awake() {
		
		if (Manager != null) Manager = new GameoverManager();
		else Manager = this;

		DontDestroyOnLoad(this);
	}
	
	void Update () {

		for (int i = 0; i < numSmasheeInColumn.Length; i++) {
			
			if (numSmasheeInColumn[i] >= 8) SmasheeGenerator.SG.generate = false;

		}
	
	}
}
