using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PatternChecker : MonoBehaviour {

	public static PatternChecker PC;

	struct Direction {
		public Vector2 direction1;
		public Vector2 direction2;

		public Direction(Vector2 direction1, Vector2 direction2) {
			this.direction1 = direction1;
			this.direction2 = direction2;
		}
	}

	Direction horizontal = new Direction(Vector2.left, Vector2.right);
	Direction vertical = new Direction(Vector2.up, Vector2.down);
	Direction mainDiagnol = new Direction(Vector2.down + Vector2.left, Vector2.up + Vector2.right);
	Direction antiDiagnol = new Direction(Vector2.up + Vector2.left, Vector2.down + Vector2.right);

	SmasheeGenerator SG;
	Smashee[,] grid;
	int requiredLength = 3;

	void Awake() {

		if (PC != null) PC = new PatternChecker();
		else PC = this;

		DontDestroyOnLoad(this);

	}

	void Start() {
		SG = SmasheeGenerator.SG;
		grid = SG.settledSmasheeGrid;

	}

	public void RequestPatternCheck(Smashee smashee) {
		CheckForPatterns(smashee);
	}

	public void RequestPatternCheck() {
		for (int x = 0; x < grid.GetLength(0); x++) {
			for (int y = 0; y < grid.GetLength(1); y++) {

				if (grid[x, y] == null) continue;
				CheckForPatterns(grid[x, y]);

			}
		}
	}

	void CheckForPatterns(Smashee smashee) {
		List<Smashee> horizontalPattern = GetPattern(smashee, horizontal);
		List<Smashee> verticalPattern = GetPattern(smashee, vertical);

		if (horizontalPattern.Count >= requiredLength) 
			new Pattern(horizontalPattern, Pattern.Orientation.Horizontal);
		if (verticalPattern.Count >= requiredLength) 
			new Pattern(verticalPattern, Pattern.Orientation.Vertical);
	}

	void DestroyPattern(List<Smashee> list) {
		foreach (Smashee smashee in list) {
			Destroy(smashee.gameObject);
		}
	}

	List<Smashee> GetPattern(Smashee smashee, Direction direction) {

		Vector2 direction1 = direction.direction1; //assiging Vector.zero leads to infinte loop
		Vector2 direction2 = direction.direction2;

		List<Smashee> side1 = GetAdjacent(smashee, direction1, new List<Smashee>());
		List<Smashee> side2 = GetAdjacent(smashee, direction2, new List<Smashee>());

		List<Smashee> all = side1.Concat(side2).ToList();
		all.Add(smashee);

		return all;
	}

	List<Smashee> GetAdjacent(Smashee smashee, Vector2 direction, List<Smashee> list) {
		
		int x = smashee.column + (int)direction.x;
		int y = smashee.row + (int)direction.y;

		Smashee adjacent = null;
		if (SG.CheckGridBounds(x, y))
			adjacent = grid[x, y];

		if (adjacent != null && adjacent.HasSameShape(smashee)) {
			list.Add(grid[x, y]);
			GetAdjacent(grid[x,y], direction, list);
		}

		return list;
	}

}
