using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonHover : MonoBehaviour
{
	public AudioClip audioSelect;
	private Transform p_Transform;

	public void OnEnable()
	{
		GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
		p_Transform = GameObject.FindWithTag("Player").GetComponent<Transform>();
	}

    public void OnMouseEnter()
    {
    	if (Time.timeScale == 0)
    	{
    		Time.timeScale = 1;
			AudioSource.PlayClipAtPoint(audioSelect, p_Transform.position);
			Time.timeScale = 0;
    	}
    	else 
    	{
    		AudioSource.PlayClipAtPoint(audioSelect, p_Transform.position);
    	}
    	
    	
    	GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
    
    public void OnMouseLeave()
    {
    	GetComponent<RectTransform>().localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }
}
