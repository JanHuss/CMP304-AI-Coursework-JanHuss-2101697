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
    public bool twelveRules;
    public bool twentyFourRules;
    public bool addCar;
    public bool addItem;
    
    // Fuzzy Logic Variables
    // fuzzy input
    private LinguisticVariable redCarDistance;
    private LinguisticVariable redCarApproach;
    private LinguisticVariable itemDistance;

    private LinguisticVariable itemApproach;
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
        itemApproach = new LinguisticVariable("itemApproach");
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
        var cToLeft = itemDistance.MembershipFunctions.AddTriangle("itemToLeft", -0.47f, -0.3f, -0.05f);
        var cNoDist = itemDistance.MembershipFunctions.AddTriangle("itemNoDist", -0.2f, 0.0f, 0.2f);
        var cToRight = itemDistance.MembershipFunctions.AddTriangle("itemToRight", 0.05f, 0.3f, 0.47f);
        var itemOffRoadLeft = itemDistance.MembershipFunctions.AddTriangle("itemOffRoadLeft", -2.05f, -0.48f, -0.42f);
        var itemOffRoadRight = itemDistance.MembershipFunctions.AddTriangle("itemOffRoadRight", 0.42f, 0.48f, 2.05f);
        
        // Input: distance to approaching item
        var itemFar = redCarApproach.MembershipFunctions.AddTriangle("itemFar", -0.47f, -0.3f, -0.05f);
        var itemClose = redCarApproach.MembershipFunctions.AddTriangle("itemClose", 0.05f, 0.3f, 0.47f);
        
        // if the red car approach is close and the item approach is far and the red car distance is to the left, then stay centred
        // if the red car is 
        
        // Output: how the green car should react
        //var goLeft = direction.MembershipFunctions.AddTriangle("goLeft", -0.47f, -0.15f, 0.2f);
        ////var isCentred = direction.MembershipFunctions.AddTriangle("isCentred", -0.3f, 0.0f, 0.3f);
        //var goRight = direction.MembershipFunctions.AddTriangle("goRight", -0.2f, 0.15f, 0.47f);
        
        var goLeft = direction.MembershipFunctions.AddTriangle("goLeft", -0.47f, -0.3f, 0.1f);
        var isCentred = direction.MembershipFunctions.AddTriangle("isCentred", -0.3f, 0.0f, 0.3f);
        var goRight = direction.MembershipFunctions.AddTriangle("goRight", -0.1f, 0.3f, 0.47f);

        if (twelveRules) {
            // Rules: green car avoiding red cars
            var ruleOne = Rule.If(redCarDistance.Is(toLeft)).Then(direction.Is(goRight));
            var ruleTwo = Rule.If(redCarDistance.Is(toRight)).Then(direction.Is(goLeft));
            var ruleThree = Rule.If(redCarDistance.Is(offRoadLeft)).Then(direction.Is(goRight));
            var ruleFour = Rule.If(redCarDistance.Is(offRoadRight)).Then(direction.Is(goLeft));
            // Rules: green car approaching red car
            //var ruleEight = Rule.If(redCarApproach.Is(far)).Then(direction.Is(isCentred));
            var ruleNine = Rule.If(redCarApproach.Is(close).And(redCarDistance.Is(toLeft))).Then(direction.Is(goRight));
            var ruleTen = Rule.If(redCarApproach.Is(close).And(redCarDistance.Is(toRight))).Then(direction.Is(goLeft));
            // Rules: green car going to items
            var ruleFive = Rule.If(itemDistance.Is(cToLeft)).Then(direction.Is(goRight));
            var ruleSix = Rule.If(itemDistance.Is(cNoDist)).Then(direction.Is(isCentred));
            var ruleSeven = Rule.If(itemDistance.Is(cToRight)).Then(direction.Is(goLeft));
            var ruleThirteen = Rule.If(itemDistance.Is(itemOffRoadLeft)).Then(direction.Is(goRight));
            var ruleFourteen = Rule.If(itemDistance.Is(itemOffRoadRight)).Then(direction.Is(goLeft));
            
            //Rules: green car approaching items
            var ruleEleven = Rule.If(itemApproach.Is(itemFar).And(itemDistance.Is(cToLeft))).Then(direction.Is(goLeft));
            var ruleTwelve = Rule.If(itemApproach.Is(itemFar).And(itemDistance.Is(cToRight))).Then(direction.Is(goRight));
            
            
            // add rules to fuzzy engine 
            if (addCar && !addItem) {
                engine.Rules.Add(ruleOne, ruleTwo, ruleThree, ruleFour,
                    ruleNine, ruleTen);
            }
            else if (!addCar && addItem) {
                engine.Rules.Add(ruleFive, ruleSix, ruleSeven, ruleEleven, 
                    ruleTwelve, ruleThirteen, ruleFourteen);
            }
            else if (addCar && addItem) {
            engine.Rules.Add(ruleOne, ruleTwo, ruleThree, ruleFour, ruleFive, 
                ruleSix, ruleSeven, ruleNine, ruleTen, ruleEleven, ruleTwelve, ruleThirteen, ruleFourteen);
            }
        }
        //else if (twentyFourRules) {
        //    var ruleOne = Rule.If(redCarDistance.Is(toRight).And(itemDistance.Is(cToRight)).And(redCarApproach.Is(close)).And(itemApproach.Is(itemFar))).Then(direction.Is(isCentred));
        //    var ruleTwo = Rule.If(redCarDistance.Is(toLeft).And(itemDistance.Is(cToLeft)).And(redCarApproach.Is(close)).And(itemApproach.Is(itemFar))).Then(direction.Is(isCentred));
        //    var ruleThree = Rule.If(redCarDistance.Is(toRight).And(itemDistance.Is(cToRight)).And(redCarApproach.Is(far)).And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
        //    var ruleFour = Rule.If(redCarDistance.Is(toLeft).And(itemDistance.Is(cToLeft)).And(redCarApproach.Is(far)).And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
        //    var ruleFive = Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(close))).Then(direction.Is(goLeft));
        //    var ruleSix = Rule.If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close))).Then(direction.Is(goRight));
        //    var ruleSeven = Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(far))).Then(direction.Is(isCentred));
        //    var ruleEight = Rule.If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far))).Then(direction.Is(isCentred));
        //    var ruleNine = Rule.If(itemDistance.Is(cToRight).And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
        //    var ruleTen = Rule.If(itemDistance.Is(cToLeft).And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
        //    var ruleEleven = Rule.If(itemDistance.Is(cToRight).And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
        //    var ruleTwelve = Rule.If(itemDistance.Is(cToLeft).And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
        //    var ruleThirteen = Rule.If(redCarDistance.Is(cNoDist).And(itemDistance.Is(cToRight))).Then(direction.Is(goRight));
        //    var ruleFourteen = Rule.If(redCarDistance.Is(cNoDist).And(itemDistance.Is(cToLeft))).Then(direction.Is(goLeft));
        //    var ruleFifteen = Rule.If(itemDistance.Is(cNoDist).And(redCarDistance.Is(toRight))).Then(direction.Is(isCentred));
        //    var ruleSixteen = Rule.If(itemDistance.Is(cNoDist).And(redCarDistance.Is(toLeft))).Then(direction.Is(isCentred));
        //    var ruleNineteen = Rule.If(redCarDistance.Is(offRoadRight)).Then(direction.Is(isCentred));
        //    var ruleTwenty = Rule.If(redCarDistance.Is(offRoadLeft)).Then(direction.Is(isCentred));
        //    var ruleTwentyOne = Rule.If(redCarDistance.Is(offRoadRight).And(itemDistance.Is(cToRight))).Then(direction.Is(goRight));
        //    var ruleTwentyTwo = Rule.If(redCarDistance.Is(offRoadLeft).And(itemDistance.Is(cToLeft))).Then(direction.Is(goLeft));
        //    
        //    // add rules to fuzzy engine 
        //    engine.Rules.Add(ruleOne, ruleTwo, ruleThree, ruleFour, ruleFive, 
        //        ruleSix, ruleSeven, ruleEight, ruleNine, ruleTen, ruleEleven, ruleTwelve,
        //        ruleThirteen, ruleFourteen, ruleFifteen, ruleSixteen, ruleNineteen, ruleTwenty,
        //        ruleTwentyOne, ruleTwentyTwo);
        //}
    }

    private void FixedUpdate()
    {
        if (addCar && !addItem) {
            Vector2 redCarPos = GameObject.FindGameObjectWithTag("RedCar").transform.position;
            // defuzzify values into precise values
             double result = engine.Defuzzify(new
            { redCarDistance = (double)this.transform.position.x + redCarPos.x,
                redCarApproach = (double)this.transform.position.y + redCarPos.y });
            // setting the results of the fuzzy logic to the car's rigidbody2d and apply force on the X-Axis
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(new Vector3((float)(result), 0.0f, 0.0f));
        }
        else if (addItem && !addCar) {
            Vector2 coinPos = GameObject.FindGameObjectWithTag("coin").transform.position;
            // defuzzify values into precise values
            double result = engine.Defuzzify(new
            { itemDistance = (double)this.transform.position.x - coinPos.x,
                itemApproach = (double)this.transform.position.y - coinPos.y });
            // setting the results of the fuzzy logic to the car's rigidbody2d and apply force on the X-Axis
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(new Vector3((float)(result), 0.0f, 0.0f));
        }
        else if (addCar && addItem) {
            Vector2 coinPos = GameObject.FindGameObjectWithTag("coin").transform.position;
            Vector2 redCarPos = GameObject.FindGameObjectWithTag("RedCar").transform.position;
            // defuzzify values into precise values
            double result = engine.Defuzzify(new
                { redCarDistance = (double)this.transform.position.x + redCarPos.x,
                        redCarApproach = (double)this.transform.position.y + redCarPos.y,
                        itemDistance = (double)this.transform.position.x - coinPos.x,
                        itemApproach = (double)this.transform.position.y - coinPos.y });
            // setting the results of the fuzzy logic to the car's rigidbody2d and apply force on the X-Axis
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(new Vector3((float)(result), 0.0f, 0.0f));
        }

        // debug lines
        //Debug.Log("Result of Red car distance: " + result);
        //Debug.Log("Result of Blue line distance: " + result);

        /*// setting the results of the fuzzy logic to the car's rigidbody2d and apply force on the X-Axis
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(new Vector3((float)(result), 0.0f, 0.0f));*/
    }
}
