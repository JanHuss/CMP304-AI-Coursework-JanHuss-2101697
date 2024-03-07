using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public ItemManager itemManager;
    public RedCarManager redCarManager;

    public TextMeshProUGUI coinsCollectedText;
    public TextMeshProUGUI totalCoinsText;
    public TextMeshProUGUI collectionAccuracyText;
    public TextMeshProUGUI carsCollidedText;
    public TextMeshProUGUI totalCarsText;
    public TextMeshProUGUI avoidanceAccuracyText;

    private float collection;
    private float avoidance;
    
    
    // Start is called before the first frame update
    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        redCarManager = FindObjectOfType<RedCarManager>();

        coinsCollectedText.text = "Coins Collected: " + itemManager.coinsCollected.ToString();
        totalCoinsText.text = "/Total Coins: " + itemManager.totalCoins.ToString();
        collection = ((float)itemManager.coinsCollected / itemManager.totalCoins) * 100;
            
        collectionAccuracyText.text = "Collection Accuracy: " + collection.ToString("F2") + "%";
        
        carsCollidedText.text = "Cars Collided: " + redCarManager.carsCollidedWith.ToString();
        totalCarsText.text = "/Total Cars: " + redCarManager.totalCars.ToString();
        avoidance = ((float)redCarManager.carsCollidedWith / redCarManager.totalCars) * 100;

        avoidanceAccuracyText.text = "Avoidance Accuracy: " + avoidance.ToString("F2") + "%";
    }

    // Update is called once per frame
    void Update()
    {
        coinsCollectedText.text = "Coins Collected: " + itemManager.coinsCollected.ToString();
        totalCoinsText.text = "/Total Coins: " + itemManager.totalCoins.ToString();
        collection = ((float)itemManager.coinsCollected / itemManager.totalCoins) * 100;
        
        collectionAccuracyText.text = "Collection Accuracy: " + collection.ToString("F2") + "%";
        
        carsCollidedText.text = "Cars Collided: " + redCarManager.carsCollidedWith.ToString();
        totalCarsText.text = "/Total Cars: " + redCarManager.totalCars.ToString();
        avoidance = ((float)redCarManager.carsCollidedWith / redCarManager.totalCars) * 100;

        avoidanceAccuracyText.text = "Avoidance Accuracy: " + avoidance.ToString("F2") + "%";
        
    }
}
