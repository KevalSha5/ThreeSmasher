using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameoverManager : MonoBehaviour {

	public static GameoverManager Manager;
	SmasheeGenerator SG;
	Smashee[,] grid;
	int columnCount;

	void Start() {
		SG = SmasheeGenerator.SG;
		grid = SG.settledSmasheeGrid;
	}

	void Awake() {
		if (Manager != null) Manager = new GameoverManager();
		else Manager = this;

		DontDestroyOnLoad(this);
	}
	
	void Update () {
		for (int x = 0; x < grid.GetLength(0); x++) {
			for (int y = 0; y < grid.GetLength(1); y++) {
				if (grid[x, y] != null) columnCount++;
			}

			if (columnCount >= SG.maxRows) 
				SG.generate = false;

			columnCount = 0;
		}
	
	}
}
