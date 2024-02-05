using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyBox : MonoBehaviour {

	bool selected = false;

	void Start()
	{
		// Here we need to setup the Fuzzy Inference System
	}

	void FixedUpdate()
	{
		if(!selected && this.transform.position.y < 0.6f)
		{
			// Convert position of box to value between 0 and 100
			double result = 0.0;



			Rigidbody rigidbody = GetComponent<Rigidbody>();
			rigidbody.AddForce(new Vector3((float)(result), 0f, 0f));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			var hit = new RaycastHit();
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)){
				if (hit.transform.name == "FuzzyBox" )Debug.Log( "You have clicked the FuzzyBox");
				selected = true;
			}
		}

		if(Input.GetMouseButton(0) && selected)
		{
			float distanceToScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
			transform.position = new Vector3(curPosition.x, Mathf.Max(0.5f, curPosition.y), transform.position.z);
		}

		if(Input.GetMouseButtonUp(0))
		{
			selected = false;
		}
	}
}
