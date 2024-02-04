using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GreenCar : MonoBehaviour
{
    // Variables
    public float greenCarStartPosX;
    public float greenCarSpeed;
    private Vector3 guideLinePos;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(greenCarStartPosX, -0.2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // adjust the car to gradually position itself so that the line is in the centre of the car
        
        guideLinePos  = GameObject.Find("AI_GuideLine").transform.position;

        transform.position = Vector3.Lerp(transform.position, guideLinePos, Time.deltaTime * greenCarSpeed); // can also use Time.time however the car just snaps to the blue line
        //transform.position = Vector3.MoveTowards(transform.position, guideLinePos, 0.05f);
    }
}
