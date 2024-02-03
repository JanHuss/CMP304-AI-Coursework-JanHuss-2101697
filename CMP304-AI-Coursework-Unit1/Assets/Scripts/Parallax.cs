using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Variables
    private float length;
    private float startPos;
    
    public GameObject camera;
    public float parallaxSpeed; // variable to change speed of parallax in Unity editor
    
    
    // Start is called before the first frame update
    void Start()
    {
           startPos = transform.position.y;
           length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (camera.transform.position.y * (1 - parallaxSpeed));
        float distance = (camera.transform.position.y * parallaxSpeed);

        transform.position = new Vector3(transform.position.x, startPos + distance, transform.position.z);

        if (temp > startPos + length)
            startPos += length;
        else if (temp < startPos - length)
            startPos -= length;
    }
}
