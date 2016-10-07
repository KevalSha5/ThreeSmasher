using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PatternManager : MonoBehaviour {

	public static PatternManager PM;

	struct Direction {
		public Vector2 direction1;
		public Vector2 direction2;

		public Direction (Vector2 direction1, Vector2 direction2) {
			this.direction1 = direction1;
			this.direction2 = direction2;
		}
	}

	Direction horizontal = new Direction(Vector2.left, Vector2.right);
	Direction vertical = new Direction(Vector2.up, Vector2.down);

	SmasheeGenerator SG;
	int requiredLength = 3;
	int selectionTolerance = 2;

	void Awake () {

		if (PM == null)	PM = this;
		else if (PM != this) Destroy(this);

		DontDestroyOnLoad(this);

	}

	void Start () {
		SG = SmasheeGenerator.SG;
		Pattern.InitStaticVars();
	}

	public void CheckUserSwipedPattern (Smashee down, Smashee up) {

		Pattern swipedPattern = null;

		foreach (Pattern pattern in Pattern.activePatterns) {

			if (pattern.HasEnds(down, up)) {
				swipedPattern = pattern;
				break;

			} else {

				if (!(pattern.ContainsSmashee(down) && pattern.ContainsSmashee(up))) continue;

				int displacement = pattern.GetSpread(down, up);

				// TODO: Make the tolerance also depends on the size of the pattern

				if (pattern.size - displacement <= selectionTolerance) {
					swipedPattern = pattern;
					break;
				}

			}

		}

		if (swipedPattern != null) swipedPattern.SmashPattern();

	}

	public void HandleSmasheeStateChange (Smashee smashee) {
		RemoveFromPatterns(smashee);
		CheckForPatterns(smashee);
	}

	public void RemoveFromPatterns (Smashee smashee) {
		foreach (Pattern pattern in Pattern.activePatterns.ToList()) {
			pattern.RemoveSmashee(smashee);
		}
	}

	public void RemoveFromPatternsOtherThan (Smashee smashee, Pattern pattern) {
		foreach (Pattern p in Pattern.activePatterns.ToList()) {
			if (p == pattern) continue;
			p.RemoveSmashee(smashee);
		}
	}

	public void RequestPatternCheck (Smashee smashee) { //For a particular smashee
		CheckForPatterns(smashee);
	}

	public bool ValidateLength (int length) {
		return length >= requiredLength;
	}

	void CheckForPatterns (Smashee smashee) {
		
		if (smashee.IsFalling()) return; // method will work without this; this is just for effeciency

		List<Smashee> horizontalPattern = GetPossiblePattern(smashee, horizontal);
		List<Smashee> verticalPattern = GetPossiblePattern(smashee, vertical);

		if (ValidateLength(horizontalPattern.Count))
			Pattern.NewPattern(horizontalPattern, Pattern.Orientation.Horizontal);
			
		if (ValidateLength(verticalPattern.Count))
			Pattern.NewPattern(verticalPattern, Pattern.Orientation.Vertical);
		
	}

	List<Smashee> GetPossiblePattern (Smashee smashee, Direction direction) {

		Vector2 direction1 = direction.direction1; //assiging Vector.zero leads to infinte loop
		Vector2 direction2 = direction.direction2;

		List<Smashee> side1 = GetAdjacent(smashee, direction1, new List<Smashee>());
		List<Smashee> side2 = GetAdjacent(smashee, direction2, new List<Smashee>());

		List<Smashee> all = side1.Concat(side2).ToList();
		all.Add(smashee);

		return all;
	}

	List<Smashee> GetAdjacent (Smashee smashee, Vector2 direction, List<Smashee> list) {
		
		int x = smashee.column + (int)direction.x;
		int y = smashee.row + (int)direction.y;

		Smashee adjacent = SG.GetSmashee(x, y);

		if (adjacent != null && adjacent.HasSameShape(smashee)) {
			list.Add(adjacent);
			GetAdjacent(adjacent, direction, list);
		}

		return list;
	}

}
