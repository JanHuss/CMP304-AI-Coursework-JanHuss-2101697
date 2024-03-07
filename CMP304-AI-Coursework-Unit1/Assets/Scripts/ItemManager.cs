using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Variables
    // Objects
    public GameObject coin;
    private ParallaxRoad parallaxRoad;
    private GreenCar greenCar;
    
    // Spawn Positions
    // X-axis
    private float laneOneSpawn = -0.47f;
    private float laneTwoSpawn = -0.16f;
    private float laneThreeSpawn = 0.16f;
    private float laneFourSpawn = 0.47f;
    
    [HideInInspector] public float greenCarLookAtCoin;
    
    // Y-axis
    private float upperBounds = 1.0f;
    
    public bool isLongRoad;

    private CURRENTLANE currentlane;
    [HideInInspector] public int randomNumber;
    [HideInInspector] public bool coinSpawned = false;
    public int coinsCollected;
    
    
    // Start is called before the first frame update
    void Start()
    {
        parallaxRoad = GameObject.FindObjectOfType<ParallaxRoad>();
        greenCar = GameObject.FindObjectOfType<GreenCar>();
        
        if (!parallaxRoad.isLongRoad)
            upperBounds = Random.Range(1.0f, 2.0f);
        else
            upperBounds = Random.Range(3.0f, 5.0f);
        
        currentlane = CURRENTLANE.LANEONE;
        randomNumber = 2;
        coinsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentlane = (CURRENTLANE)randomNumber;
        
        if (!coinSpawned && greenCar.addItem)
        {
            switch (currentlane)
            {
                case CURRENTLANE.LANEONE:
                    Instantiate(coin, new Vector3(laneOneSpawn, upperBounds, 0.0f), coin.transform.rotation);
                    greenCarLookAtCoin = laneOneSpawn;
                    coinSpawned = true;

                    // Debug.Log("Lane One");
                    break;
                case CURRENTLANE.LANETWO:
                    Instantiate(coin, new Vector3(laneTwoSpawn, upperBounds, 0.0f), coin.transform.rotation);
                    greenCarLookAtCoin = laneTwoSpawn;
                    coinSpawned = true;

                    //Debug.Log("Lane Two");
                    break;
                case CURRENTLANE.LANETHREE:
                    Instantiate(coin, new Vector3(laneThreeSpawn, upperBounds, 0.0f), coin.transform.rotation);
                    greenCarLookAtCoin = laneThreeSpawn;
                    coinSpawned = true;
                    
                    //Debug.Log("Lane Three");
                    break;
                case CURRENTLANE.LANEFOUR:
                    Instantiate(coin, new Vector3(laneFourSpawn, upperBounds, 0.0f), coin.transform.rotation);
                    greenCarLookAtCoin = laneFourSpawn;
                    coinSpawned = true;
                   
                    //Debug.Log("Lane Four");
                    break;
            }
        }
    }
}
