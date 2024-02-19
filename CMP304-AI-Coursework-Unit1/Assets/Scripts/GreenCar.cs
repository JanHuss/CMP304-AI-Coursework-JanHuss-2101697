using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FLS;
using FLS.Rules;
using FLS.MembershipFunctions;

public class GreenCar : MonoBehaviour
{
    // Variables
    public GameObject aIGuideline;
    public float greenCarStartPosX;
    public float greenCarSpeed;
    private Vector3 guideLinePos;
    
    // Fuzzy Logic Variables
    // fuzzy input
    private LinguisticVariable distance;
    // fuzzy output
    private LinguisticVariable direction;

    private IFuzzyEngine engine;
    
    // Start is called before the first frame update
    void Start()
    {
        // setup fuzzy inference system
        distance = new LinguisticVariable("distance");
        direction = new LinguisticVariable("direction");

        engine = new FuzzyEngineFactory().Default();
        
        // setting linguistic variables and their curvature
        // input for distance
        var toLeft = distance.MembershipFunctions.AddTriangle("toLeft", -0.47f, -0.3f, -0.05f);
        var noDist = distance.MembershipFunctions.AddTriangle("noDist", -0.2f, 0.0f, 0.2f);
        var toRight = distance.MembershipFunctions.AddTriangle("toRight", 0.05f, 0.3f, 0.47f);
        
        // output for direction
        var goLeft = direction.MembershipFunctions.AddTriangle("goLeft", -0.47f, -0.15f, 0.2f);
        var isCentred = direction.MembershipFunctions.AddTriangle("isCentred", -0.3f, 0.0f, 0.25f);
        var goRight = direction.MembershipFunctions.AddTriangle("goRight", -0.2f, 0.15f, 0.47f);
        
        // setting up fuzzy logic rules
        var ruleOne = Rule.If(distance.Is(toLeft)).Then(direction.Is(goRight));
        var ruleTwo = Rule.If(distance.Is(noDist)).Then(direction.Is(isCentred));
        var ruleThree = Rule.If(distance.Is(toRight)).Then(direction.Is(goLeft));
        
        // add rules to fuzzy engine 
        engine.Rules.Add(ruleOne, ruleTwo, ruleThree);
    }

    private void FixedUpdate()
    {
        // defuzzify values into precise values
        // distance: the cars position minus the blue line position
        double result = engine.Defuzzify(new
            { distance = (double)this.transform.position.x - aIGuideline.transform.position.x });

        // debug lines
        //Debug.Log("Result of distance: " + result);

        // setting the results of the fuzzy logic to the car's rigidbody2d and apply force on the X-Axis
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(new Vector3((float)(result), 0.0f, 0.0f));
    }
}
