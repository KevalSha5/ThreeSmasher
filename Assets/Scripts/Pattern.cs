using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Pattern {

	public enum Orientation {Undetermined, Horizontal, Vertical};

	public static GameObject patternHighlight;
	PatternHighlighter ph;

	List<Smashee> list;
	public static List<Pattern> activePatterns;

	public int size {get{return list.Count;}}

	public Orientation orientation;
	public Smashee first;
	public Smashee[] middle = new Smashee[2];
	public Smashee last;

	public static void InitStaticVars () {
		activePatterns = new List<Pattern>();
		patternHighlight = (GameObject)Resources.Load("PatternHighlight");
	}	

	public static void NewPattern (List<Smashee> list, Orientation orientation) {
	
		Pattern newPattern = new Pattern(list, orientation);
		bool absorbed = false;

		foreach (Pattern other in activePatterns) {
				
			if (newPattern.EqualsPattern(other)) { 
				return;
			}

			if (other.ContainsPattern(newPattern)) {
				// A worse pattern is being made
				return;
			}
			
			if (newPattern.ContainsPattern(other)) {
				newPattern.AbsorbPattern(other);
				absorbed = true;
				break;
			}
		}

		if (!absorbed) newPattern.NewPatternHighlighter();

		AddToActivePatterns(newPattern);
		// Debug.Log("New Pattern: " + newPattern);

	}

	private Pattern (List<Smashee> patternList, Orientation orientation) {

		// assumes pattern has met required length

		this.list = patternList;
		this.orientation = orientation;

		SortList();
		SetFirstLast();
		SetMiddle();

	} 

	public void SmashPattern () {
		foreach (Smashee smashee in list) {
			PatternManager.PM.RemoveFromPatternsOtherThan(smashee, this);
			smashee.Smash();
		}

		GetPoints(size);
		DismissPattern();
	}

	public void DismissPattern() {
		// Debug.Log("Dismissing" + this);
		ClearPatternHighlighter();
		RemovePatternFromActive(this);
	}

	public void AbsorbPattern (Pattern other) {
		AdoptPatternHighlighter(other);
		RemovePatternFromActive(other);
	}

	public void RemoveSmashee (Smashee smashee) {

		if (!list.Contains(smashee)) return;

		
		if (!PatternManager.PM.ValidateLength(size - 1)) {
			DismissPattern();
			return;
		}

		list.Remove(smashee);

		if (smashee == first || smashee == last) {
			SetFirstLast();
			// ph.AdjustHighlight(this);
		} else {
			// pattern may have enough on both sides to split
			// pattern, this is temporary solution to splitting
			DismissPattern();
			PatternManager.PM.RequestPatternCheck(first);
			PatternManager.PM.RequestPatternCheck(last);
		}

	}

	public List<Smashee> GetSmashees() {
		return list;
	}

	public bool HasEnds (Smashee down, Smashee up) {
		return (this.first == down && this.last == up) || (this.first == up && this.last == down);
	}

	public bool BreaksPattern (Pattern pattern) {
		return false;
	}

	public bool ContainsPattern (Pattern other) {
		return !other.list.Except(this.list).Any();
	}

	public bool ContainsSmashee (Smashee smashee) {
		return list.Contains(smashee);
	}

	public bool EqualsPattern (Pattern other) {
		return (this.first == other.first && this.last == other.last) 
			|| (this.first == other.last && this.last == other.first);
	}

	public Smashee GetSmasheeOtherThan (Smashee smashee) {
		int index = list.IndexOf(smashee);
		return list.ElementAt(index == 0 ? 1 : 0);
	}

	public int GetSpread(Smashee one, Smashee two) {

		int result = -1;

		if (orientation == Orientation.Horizontal) {
			result = Mathf.Abs(one.column - two.column);

		} else if (orientation == Orientation.Vertical) {
			result = Mathf.Abs(one.row - two.row);

		} 

		return result;

	}

	public Smashee GetLastAdded () {
		return list.OrderBy(s => s.orderAdded).Last();
	}

	void NewPatternHighlighter () {
		GameObject go = MonoBehaviour.Instantiate(patternHighlight) as GameObject;
		ph = go.GetComponent<PatternHighlighter>();
		ph.NewHighlight(this);
	}

	void AdoptPatternHighlighter (Pattern other) {
		this.ph = other.ph;
		ph.AdjustHighlight(this);
	}

	void ClearPatternHighlighter () {
		ph.Clear();
	}

	void GetPoints(int smashed) {

		int gained = PointsManager.PM.GetPointsForSmashing(size);

		Vector3 centerPoint = (first.transform.position + last.transform.position) / 2;
		Color color = Color.green;

		FloatingTextFactory.NewText("+" + gained, color, 3.5f, 1.5f, centerPoint, Quaternion.identity);

	}

	void SortList () {
		
		if (orientation == Orientation.Horizontal) {			
			list.Sort((s1, s2) => s1.column.CompareTo(s2.column));

		} else if (orientation == Orientation.Vertical) {			
			list.Sort((s1, s2) => s1.row.CompareTo(s2.row));

		}

	}

	void SetFirstLast () {
		first = list.First();
		last = list.Last();
	}

	void SetMiddle () {

		if (first == null || last == null) Debug.LogError("Error on Determine Middle");

		if (size %2 == 0) {

			int middleIndex1 = size/2;
			int middleIndex2 = size/2 - 1;

			middle[0] = list.ElementAt(middleIndex1);
			middle[1] = list.ElementAt(middleIndex2);

		} else {

			int middleIndex = (size - 1)/2;
			middle[0] = list.ElementAt(middleIndex);

		}

	}

	void DetermineOrientation () {
		Smashee first = list.First();
		Smashee last = list.Last();

		if (first.column == last.column)
			orientation = Orientation.Vertical;

		if (first.row == last.row)
			orientation = Orientation.Horizontal;

		if ((first.column == last.column && first.row == last.row) ||
			(first.column != last.column && first.row != last.row)) {
			Debug.LogError("Possible Error in DetermineOrientation()");
		}
	}

	private static void AddToActivePatterns (Pattern pattern) {
		activePatterns.Add(pattern);
	}

	private static void RemovePatternFromActive (Pattern pattern) {
		activePatterns.Remove(pattern);
	}

	public static void LogActivePatterns () {
		foreach (Pattern p in activePatterns)
			Debug.Log(p);
	}

	public override string ToString () {

		string str = "";
		foreach (Smashee smashee in list)
			str += smashee + " ";
		str += orientation;
		return str;		
	
	}

}
