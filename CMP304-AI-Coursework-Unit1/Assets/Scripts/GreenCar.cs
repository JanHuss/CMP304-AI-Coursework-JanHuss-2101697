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
        // set car position
        //transform.position = new Vector3(greenCarStartPosX, -0.2f, 0);
        
        // setup fuzzy inference system

        distance = new LinguisticVariable("distance");
        direction = new LinguisticVariable("direction");

        engine = new FuzzyEngineFactory().Default();
        
        // setting linguistic variables and their curvature
        // input for distance
       //var toLeft = distance.MembershipFunctions.AddTrapezoid("toLeft", 1, 5, 50, 50);
       //var noDist = distance.MembershipFunctions.AddTrapezoid("noDist", -5, -0.5, 0.5, 5);
       //var toRight = distance.MembershipFunctions.AddTrapezoid("toRight", -50, -50, -5, -1);
        var toLeft = distance.MembershipFunctions.AddTriangle("toLeft", -0.47f, -0.3f, -0.05f);
        var noDist = distance.MembershipFunctions.AddTriangle("noDist", -0.2f, 0.0f, 0.2f);
        var toRight = distance.MembershipFunctions.AddTriangle("toRight", 0.05f, 0.3f, 0.47f);
        
        // output for direction
       //var goLeft = direction.MembershipFunctions.AddTrapezoid("goLeft", 1, 5, 40, 50);
       //var isCentred = direction.MembershipFunctions.AddTrapezoid("isCentred", -5, -0.5, 0.5, 5);
       //var goRight = direction.MembershipFunctions.AddTrapezoid("goRight", -40, -50, -5, -1);
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
        // might not need this if statement check
       //if (this.transform.position.x != GameObject.Find("AI_Guideline").transform.position.x)
       //{
            // defuzzify values into precise values
            // distance: the cars position minus the blue line position
            double result = engine.Defuzzify(new { distance = (double)this.transform.position.x - aIGuideline.transform.position.x});
            
            // debug lines
            Debug.Log("Result of distance: " + result);
            
            //result += aIGuideline.transform.position.x;
            
            // setting the results of the fuzzy logic to the car's rigidbody and apply force on the X-Axis
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.AddForce(new Vector3((float)(result),0.0f,0.0f));
       // }
    }

    // Update is called once per frame
    void Update()
    {
        // this is from the uni lab and will be removed when coursework is complete
        /*// adjust the car to gradually position itself so that the line is in the centre of the car
        
       // guideLinePos  = GameObject.Find("AI_GuideLine").transform.position;

        //transform.position = Vector3.Lerp(transform.position, guideLinePos, Time.deltaTime * greenCarSpeed); // can also use Time.time however the car just snaps to the blue line
        //transform.position = Vector3.MoveTowards(transform.position, guideLinePos, 0.05f);
        if (Input.GetMouseButtonDown(0)) {
            var hit = new RaycastHit();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)){
                if (hit.transform.name == "FuzzyBox" )Debug.Log( "You have clicked the FuzzyBox");
                //selected = true;
            }
        }

        if(Input.GetMouseButton(0)/* && selected#1#)
        {
            float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
            transform.position = new Vector3(curPosition.x, Mathf.Max(0.5f, curPosition.y), transform.position.z);
        }*/

       
    }
}
