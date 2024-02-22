using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

enum CURRENTLANE { LANEONE, LANETWO, LANETHREE, LANEFOUR };

public class RedCarManager : MonoBehaviour
{
    // Variables
    // Objects
    public GameObject redCar;
    
    // Spawn Positions
    // X-axis
    private float laneOneSpawn = -0.47f;
    private float laneTwoSpawn = -0.16f;
    private float laneThreeSpawn = 0.16f;
    private float laneFourSpawn = 0.47f;

    [HideInInspector] public float greenCarLookAtLane;
    
    // Y-axis
    private float upperBounds = 1.0f;

    private CURRENTLANE currentlane;
    [HideInInspector] public int randomNumber;
    
    [HideInInspector] public bool redCarSpawned = false;
   
    // Start is called before the first frame update
    void Start()
    {
        currentlane = CURRENTLANE.LANEONE;
        randomNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentlane = (CURRENTLANE)randomNumber;
        
        if (!redCarSpawned)
        {
            switch (currentlane)
            {
                case CURRENTLANE.LANEONE:
                        Instantiate(redCar, new Vector3(laneOneSpawn, upperBounds, 0.0f), redCar.transform.rotation);
                        greenCarLookAtLane = laneOneSpawn;
                        redCarSpawned = true;

                   // Debug.Log("Lane One");
                    break;
                case CURRENTLANE.LANETWO:
                        Instantiate(redCar, new Vector3(laneTwoSpawn, upperBounds, 0.0f), redCar.transform.rotation);
                        greenCarLookAtLane = laneTwoSpawn;
                        redCarSpawned = true;

                    //Debug.Log("Lane Two");
                    break;
                case CURRENTLANE.LANETHREE:
                        Instantiate(redCar, new Vector3(laneThreeSpawn, upperBounds, 0.0f), redCar.transform.rotation);
                        greenCarLookAtLane = laneThreeSpawn;
                        redCarSpawned = true;
                    
                    //Debug.Log("Lane Three");
                    break;
                case CURRENTLANE.LANEFOUR:
                        Instantiate(redCar, new Vector3(laneFourSpawn, upperBounds, 0.0f), redCar.transform.rotation);
                        greenCarLookAtLane = laneFourSpawn;
                        redCarSpawned = true;
                   
                    //Debug.Log("Lane Four");
                    break;
            }
        }
    }
}
