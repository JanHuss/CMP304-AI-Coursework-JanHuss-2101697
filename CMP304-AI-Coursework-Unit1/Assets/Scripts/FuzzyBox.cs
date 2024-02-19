using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FLS;
using FLS.Rules;
using FLS.MembershipFunctions;

public class FuzzyBox : MonoBehaviour {

	bool selected = false;
	private IFuzzyEngine engine;
	private LinguisticVariable distance;
	private LinguisticVariable direction;
	private LinguisticVariable speed;

	void Start()
	{
		// Here we need to setup the Fuzzy Inference System
		distance = new LinguisticVariable("distance");
		direction = new LinguisticVariable("direction");
		speed = new LinguisticVariable("speed");
		
		engine = new FuzzyEngineFactory().Default();

		// setting linguistic variables 
		// input : distance
		var right = distance.MembershipFunctions.AddTrapezoid("right", -50, -50, -5, -1);
		var none = distance.MembershipFunctions.AddTrapezoid("none", -5, -0.5, 0.5, 5);
		var left = distance.MembershipFunctions.AddTrapezoid("left", 1, 5, 50, 50);
		var farRight = distance.MembershipFunctions.AddTrapezoid("farRight", -70, -65, -8, -2);
		var farLeft = distance.MembershipFunctions.AddTrapezoid("farLeft", 2, 6, 60, 60);
		
		// input : speed
		var fast = speed.MembershipFunctions.AddTriangle("fast", 1.0, 20.0, 60.0);
		var slow = speed.MembershipFunctions.AddTriangle("slow", -60.0, -20.0, -1.0);
		var stationary = speed.MembershipFunctions.AddTriangle("stationary", -30.0, 0.0, 30.0);
		
		// output
		var directionRight = direction.MembershipFunctions.AddTrapezoid("directionRight", -40, -50, -5, -1);
		var directionNone = direction.MembershipFunctions.AddTrapezoid("directionNone", -5, -0.5, 0.5, 5);
		var directionLeft = direction.MembershipFunctions.AddTrapezoid("directionLeft", 1, 5, 40, 50);
		var directionFarRight = direction.MembershipFunctions.AddTrapezoid("directionFarRight", -70, -65, -8, -2);
		var directionFarLeft = direction.MembershipFunctions.AddTrapezoid("directionFarLeft", 2, 6, 60, 60);
		
		// setting up the rules
		var rule1 = Rule.If(distance.Is(right)).Then(direction.Is(directionLeft));
		var rule2 = Rule.If(distance.Is(left)).Then(direction.Is(directionRight));
		var rule3 = Rule.If(distance.Is(none)).Then(direction.Is(directionNone));
		var rule4 = Rule.If(distance.Is(farRight)).Then(direction.Is(directionFarLeft));
		var rule5 = Rule.If(distance.Is(farLeft)).Then(direction.Is(directionFarRight));
		var rule6 = Rule.If(distance.Is(right)).Then(speed.Is(slow));
		//var rule7 = Rule.If(distance.Is(left)).Then(speed.Is(slow));
		//var rule8 = Rule.If(distance.Is(none)).Then(speed.Is(stationary));
		//var rule9 = Rule.If(distance.Is(farRight)).Then(speed.Is(fast));
		//var rule10 = Rule.If(distance.Is(farLeft)).Then(speed.Is(fast));
		
		// add rules to the fuzzy logic engine
		engine.Rules.Add(rule1, rule2, rule3, rule4, rule5
									/*rule6, rule7, rule8, rule9, rule10*/);
	}

	void FixedUpdate()
	{
		if(!selected && this.transform.position.y < 0.6f)
		{
			Vector3 centre = new Vector3(0, 0, 0);
			// Convert position of box to value between 0 and 100
			double result = engine.Defuzzify(new { distance = (double)this.transform.position.x, speed = (double) Vector3.Distance(this.transform.position, centre) / Time.deltaTime});
			//double velocityResult = engine.Defuzzify(new { speed });
			Debug.Log("Result: " + result);

			Rigidbody rigidbody = GetComponent<Rigidbody>();
			rigidbody.AddForce(new Vector3((float)(result/* * velocity*/), 0f, 0f));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			var hit = new RaycastHit();
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)){
				if (hit.transform.name == "FuzzyBox" )Debug.Log( "You have clicked the FuzzyBox");
				selected = true;
			}
		}

		if(Input.GetMouseButton(0) && selected)
		{
			float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
			transform.position = new Vector3(curPosition.x, Mathf.Max(0.5f, curPosition.y), transform.position.z);
		}

		if(Input.GetMouseButtonUp(0))
		{
			selected = false;
		}
	}
}
