using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RedCar : MonoBehaviour
{
    // Variables
    public float redCarSpeed;
    private float lowerBounds = -1.0f;
    
    private RedCarManager redCarManager;

    private void Start()
    {
        redCarManager = GameObject.FindObjectOfType<RedCarManager>();
        
        if (redCarManager == null)
            Debug.LogError("RedCarManager not found in the scene.");

        redCarManager.randomNumber = Random.Range(0, 4);
        
        //Debug.Log("Next RandomNumber is set to Lane: " + redCarManager.randomNumber);
    }

    // Update is called once per frame
    void Update()
    {
        // move the car backwards down the Y axis
        transform.Translate(0.0f, redCarSpeed * Time.deltaTime, 0.0f);
        
        //Debug.Log("Red Car Position Y: " + transform.position.y);
        
        // Destroy car when past lower bounds value
        if (transform.position.y <= lowerBounds)
        {
            if (redCarManager)
            {
                redCarManager.redCarSpawned = false;
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("RedCarManager reference is null.");
            }
        }
    }
}
