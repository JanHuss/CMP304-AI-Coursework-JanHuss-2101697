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
    public bool fourteenRules;
    public bool twentytwoRules;
    public bool HundredAndTwentyRules;
    public bool addCar;
    public bool addItem;
    private float greenCarYPos;
    
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
        greenCarYPos = -1.03f;
        
        // setup fuzzy inference system
        redCarDistance = new LinguisticVariable("redCarDistance");
        redCarApproach = new LinguisticVariable("redCarApproach");
        itemDistance = new LinguisticVariable("itemDistance");
        itemApproach = new LinguisticVariable("itemApproach");
        direction = new LinguisticVariable("direction");

        engine = new FuzzyEngineFactory().Default();
        
        // Inputs
        #region PhaseOneInputs
            //// Input: distance to closest red car
            //var toLeft = redCarDistance.MembershipFunctions.AddTriangle("toLeft", -0.47f, -0.3f, 0.05f);
            //var toRight = redCarDistance.MembershipFunctions.AddTriangle("toRight", -0.05f, 0.3f, 0.47f);
            //var offRoadLeft = redCarDistance.MembershipFunctions.AddTriangle("offRoadLeft", -2.05f, -0.48f, -0.42f);
            //var offRoadRight = redCarDistance.MembershipFunctions.AddTriangle("offRoadRight", 0.42f, 0.48f, 2.05f);
            //
            //// Input: distance to approaching red car
            //var far = redCarApproach.MembershipFunctions.AddTriangle("far", -0.47f, -0.3f, 0.01f);
            //var close = redCarApproach.MembershipFunctions.AddTriangle("close", -0.01f, 0.3f, 0.47f);
            //
            //// Input: distance to closest item
            //var cToLeft = itemDistance.MembershipFunctions.AddTriangle("itemToLeft", -0.47f, -0.3f, -0.05f);
            //var cNoDist = itemDistance.MembershipFunctions.AddTriangle("itemNoDist", -0.2f, 0.0f, 0.2f);
            //var cToRight = itemDistance.MembershipFunctions.AddTriangle("itemToRight", 0.05f, 0.3f, 0.47f);
            //var itemOffRoadLeft = itemDistance.MembershipFunctions.AddTriangle("itemOffRoadLeft", -2.05f, -0.48f, -0.42f);
            //var itemOffRoadRight = itemDistance.MembershipFunctions.AddTriangle("itemOffRoadRight", 0.42f, 0.48f, 2.05f);
            //
            //// Input: distance to approaching item
            //var itemFar = redCarApproach.MembershipFunctions.AddTriangle("itemFar", -0.47f, -0.3f, 0.05f);
            //var itemClose = redCarApproach.MembershipFunctions.AddTriangle("itemClose", -0.05f, 0.3f, 0.47f);

        #endregion
        #region PhaseTwoInputs
        // Input: distance to closest red car
        var toLeft = redCarDistance.MembershipFunctions.AddTriangle("toLeft", -0.47f, -0.3f, 0.05f);
        var toRight = redCarDistance.MembershipFunctions.AddTriangle("toRight", -0.05f, 0.3f, 0.47f);
        var offRoadLeft = redCarDistance.MembershipFunctions.AddTriangle("offRoadLeft", -2.05f, -0.48f, -0.42f);
        var offRoadRight = redCarDistance.MembershipFunctions.AddTriangle("offRoadRight", 0.42f, 0.48f, 2.05f);
            
        // Input: distance to approaching red car
        var far = redCarApproach.MembershipFunctions.AddTriangle("far", -0.47f, -0.3f, 0.01f);
        var close = redCarApproach.MembershipFunctions.AddTriangle("close", -0.01f, 0.3f, 0.47f);
            
        // Input: distance to closest item
        var cToLeft = itemDistance.MembershipFunctions.AddTriangle("itemToLeft", -0.47f, -0.3f, -0.05f);
        var cNoDist = itemDistance.MembershipFunctions.AddTriangle("itemNoDist", -0.2f, 0.0f, 0.2f);
        var cToRight = itemDistance.MembershipFunctions.AddTriangle("itemToRight", 0.05f, 0.3f, 0.47f);
        var itemOffRoadLeft = itemDistance.MembershipFunctions.AddTriangle("itemOffRoadLeft", -2.05f, -0.48f, -0.42f);
        var itemOffRoadRight = itemDistance.MembershipFunctions.AddTriangle("itemOffRoadRight", 0.42f, 0.48f, 2.05f);
            
        // Input: distance to approaching item
        var itemFar = redCarApproach.MembershipFunctions.AddTriangle("itemFar", -0.47f, -0.3f, 0.05f);
        var itemClose = redCarApproach.MembershipFunctions.AddTriangle("itemClose", -0.05f, 0.3f, 0.47f);

        #endregion

        // Outputs
        #region PhaseOneAndTwoOutputs
            // Output: how the green car should react
            var goLeft = direction.MembershipFunctions.AddTriangle("goLeft", -0.47f, -0.3f, 0.1f);
            var isCentred = direction.MembershipFunctions.AddTriangle("isCentred", -0.3f, 0.0f, 0.3f);
            var goRight = direction.MembershipFunctions.AddTriangle("goRight", -0.1f, 0.3f, 0.47f);

        #endregion

        // Rules and Adding to engine
        #region 14 Rules

        // 14 Rules
           if (fourteenRules) {
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
        

        #endregion
        #region 22 Rules
           
       // 22 Rules
        else if (twentytwoRules) {
            var ruleOne = Rule.If(redCarDistance.Is(toRight).And(itemDistance.Is(cToRight)).And(redCarApproach.Is(close)).And(itemApproach.Is(itemFar))).Then(direction.Is(isCentred));
            var ruleTwo = Rule.If(redCarDistance.Is(toLeft).And(itemDistance.Is(cToLeft)).And(redCarApproach.Is(close)).And(itemApproach.Is(itemFar))).Then(direction.Is(isCentred));
            var ruleThree = Rule.If(redCarDistance.Is(toRight).And(itemDistance.Is(cToRight)).And(redCarApproach.Is(far)).And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleFour = Rule.If(redCarDistance.Is(toLeft).And(itemDistance.Is(cToLeft)).And(redCarApproach.Is(far)).And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleFive = Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(close))).Then(direction.Is(goLeft));
            var ruleSix = Rule.If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close))).Then(direction.Is(goRight));
            var ruleSeven = Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(far))).Then(direction.Is(isCentred));
            var ruleEight = Rule.If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far))).Then(direction.Is(isCentred));
            var ruleNine = Rule.If(itemDistance.Is(cToRight).And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleTen = Rule.If(itemDistance.Is(cToLeft).And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleEleven = Rule.If(itemDistance.Is(cToRight).And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleTwelve = Rule.If(itemDistance.Is(cToLeft).And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleThirteen = Rule.If(redCarDistance.Is(cNoDist).And(itemDistance.Is(cToRight))).Then(direction.Is(goRight));
            var ruleFourteen = Rule.If(redCarDistance.Is(cNoDist).And(itemDistance.Is(cToLeft))).Then(direction.Is(goLeft));
            var ruleFifteen = Rule.If(itemDistance.Is(cNoDist).And(redCarDistance.Is(toRight))).Then(direction.Is(isCentred));
            var ruleSixteen = Rule.If(itemDistance.Is(cNoDist).And(redCarDistance.Is(toLeft))).Then(direction.Is(isCentred));
            var ruleNineteen = Rule.If(redCarDistance.Is(offRoadRight)).Then(direction.Is(isCentred));
            var ruleTwenty = Rule.If(redCarDistance.Is(offRoadLeft)).Then(direction.Is(isCentred));
            var ruleTwentyOne = Rule.If(redCarDistance.Is(offRoadRight).And(itemDistance.Is(cToRight))).Then(direction.Is(goRight));
            var ruleTwentyTwo = Rule.If(redCarDistance.Is(offRoadLeft).And(itemDistance.Is(cToLeft))).Then(direction.Is(goLeft));
            
            // add rules to fuzzy engine 
            engine.Rules.Add(ruleOne, ruleTwo, ruleThree, ruleFour, ruleFive, 
                ruleSix, ruleSeven, ruleEight, ruleNine, ruleTen, ruleEleven, ruleTwelve,
                ruleThirteen, ruleFourteen, ruleFifteen, ruleSixteen, ruleNineteen, ruleTwenty,
                ruleTwentyOne, ruleTwentyTwo);
        }

        #endregion
        #region 120 Rules
        
        // 120 rules
        if (HundredAndTwentyRules)
        {
            var ruleOne = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleTwo = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleThree = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleFour = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleFive = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleSix = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleSeven = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleEight = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleNine = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleTen = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleEleven = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleTwelve = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleThirteen = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleFourteen = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleFifteen = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleSixteen = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleSeventeen =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleEighteen = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleNineteen = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleTwenty = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleTwentyOne =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleTwentyTwo =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleTwentyThree =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleTwentyFour =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleTwentyFive =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleTwentySix =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleTwentySeven =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleTwentyEight =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleTwentyNine =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleThirty = Rule
                .If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleThirtyOne =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleThirtyTwo =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleThirtyThree =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleThirtyFour =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleThirtyFive =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleThirtySix =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleThirtySeven =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleThirtyEight =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleThirtyNine =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleForty = Rule
                .If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleFortyOne = Rule
                .If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleFortyTwo = Rule
                .If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleFortyThree =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleFortyFour =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cToLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleFortyFive =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleFortySix = Rule
                .If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cNoDist))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleFortySeven =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleFortyEight =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close)).And(itemDistance.Is(cToRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleFortyNine =
                Rule.If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleFifty = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleFiftyOne = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleFiftyTwo = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleFiftyThree =
                Rule.If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleFiftyFour =
                Rule.If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleFiftyFive =
                Rule.If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleFiftySix = Rule
                .If(redCarDistance.Is(toLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleFiftySeven =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleFiftyEight =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleFiftyNine =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleSixty = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleSixtyOne = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleSixtyTwo = Rule
                .If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleSixtyThree =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleSixtyFour =
                Rule.If(redCarDistance.Is(toRight).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleSixtyFive =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleSixtySix = Rule
                .If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleSixtySeven =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far)).And(itemDistance.Is(itemOffRoadLeft))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleSixtyEight =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(far))
                    .And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleSixtyNine =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close))
                    .And(itemDistance.Is(itemOffRoadLeft)).And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleSeventy = Rule
                .If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemFar))).Then(direction.Is(goRight));
            var ruleSeventyOne =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close))
                    .And(itemDistance.Is(itemOffRoadLeft)).And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleSeventyTwo =
                Rule.If(redCarDistance.Is(offRoadLeft).And(redCarApproach.Is(close))
                    .And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goRight));
            var ruleSeventyThree =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far))
                    .And(itemDistance.Is(itemOffRoadLeft)).And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleSeventyFour =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far))
                    .And(itemDistance.Is(itemOffRoadRight)).And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleSeventyFive =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far))
                    .And(itemDistance.Is(itemOffRoadLeft)).And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleSeventySix =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(far))
                    .And(itemDistance.Is(itemOffRoadRight)).And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleSeventySeven =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close))
                    .And(itemDistance.Is(itemOffRoadLeft)).And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleSeventyEight =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close))
                    .And(itemDistance.Is(itemOffRoadRight)).And(itemApproach.Is(itemFar))).Then(direction.Is(goLeft));
            var ruleSeventyNine =
                Rule.If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close))
                    .And(itemDistance.Is(itemOffRoadLeft)).And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleEighty = Rule
                .If(redCarDistance.Is(offRoadRight).And(redCarApproach.Is(close)).And(itemDistance.Is(itemOffRoadRight))
                    .And(itemApproach.Is(itemClose))).Then(direction.Is(goLeft));
            var ruleEightyOne = Rule.If(redCarDistance.Is(toLeft).And(itemDistance.Is(cToLeft)))
                .Then(direction.Is(goRight));
            var ruleEightyTwo = Rule.If(redCarDistance.Is(toLeft).And(itemDistance.Is(cNoDist)))
                .Then(direction.Is(goRight));
            var ruleEightyThree = Rule.If(redCarDistance.Is(toLeft).And(itemDistance.Is(cToRight)))
                .Then(direction.Is(goRight));
            var ruleEightyFour = Rule.If(redCarDistance.Is(toLeft).And(itemDistance.Is(itemOffRoadLeft)))
                .Then(direction.Is(goRight));
            var ruleEightyFive = Rule.If(redCarDistance.Is(toLeft).And(itemDistance.Is(itemOffRoadRight)))
                .Then(direction.Is(goRight));
            var ruleEightySix = Rule.If(redCarDistance.Is(toRight).And(itemDistance.Is(cToLeft)))
                .Then(direction.Is(goLeft));
            var ruleEightySeven = Rule.If(redCarDistance.Is(toRight).And(itemDistance.Is(cNoDist)))
                .Then(direction.Is(goLeft));
            var ruleEightyEight = Rule.If(redCarDistance.Is(toRight).And(itemDistance.Is(cToRight)))
                .Then(direction.Is(goLeft));
            var ruleEightyNine = Rule.If(redCarDistance.Is(toRight).And(itemDistance.Is(itemOffRoadLeft)))
                .Then(direction.Is(goLeft));
            var ruleNinety = Rule.If(redCarDistance.Is(toRight).And(itemDistance.Is(itemOffRoadRight)))
                .Then(direction.Is(goLeft));
            var ruleNinetyOne = Rule.If(redCarDistance.Is(offRoadLeft).And(itemDistance.Is(cToLeft)))
                .Then(direction.Is(goRight));
            var ruleNinetyTwo = Rule.If(redCarDistance.Is(offRoadLeft).And(itemDistance.Is(cNoDist)))
                .Then(direction.Is(goRight));
            var ruleNinetyThree = Rule.If(redCarDistance.Is(offRoadLeft).And(itemDistance.Is(cToRight)))
                .Then(direction.Is(goRight));
            var ruleNinetyFour = Rule.If(redCarDistance.Is(offRoadLeft).And(itemDistance.Is(itemOffRoadLeft)))
                .Then(direction.Is(goRight));
            var ruleNinetyFive = Rule.If(redCarDistance.Is(offRoadLeft).And(itemDistance.Is(itemOffRoadRight)))
                .Then(direction.Is(goRight));
            var ruleNinetySix = Rule.If(redCarDistance.Is(offRoadRight).And(itemDistance.Is(cToLeft)))
                .Then(direction.Is(goLeft));
            var ruleNinetySeven = Rule.If(redCarDistance.Is(offRoadRight).And(itemDistance.Is(cNoDist)))
                .Then(direction.Is(goLeft));
            var ruleNinetyEight = Rule.If(redCarDistance.Is(offRoadRight).And(itemDistance.Is(cToRight)))
                .Then(direction.Is(goLeft));
            var ruleNinetyNine = Rule.If(redCarDistance.Is(offRoadRight).And(itemDistance.Is(itemOffRoadLeft)))
                .Then(direction.Is(goLeft));
            var ruleOneHundred = Rule.If(redCarDistance.Is(offRoadRight).And(itemDistance.Is(itemOffRoadRight)))
                .Then(direction.Is(goLeft));
            var ruleOneHundredOne = Rule.If(itemDistance.Is(cToLeft).And(redCarApproach.Is(far)))
                .Then(direction.Is(goRight));
            var ruleOneHundredTwo = Rule.If(itemDistance.Is(cToRight).And(redCarApproach.Is(far)))
                .Then(direction.Is(goLeft));
            var ruleOneHundredThree = Rule.If(itemDistance.Is(cNoDist).And(redCarApproach.Is(far)))
                .Then(direction.Is(isCentred));
            var ruleOneHundredFour = Rule.If(itemDistance.Is(itemOffRoadLeft).And(redCarApproach.Is(far)))
                .Then(direction.Is(goRight));
            var ruleOneHundredFive = Rule.If(itemDistance.Is(itemOffRoadRight).And(redCarApproach.Is(far)))
                .Then(direction.Is(goLeft));
            var ruleOneHundredSix = Rule.If(itemDistance.Is(cToLeft).And(redCarApproach.Is(close)))
                .Then(direction.Is(goRight));
            var ruleOneHundredSeven = Rule.If(itemDistance.Is(cToRight).And(redCarApproach.Is(close)))
                .Then(direction.Is(goLeft));
            var ruleOneHundredEight = Rule.If(itemDistance.Is(cNoDist).And(redCarApproach.Is(close)))
                .Then(direction.Is(isCentred));
            var ruleOneHundredNine = Rule.If(itemDistance.Is(itemOffRoadLeft).And(redCarApproach.Is(close)))
                .Then(direction.Is(goRight));
            var ruleOneHundredTen = Rule.If(itemDistance.Is(itemOffRoadRight).And(redCarApproach.Is(close)))
                .Then(direction.Is(goLeft));
            var ruleOneHundredEleven = Rule.If(redCarDistance.Is(toLeft).And(itemApproach.Is(itemFar)))
                .Then(direction.Is(goRight));
            var ruleOneHundredTwelve = Rule.If(redCarDistance.Is(toRight).And(itemApproach.Is(itemFar)))
                .Then(direction.Is(goLeft));
            var ruleOneHundredThirteen = Rule.If(redCarDistance.Is(offRoadLeft).And(itemApproach.Is(itemFar)))
                .Then(direction.Is(goRight));
            var ruleOneHundredFourteen = Rule.If(redCarDistance.Is(offRoadRight).And(itemApproach.Is(itemFar)))
                .Then(direction.Is(goLeft));
            var ruleOneHundredFifteen = Rule.If(redCarDistance.Is(toLeft).And(itemApproach.Is(itemClose)))
                .Then(direction.Is(goRight));
            var ruleOneHundredSixteen = Rule.If(redCarDistance.Is(toRight).And(itemApproach.Is(itemClose)))
                .Then(direction.Is(goLeft));
            var ruleOneHundredSeventeen = Rule.If(redCarDistance.Is(offRoadLeft).And(itemApproach.Is(itemClose)))
                .Then(direction.Is(goRight));
            var ruleOneHundredEighteen = Rule.If(redCarDistance.Is(offRoadRight).And(itemApproach.Is(itemClose)))
                .Then(direction.Is(goLeft));
            var ruleOneHundredNineteen = Rule.If(itemDistance.Is(cToLeft).And(itemApproach.Is(itemFar)))
                .Then(direction.Is(goRight));
            var ruleOneHundredTwenty = Rule.If(itemDistance.Is(cToRight).And(itemApproach.Is(itemFar)))
                .Then(direction.Is(goLeft));

            engine.Rules.Add(ruleOne, ruleTwo, ruleThree, ruleFour, ruleFive, ruleSix, ruleSeven, ruleEight, ruleNine,
                ruleTen,
                ruleEleven, ruleTwelve, ruleThirteen, ruleFourteen, ruleFifteen, ruleSixteen, ruleSeventeen,
                ruleEighteen, ruleNineteen, ruleTwenty,
                ruleTwentyOne, ruleTwentyTwo, ruleTwentyThree, ruleTwentyFour, ruleTwentyFive, ruleTwentySix,
                ruleTwentySeven, ruleTwentyEight, ruleTwentyNine, ruleThirty,
                ruleThirtyOne, ruleThirtyTwo, ruleThirtyThree, ruleThirtyFour, ruleThirtyFive, ruleThirtySix,
                ruleThirtySeven, ruleThirtyEight, ruleThirtyNine, ruleForty,
                ruleFortyOne, ruleFortyTwo, ruleFortyThree, ruleFortyFour, ruleFortyFive, ruleFortySix, ruleFortySeven,
                ruleFortyEight, ruleFortyNine, ruleFifty,
                ruleFiftyOne, ruleFiftyTwo, ruleFiftyThree, ruleFiftyFour, ruleFiftyFive, ruleFiftySix, ruleFiftySeven,
                ruleFiftyEight, ruleFiftyNine, ruleSixty,
                ruleSixtyOne, ruleSixtyTwo, ruleSixtyThree, ruleSixtyFour, ruleSixtyFive, ruleSixtySix, ruleSixtySeven,
                ruleSixtyEight, ruleSixtyNine, ruleSeventy,
                ruleSeventyOne, ruleSeventyTwo, ruleSeventyThree, ruleSeventyFour, ruleSeventyFive, ruleSeventySix,
                ruleSeventySeven, ruleSeventyEight, ruleSeventyNine, ruleEighty,
                ruleEightyOne, ruleEightyTwo, ruleEightyThree, ruleEightyFour, ruleEightyFive, ruleEightySix,
                ruleEightySeven, ruleEightyEight, ruleEightyNine, ruleNinety,
                ruleNinetyOne, ruleNinetyTwo, ruleNinetyThree, ruleNinetyFour, ruleNinetyFive, ruleNinetySix,
                ruleNinetySeven, ruleNinetyEight, ruleNinetyNine, ruleOneHundred,
                ruleOneHundredOne, ruleOneHundredTwo, ruleOneHundredThree, ruleOneHundredFour, ruleOneHundredFive,
                ruleOneHundredSix,
                ruleOneHundredSeven, ruleOneHundredEight, ruleOneHundredNine, ruleOneHundredTen,
                ruleOneHundredEleven, ruleOneHundredTwelve, ruleOneHundredThirteen, ruleOneHundredFourteen,
                ruleOneHundredFifteen,
                ruleOneHundredSixteen, ruleOneHundredSeventeen, ruleOneHundredEighteen, ruleOneHundredNineteen,
                ruleOneHundredTwenty);
        }

        #endregion
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
        else if (!addCar && addItem) {
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