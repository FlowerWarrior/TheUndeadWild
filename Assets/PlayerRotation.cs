using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
	public Transform rifleHolderTransform;

	private Vector3 mouseWorldPos;
	
	private bool ready = false;
	private int speed = 8;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(ReadyAfter(1F));
    }
    
    private IEnumerator ReadyAfter(float sec)
    {
    	yield return new WaitForSeconds(sec);
    	ready = true;
    }

    // Update is called once per frame
    void Update()
    {
    	// Get mouse position in world
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			mouseWorldPos = hit.point;
			//Debug.Log(mouseWorldPos);
		}
    
    	if (ready)
    	{
		    // Rotate player to mouse
		    transform.LookAt(new Vector3 (mouseWorldPos.x, transform.position.y, mouseWorldPos.z));
		    
		    // Rifle rotation
		    //rifleHolderTransform.LookAt(new Vector3 (mouseWorldPos.x, rifleHolderTransform.position.y, mouseWorldPos.z));
    	}
    	else
    	{
    		var targetRotation = Quaternion.LookRotation(new Vector3 (mouseWorldPos.x, transform.position.y, mouseWorldPos.z) - transform.position);
       
			// Smoothly rotate towards the target point.
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);			 
    	}
    }
}
