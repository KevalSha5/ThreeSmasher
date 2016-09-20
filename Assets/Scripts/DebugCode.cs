using UnityEngine;
using System.Collections;

public class DebugCode : MonoBehaviour {

	SmasheeGenerator SG;

	void Start () {

		SG = SmasheeGenerator.SG;

//		AddingPatternFlare();
//		AddingPatternHighlighter();
		Nothing();

	}
	
	void Update () {
	
	}

	void Nothing () {

		SG.generate = false;

	}

	void AddingPatternHighlighter () {

		SG.generate = false;
		SG.NewSmashee(0, 0);
		SG.NewSmashee(0, 0);
		SG.NewSmashee(0, 0);
		SG.NewSmashee(0, 0);
		SG.NewSmashee(1, 0);
		SG.NewSmashee(2, 1);
		SG.NewSmashee(3, 2);
		SG.NewSmashee(1, 0);
		SG.NewSmashee(2, 1);
		SG.NewSmashee(3, 2);
		SG.NewSmashee(1, 0);
		SG.NewSmashee(2, 1);
		SG.NewSmashee(3, 2);

	}

	void AddingPatternFlare () {


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
