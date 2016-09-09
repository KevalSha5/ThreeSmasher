using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Rules : MonoBehaviour {

	public static Rules RulePatterns;
	public enum Direction {Horizontal, Vertical, MainDiagnol, AntiDiagnol};

	SmasheeGenerator SG;
	Smashee[,] grid;
	Smashee startingSmashee;
	int requiredLength = 3;
	public bool dirty = true;

	void Awake() {

		if (RulePatterns != null) RulePatterns = new Rules();
		else RulePatterns = this;

		DontDestroyOnLoad(this);

	}

	void Start() {
		SG = SmasheeGenerator.SG;
		grid = SG.settledSmasheeGrid;
	}

	void Update() {

		if (!dirty) return;

		for (int x = 0; x < grid.GetLength(0); x++) {
			for (int y = 0; y < grid.GetLength(1); y++) {
				if (grid[x, y] != null) {
					CheckInDirection(grid[x, y], Direction.Horizontal);
					CheckInDirection(grid[x, y], Direction.Vertical);
//					CheckInDirection(grid[x, y], Direction.MainDiagnol);
//					CheckInDirection(grid[x, y], Direction.AntiDiagnol);

				}
			}
		}

//		foreach (Smashee smashee in grid) {
//			if (smashee != null) {
//				CheckInDirection(smashee, Direction.Horizontal);
//				CheckInDirection(smashee, Direction.Vertical);
////				CheckInDirection(smashee, Direction.MainDiagnol);
////				CheckInDirection(smashee, Direction.AntiDiagnol);
//			}
//		}

		dirty = false;
	}

	void CheckInDirection(Smashee smashee, Direction directionEnum) {

		startingSmashee = smashee;

		Vector2 direction1 = Vector2.up;
		Vector2 direction2 = Vector2.up;

		switch (directionEnum) {
		case Direction.Horizontal:
			direction1 = Vector2.left;
			direction2 = Vector2.right;
			break;
		case Direction.Vertical:
			direction1 = Vector2.up;
			direction2 = Vector2.down;
			break;
		case Direction.MainDiagnol:
			direction1 = Vector2.down + Vector2.left;
			direction2 = Vector2.up + Vector2.right;
			break;
		case Direction.AntiDiagnol:
			direction1 = Vector2.up + Vector2.left;
			direction2 = Vector2.down + Vector2.right;
			break;
		}

		List<Smashee> side1 = new List<Smashee>();
		List<Smashee> side2 = new List<Smashee>();

		SG.DebugPrintGrid();


		try {
			side1 = GetAdjacent(smashee, direction1, new List<Smashee>());
		} catch (System.Exception e) {
			Debug.Log(e);
			Debug.Log(directionEnum);
			Debug.Log("Direction 1 OVERFLOW");
			SG.DebugPrintGrid();
			Debug.Log(startingSmashee.column + " " + startingSmashee.row);
			Debug.Break();
		}

		try {
			side2 = GetAdjacent(smashee, direction2, new List<Smashee>());
		} catch (System.Exception e) {
			Debug.Log(e);
			Debug.Log(directionEnum);
			Debug.Log("Direction 2 OVERFLOW");
			SG.DebugPrintGrid();
			Debug.Log(startingSmashee.column + " " + startingSmashee.row);
			Debug.Break();
		}

		List<Smashee> all = side1.Concat(side2).ToList();
		all.Add(smashee);

		if (all.Count >= requiredLength) {
			foreach (Smashee adjacent in all) {
				Destroy(adjacent.gameObject);
			}
		}

		if (startingSmashee.debugging) Debug.Log("\n\n\n");

	}

	List<Smashee> GetAdjacent(Smashee smashee, Vector2 direction, List<Smashee> list) {
		
		int x = smashee.column + (int)direction.x;
		int y = smashee.row + (int)direction.y;

		Smashee adjacent = null;
		if (SG.CheckGridBounds(x, y))
			adjacent = grid[x, y];

		if (adjacent != null && adjacent.HasSameShape(smashee)) {

			if (startingSmashee.debugging) {
				Debug.Log("Adding");
			}

			list.Add(grid[x, y]);
			GetAdjacent(grid[x,y], direction, list);
		}

		return list;
	}

}
