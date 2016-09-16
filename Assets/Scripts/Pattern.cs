using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Pattern {

	public enum Orientation {Undetermined, Horizontal, Vertical};

	public static GameObject particlePrefab;
	ParticleSystem particleSystem;

	List<Smashee> list;
	public static List<Pattern> activePatterns;

	public int size {get{return list.Count;}}

	public Orientation orientation;
	public Smashee first;
	public Smashee[] middle = new Smashee[2];
	public Smashee last;


	public static Pattern NewPattern (List<Smashee> list, Orientation orientation) {

		if (activePatterns == null) activePatterns = new List<Pattern>();
		if (particlePrefab == null)	particlePrefab = (GameObject)Resources.Load("PatternFlare");

		return new Pattern(list, orientation);

	}

	private Pattern (List<Smashee> list, Orientation orientation) {

		this.list = list;
		this.orientation = orientation;

		SortList();
		SetFirstLast();
		SetMiddle();

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

	public void CrushPattern () {
		foreach (Smashee smashee in list) {
			PatternChecker.PC.CheckPatternsBroken(smashee);
			MonoBehaviour.Destroy(smashee.gameObject);
		}

		GetPoints(size);
		DestoryParticleSystem();
		RemovePatternFromAll();
	}

	public void ForgetPattern() {
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

	void GetPoints(int smashed) {

		if (smashed == 3) {
			PointsManager.PM.Gain(3);
		} else if (smashed == 4) {
			PointsManager.PM.Gain(6);
		} else if (smashed == 5) {
			PointsManager.PM.Gain(15);
		}

	}

	void OrientParticleSystem () {

		if (particleSystem == null)
			particleSystem = (MonoBehaviour.Instantiate(particlePrefab) as GameObject).GetComponent<ParticleSystem>();

		Vector3 box = Vector3.zero;
		box.x = Smashee.width;
		box.z = Smashee.height;

		if (orientation == Orientation.Horizontal) box.x *= size;
		if (orientation == Orientation.Vertical) box.z *= size;

		ParticleSystem.ShapeModule shape = particleSystem.shape;
		shape.box = box;
		 
		Vector3 pos = Vector3.zero;
		if (size % 2 != 0) {
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

		if (orientation == Orientation.Horizontal) {
			return Mathf.Abs(one.column - two.column);

		} else if (orientation == Orientation.Vertical) {
			return Mathf.Abs(one.row - two.row);

		} else {
			return -1;
		}

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

	public override string ToString () {

		string str = "";
		foreach (Smashee smashee in list)
			str += smashee + " ";
		str += orientation;
		return str;		
	
	}

}
