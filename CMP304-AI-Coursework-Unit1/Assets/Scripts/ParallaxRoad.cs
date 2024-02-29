using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxRoad : MonoBehaviour
{
    // Variables
    public float roadInitialSpeed;
    private float greenCarSpeed;
    private float lowestPoint;
    private Vector3 highestPoint;

    public bool isLongRoad;
    
    // Start is called before the first frame update
    void Start()
    {
        greenCarSpeed = GameObject.Find("AI_Car_Green").GetComponent<GreenCar>().greenCarSpeed;
        if (!isLongRoad)
        {
            lowestPoint = -1.8f;
            highestPoint = new Vector3(0,1.75f,0);
        }
        else
        {
            lowestPoint = -6.85f;
            highestPoint = new Vector3(0, 7, 0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // move the position of road down the y axis and if it hits below a certain value, set the 
        // image position back to the top value and keep moving down

        if (transform.position.y >= lowestPoint)
        {
            // first step: move image downward
            Vector3 roadTransform = transform.position;
            roadTransform.y -= roadInitialSpeed * Time.deltaTime;
            transform.position = roadTransform;
        }
        else
        {
            transform.position = highestPoint;
        }
        
        // second step: setting its position back to top
    }
}
