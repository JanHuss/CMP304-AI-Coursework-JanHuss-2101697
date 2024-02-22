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
    public GameObject redCar;
    public GameObject redCarManager;
    public GameObject itemManager;
    public float greenCarStartPosX;
    public float greenCarSpeed;
    private Vector3 guideLinePos;
    
    // Fuzzy Logic Variables
    // fuzzy input
    private LinguisticVariable redCarDistance;
    private LinguisticVariable redCarApproach;
    private LinguisticVariable itemDistance;
    // fuzzy output
    private LinguisticVariable direction;

    private IFuzzyEngine engine;
    
    // Start is called before the first frame update
    void Start()
    {
        // setup fuzzy inference system
        redCarDistance = new LinguisticVariable("redCarDistance");
        redCarApproach = new LinguisticVariable("redCarApproach");
        itemDistance = new LinguisticVariable("itemDistance");
        direction = new LinguisticVariable("direction");

        engine = new FuzzyEngineFactory().Default();
        
        // Input: distance to closest red car
        var toLeft = redCarDistance.MembershipFunctions.AddTriangle("toLeft", -0.47f, -0.3f, -0.05f);
        var toRight = redCarDistance.MembershipFunctions.AddTriangle("toRight", 0.05f, 0.3f, 0.47f);
        var offRoadLeft = redCarDistance.MembershipFunctions.AddTriangle("offRoadLeft", -2.05f, -0.48f, -0.42f);
        var offRoadRight = redCarDistance.MembershipFunctions.AddTriangle("offRoadRight", 0.42f, 0.48f, 2.05f);
        
        // Input: distance to approaching red car
        var far = redCarApproach.MembershipFunctions.AddTriangle("far", -0.47f, -0.3f, -0.05f);
        var close = redCarApproach.MembershipFunctions.AddTriangle("close", 0.05f, 0.3f, 0.47f);
        
        // Input: distance to closest item
        var cToLeft = itemDistance.MembershipFunctions.AddTriangle("item", -0.47f, -0.3f, -0.05f);
        var cNoDist = itemDistance.MembershipFunctions.AddTriangle("itemNoDist", -0.2f, 0.0f, 0.2f);
        var cToRight = itemDistance.MembershipFunctions.AddTriangle("itemToRight", 0.05f, 0.3f, 0.47f);
        
        // Output: how the green car should react
        //var goLeft = direction.MembershipFunctions.AddTriangle("goLeft", -0.47f, -0.15f, 0.2f);
        ////var isCentred = direction.MembershipFunctions.AddTriangle("isCentred", -0.3f, 0.0f, 0.3f);
        //var goRight = direction.MembershipFunctions.AddTriangle("goRight", -0.2f, 0.15f, 0.47f);
        
        var goLeft = direction.MembershipFunctions.AddTriangle("goLeft", -0.47f, -0.3f, 0.1f);
        var isCentred = direction.MembershipFunctions.AddTriangle("isCentred", -0.3f, 0.0f, 0.3f);
        var goRight = direction.MembershipFunctions.AddTriangle("goRight", -0.1f, 0.3f, 0.47f);
        
        // Rules: green car avoiding red cars
        var ruleOne = Rule.If(redCarDistance.Is(toLeft)).Then(direction.Is(goRight));
        var ruleTwo = Rule.If(redCarDistance.Is(toRight)).Then(direction.Is(goLeft));
        var ruleThree = Rule.If(redCarDistance.Is(offRoadLeft)).Then(direction.Is(goRight));
        var ruleFour = Rule.If(redCarDistance.Is(offRoadRight)).Then(direction.Is(goLeft));
        // Rules: green car approaching red car
        var ruleEight = Rule.If(redCarApproach.Is(far)).Then(direction.Is(isCentred));
        var ruleNine = Rule.If(redCarApproach.Is(close).And(redCarDistance.Is(toLeft))).Then(direction.Is(goRight));
        var ruleTen = Rule.If(redCarApproach.Is(close).And(redCarDistance.Is(toRight))).Then(direction.Is(goLeft));
        // Rules: green car approaching items
        var ruleFive = Rule.If(itemDistance.Is(cToLeft)).Then(direction.Is(goRight));
        var ruleSix = Rule.If(itemDistance.Is(cNoDist)).Then(direction.Is(isCentred));
        var ruleSeven = Rule.If(itemDistance.Is(cToRight)).Then(direction.Is(goLeft));
        
        
        // add rules to fuzzy engine 
        engine.Rules.Add(ruleOne, ruleTwo, ruleThree, ruleFour, ruleFive, 
                                    ruleSix, ruleSeven, ruleEight, ruleNine, ruleTen);
    }

    private void FixedUpdate()
    {
        // defuzzify values into precise values
        // redistance: the cars position minus the blue line position
        // item: 
        double result = engine.Defuzzify(new
            { redCarDistance = (double)this.transform.position.x + redCarManager.GetComponent<RedCarManager>().greenCarLookAtLane,
                    itemDistance = (double)this.transform.position.x - itemManager.GetComponent<ItemManager>().greenCarLookAtCoin,
                    redCarApproach = (double)this.transform.position.y + redCarManager.GetComponent<RedCarManager>().greenCarLookAtLane
            /*redCarDistance = (double)this.transform.position.x - aIGuideline.transform.position.x }*/});

        // debug lines
        //Debug.Log("Result of Red car distance: " + result);
        //Debug.Log("Result of Blue line distance: " + result);

        // setting the results of the fuzzy logic to the car's rigidbody2d and apply force on the X-Axis
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(new Vector3((float)(result), 0.0f, 0.0f));
    }
}
