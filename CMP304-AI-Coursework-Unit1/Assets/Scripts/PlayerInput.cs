using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Variables
    [SerializeField]
    public float movementSpeed;

    private float lineBoundsLeft = -0.47f;
    private float lineBoundsRight = 0.47f;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= lineBoundsRight && transform.position.x >= lineBoundsLeft)
        {
            float translation = Input.GetAxis("Horizontal") * movementSpeed;

            translation *= Time.deltaTime;
            
            transform.Translate(translation, 0,0);
        }
        else if (transform.position.x >= lineBoundsRight)
        {
            transform.position = new Vector3(0.469f, 0, 0);
        }
        else if (transform.position.x <= lineBoundsLeft)
        {
            transform.position = new Vector3(-0.469f, 0, 0);
        }
        
    }
}
