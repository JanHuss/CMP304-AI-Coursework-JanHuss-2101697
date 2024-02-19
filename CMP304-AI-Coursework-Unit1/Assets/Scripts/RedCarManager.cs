using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RedCarManager : MonoBehaviour
{
    // Variables
    public GameObject redCar;
    
    // Spawn Positions
    // X-axis
    private float laneOneSpawn = -0.47f;
    private float laneTwoSpawn = -0.16f;
    private float laneThreeSpawn = 0.16f;
    private float laneFourSpawn = 0.47f;
    // Y-axis
    private float upperBounds = 1.0f;
    
    // car needs to spawn on different lanes. Y spawn position is 1.0f. maybe spawn one at a time for now
    
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate Red Car
        Instantiate(redCar, new Vector3(laneOneSpawn, upperBounds, 0.0f), redCar.transform.rotation);
        //Instantiate(redCar, new Vector3(laneTwoSpawn, upperBounds, 0.0f), redCar.transform.rotation);
        //Instantiate(redCar, new Vector3(laneThreeSpawn, upperBounds, 0.0f), redCar.transform.rotation);
        //Instantiate(redCar, new Vector3(laneFourSpawn, upperBounds, 0.0f), redCar.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
