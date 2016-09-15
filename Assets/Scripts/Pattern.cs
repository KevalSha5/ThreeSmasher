using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Pattern {

	public enum Orientation {Undetermined, Horizontal, Vertical};
	Orientation orientation;

	public static GameObject particlePrefab;
	ParticleSystem particleSystem;

	int smasheeCount = 0;
	List<Smashee> list;
	public static List<Pattern> activePatterns;

	Smashee first;
	Smashee[] middle = new Smashee[2];
	Smashee last;


	public static Pattern NewPattern (List<Smashee> list, Orientation orientation) {

		if (activePatterns == null) activePatterns = new List<Pattern>();
		if (particlePrefab == null)	particlePrefab = (GameObject)Resources.Load("PatternFlare");

		return new Pattern(list, orientation);

	}

	private Pattern (List<Smashee> list, Orientation orientation) {

		this.list = list;
		this.orientation = orientation;
		smasheeCount = list.Count;

		DetermineFirstAndLast();
		DetermineMiddle();

		Pattern patternToRemove = null;
		bool adoptedParticleSystem = false;

		foreach (Pattern other in activePatterns) {
			
			if (this.EqualsPattern(other)) {
				return;
			}
			
			if (this.ContainsPattern(other)) {
				patternToRemove = other;
				AdoptParticleSystem(other);
				adoptedParticleSystem = true;
				break;
			} 

		}

		if (!adoptedParticleSystem)	NewParticleSystem(); // brand new 
		if (patternToRemove != null) patternToRemove.RemovePatternFromAll();

		AddToActivePatterns();

	} 

	private void AddToActivePatterns () {
		activePatterns.Add(this);
	}

	private void RemovePatternFromAll () {
		activePatterns.Remove(this);
	}

	public void DestroyPattern () {
		foreach (Smashee smashee in list)
			MonoBehaviour.Destroy(smashee.gameObject);

		DestoryParticleSystem();
		RemovePatternFromAll();
	}

	void NewParticleSystem () {
		OrientParticleSystem();
		ActivateParticleSystem();
	}

	void AdoptParticleSystem (Pattern other) {
		this.particleSystem = other.particleSystem;
		OrientParticleSystem();
	}

	void ActivateParticleSystem () {
		particleSystem.Play();
	}

	void DestoryParticleSystem () {
		particleSystem.Stop();
	}

	void OrientParticleSystem () {

		if (particleSystem == null)
			particleSystem = (MonoBehaviour.Instantiate(particlePrefab) as GameObject).GetComponent<ParticleSystem>();

		Vector3 box = Vector3.zero;
		box.x = Smashee.width;
		box.z = Smashee.height;

		if (orientation == Orientation.Horizontal) box.x *= smasheeCount;
		if (orientation == Orientation.Vertical) box.z *= smasheeCount;

		ParticleSystem.ShapeModule shape = particleSystem.shape;
		shape.box = box;
		 
		Vector3 pos = Vector3.zero;
		if (smasheeCount % 2 != 0) {
			pos.x = middle[0].transform.position.x;
			pos.y = middle[0].transform.position.y;
		} else {
			pos.x = (middle[0].transform.position.x + middle[1].transform.position.x) / 2f;
			pos.y = (middle[0].transform.position.y + middle[1].transform.position.y) / 2f;
		}

		particleSystem.transform.position = pos;
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

	public bool EqualsPattern (Pattern other) {
		return (this.first == other.first && this.last == other.last) 
			|| (this.first == other.last && this.last == other.first);
	}

	void DetermineFirstAndLast() {

		foreach (Smashee s in list) {

			if (first == null) first = s;
			if (last == null) last = s;

			if (orientation == Orientation.Horizontal) {
				if (s.column < first.column) first = s; 
				else if (s.column > last.column) last = s; 
			} else if (orientation == Orientation.Vertical) {
				if (s.row < first.row) first = s;
				else if (s.row > last.row) first = s;
			}

		}

	}

	void DetermineMiddle () {

		if (first == null || last == null) Debug.LogError("Error on Determine Middle");

		if (smasheeCount % 2 != 0) {

			int middleIndex = -1;

			if (orientation == Orientation.Horizontal) {
				middleIndex = (first.column + last.column) / 2;
				middle[0] = SmasheeGenerator.SG.GetSmashee(middleIndex, first.row);
			} else {
				middleIndex = (first.row + last.row) / 2;
				middle[0] = SmasheeGenerator.SG.GetSmashee(first.column, middleIndex);
			}

		} else {

			int middleIndex1 = -1;
			int middleIndex2 = -1;

			if (orientation == Orientation.Horizontal) {
				middleIndex1 = first.column + smasheeCount / 2 - 1;
				middleIndex2 = last.column - smasheeCount / 2 + 1;
				middle[0] = SmasheeGenerator.SG.GetSmashee(middleIndex1, first.row);
				middle[1] = SmasheeGenerator.SG.GetSmashee(middleIndex2, first.row);
			} else {
				middleIndex1 = first.row + smasheeCount / 2 - 1;
				middleIndex2 = last.row - smasheeCount / 2 + 1;
				middle[0] = SmasheeGenerator.SG.GetSmashee(first.column, middleIndex1);
				middle[1] = SmasheeGenerator.SG.GetSmashee(first.column, middleIndex2);
			}

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

	public override string ToString () {

		string str = "";
		foreach (Smashee smashee in list)
			str += smashee + " ";
		str += orientation;
		return str;		
	
	}

}
