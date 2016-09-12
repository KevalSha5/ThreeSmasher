using UnityEngine;
using System.Collections;

public class DebugCode : MonoBehaviour {

	void Start () {

		AddingPatternFlare();

	}
	
	void Update () {
	
	}

	void AddingPatternFlare() {

		SmasheeGenerator SG = SmasheeGenerator.SG;

		SG.generate = false;
		SG.NewSmashee(0, 0);
		SG.NewSmashee(0, 0);
		SG.NewSmashee(0, 0);
		SG.NewSmashee(0, 0);
		SG.NewSmashee(1, 0);
		SG.NewSmashee(2, 0);
		SG.NewSmashee(3, 0);

	}
}
