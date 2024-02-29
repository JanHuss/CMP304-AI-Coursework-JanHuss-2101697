using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    // Variables
    private float lowerBounds;
    public float coinSpeed;

    private ItemManager itemManager;
    private ParallaxRoad parallaxRoad;
    public GreenCar greenCar;
    
    // Start is called before the first frame update
    void Start()
    {
        itemManager = GameObject.FindObjectOfType<ItemManager>();
        parallaxRoad = GameObject.FindObjectOfType<ParallaxRoad>();
        
        if (!parallaxRoad.isLongRoad)
            lowerBounds = -1.0f;
        else
            lowerBounds = -3.0f;
        
        if (itemManager == null)
            Debug.LogError("ItemManager not found in the scene.");
    }

    // Update is called once per frame
    void Update()
    {
        // move the coin backwards down the Y axis
        transform.Translate(0.0f, coinSpeed * Time.deltaTime, 0.0f);
        
        //Debug.Log("Red Car Position Y: " + transform.position.y);
        
        // Destroy car when past lower bounds value
        if (transform.position.y <= lowerBounds)
        {
            if (itemManager)
            {
                itemManager.coinSpawned = false;
                itemManager.coinsCollected -= 1;
                itemManager.randomNumber = Random.Range(0, 4);
                Debug.Log("Next RandomNumber for item is set to Lane: " + itemManager.randomNumber);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("RedCarManager reference is null.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "GreenCar")
        {
            itemManager.coinSpawned = false;
            itemManager.coinsCollected += 1;
            itemManager.randomNumber = Random.Range(0, 4);
            Destroy(gameObject);
        }
    }
}
