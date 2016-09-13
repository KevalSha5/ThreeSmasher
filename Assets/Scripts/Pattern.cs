using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Pattern {

	public enum Orientation {Horizontal, Vertical};
	public static List<Pattern> allPatterns;

	int count = 0;
	List<Smashee> list;
	Orientation orientation;

	Smashee first;
	Smashee[] middle = new Smashee[2];
	Smashee last;

	public static GameObject particlePrefab;
	ParticleSystem particleSystem;

	public Pattern(List<Smashee> list, Orientation orientation) {

		if (allPatterns == null) allPatterns = new List<Pattern>();
		if (particlePrefab == null) particlePrefab = (GameObject)Resources.Load("PatternFlare");

		this.list = list;
		this.orientation = orientation;
		count = list.Count;

		InitVariables();

		List<Pattern> patternsToRemove = new List<Pattern>();
		bool adoptedSystem = false;

		foreach (Pattern other in allPatterns) {
			
			if (this.EqualsPattern(other)) { // same pattern 

//				Debug.Log("Found a duplicate");
				return;

			}
			
			if (this.ContainsPattern(other)) { // better pattern

				patternsToRemove.Add(other);
				AdoptParticleSystem(other);
				adoptedSystem = true;

			} 

		}

		if (!adoptedSystem)	NewParticleSystem(); // brand new 

		AddToAllPatterns();

		foreach (Pattern other in patternsToRemove)
			other.RemovePatternFromAll();

	}

	void NewParticleSystem() {
//		if (allPatterns.Count == 0) Debug.Log("Frist Pattern");
//		Debug.Log("New Pattern");
//		Debug.Log(this);
		PositionParticleSystem();
		ActivateParticleSystem();
	}

	void AdoptParticleSystem(Pattern other) {
//		Debug.Log("Adopted Pattern");
//		Debug.Log(this + " contains " + other);
		this.particleSystem = other.particleSystem;
		PositionParticleSystem();
	}

	void InitVariables() {
		if (orientation == null) DetermineOrientation();
		DetermineFirst();
		DetermineLast();
		DetermineMiddle();
	}

	void DetermineOrientation() {
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

	void PositionParticleSystem() {

		if (particleSystem == null)
			particleSystem = ((GameObject)MonoBehaviour.Instantiate(particlePrefab)).GetComponent<ParticleSystem>();

		Vector3 box = Vector3.zero;
		box.x = Smashee.width;
		box.z = Smashee.height;

		if (orientation == Orientation.Horizontal) box.x *= count;
		if (orientation == Orientation.Vertical) box.z *= count;

		ParticleSystem.ShapeModule shape = particleSystem.shape;
		shape.box = box;
		 
		Vector3 pos = Vector3.zero;
		if (count%2 != 0) {
			pos.x = middle[0].transform.position.x;
			pos.y = middle[0].transform.position.y;
		} else {
			pos.x = (middle[0].transform.position.x + middle[1].transform.position.x)/2f;
			pos.y = (middle[0].transform.position.y + middle[1].transform.position.y)/2f;
		}

		particleSystem.transform.position = pos;
	}

	void ActivateParticleSystem() {
		particleSystem.Play();
	}


	void DetermineFirst() {

		foreach (Smashee s in list) {
			if (first == null) first = s;	

			if (orientation == Orientation.Horizontal && s.column < first.column) {
				first = s;
			} else if (orientation == Orientation.Vertical && s.row < first.row) {
				first = s;	
			} 
		}

	}

	void DetermineLast() {

		foreach (Smashee s in list) {

			if (last == null) last = s;

			if (orientation == Orientation.Horizontal && s.column > first.column) {
				last = s;
			} else if (orientation == Orientation.Vertical && s.row > first.row) {
				last = s;	
			} 

		}

	}

	void DetermineMiddle() {

		if (first == null) DetermineFirst();
		if (last == null) DetermineLast();

		if (count%2 != 0) {

			int middleIndex = -1;

			if (orientation == Orientation.Horizontal) {
				middleIndex = (first.column + last.column)/2;
				middle[0] = SmasheeGenerator.SG.GetSmashee(middleIndex, first.row);
			} else {
				middleIndex = (first.row + last.row)/2;
				middle[0] = SmasheeGenerator.SG.GetSmashee(first.column, middleIndex);
			}


		} else {

			int middleIndex1 = -1;
			int middleIndex2 = -1;

			if (orientation == Orientation.Horizontal) {
				middleIndex1 = first.column + count/2 - 1;
				middleIndex2 = last.column - count/2 + 1;
				middle[0] = SmasheeGenerator.SG.GetSmashee(middleIndex1, first.row);
				middle[1] = SmasheeGenerator.SG.GetSmashee(middleIndex2, first.row);
			} else {
				middleIndex1 = first.row + count/2 - 1;
				middleIndex2 = last.row - count/2 + 1;
				middle[0] = SmasheeGenerator.SG.GetSmashee(first.column, middleIndex1);
				middle[1] = SmasheeGenerator.SG.GetSmashee(first.column, middleIndex2);
			}

		}

	}

	bool ContainsPattern(Pattern other) {
//		Debug.Log("Checking " + this + " contains " + other);
//		Debug.Log("Result: " + !other.list.Except(this.list).Any());
		return !other.list.Except(this.list).Any();
	}

	bool EqualsPattern(Pattern other) {

//		return (this.first == other.first && this.last == other.last);

		if (this.list == null || other.list == null) return false;
		if (this.list.Count != other.list.Count) return false;
		if (this.orientation != other.orientation) return false;

		Dictionary<Smashee, int> hash = new Dictionary<Smashee, int>();

		foreach (Smashee smashee in this.list) {
			if (hash.ContainsKey(smashee)) hash[smashee]++;
			else hash.Add(smashee, 1);
		}

		foreach (Smashee smashee in other.list) {
			if (!hash.ContainsKey(smashee) || hash[smashee] == 0) return false;
		}

		return true;

	}

	private void AddToAllPatterns() {
		allPatterns.Add(this);

//		foreach (Smashee smashee in list) {
//			smashee.square.color = Color.white;
//		}

	}

	private void RemovePatternFromAll() {
		allPatterns.Remove(this);
	}
		

	public override string ToString() {

		string str = "";
		foreach (Smashee smashee in list)
			str += smashee + " ";
		str += orientation;
		return str;		
	
	}

}
