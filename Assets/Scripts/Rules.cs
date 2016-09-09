using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Rules : MonoBehaviour {

	public enum Direction {Horizontal, Vertical, MainDiagnol, AntiDiagnol};

	SmasheeGenerator SG;
	Smashee[,] grid;
	int requiredLength = 3;

	void Start() {
		SG = SmasheeGenerator.SG;
		grid = SG.settledSmasheeGrid;
	}

	void Update() {
		foreach (Smashee smashee in grid) {
			if (smashee != null) {
				CheckInDirection(smashee, Direction.Horizontal);
				CheckInDirection(smashee, Direction.Vertical);
//				CheckInDirection(smashee, Direction.MainDiagnol);
//				CheckInDirection(smashee, Direction.AntiDiagnol);
			}
		}
	}

	void CheckInDirection(Smashee smashee, Direction directionEnum) {

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

		List<Smashee> side1 = GetAdjacent(smashee, direction1, new List<Smashee>());
		List<Smashee> side2 = GetAdjacent(smashee, direction2, new List<Smashee>());

		List<Smashee> all = side1.Concat(side2).ToList();
		all.Add(smashee);

		if (all.Count >= requiredLength) {
			foreach (Smashee adjacent in all)
				Destroy(adjacent.gameObject);
		}
	}

	List<Smashee> GetAdjacent(Smashee smashee, Vector2 direction, List<Smashee> list) {
		
		int x = smashee.column + (int)direction.x;
		int y = smashee.row + (int)direction.y;

		if (SG.CheckGridBounds(x, y) &&
			grid[x, y] != null &&
			smashee.HasSameShape(grid[x, y])) {

			Debug.Log("x: " + x + " y: " + y);

			list.Add(grid[x, y]);
			GetAdjacent(grid[x,y], direction, list);
		}

		return list;
	}

}
