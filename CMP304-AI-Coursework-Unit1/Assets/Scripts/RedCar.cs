using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCar : MonoBehaviour
{
    public float redCarSpeed;
    private float lowerBounds = -1.0f;
    // Update is called once per frame
    void Update()
    {
        // move the car backwards down the Y axis
        transform.Translate(0.0f, redCarSpeed * Time.deltaTime, 0.0f);
        
        // Debug Lines
        //Debug.Log("Red Car Position Y: " + transform.position.y);
        
        // Destroy car when past bounds of -1 on Y axis
        if (transform.position.y <= lowerBounds)
            Destroy(gameObject);
    }
}
