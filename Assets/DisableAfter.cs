using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfter : MonoBehaviour
{
	public float sec;
	public GameObject boosterCards;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
    	yield return new WaitForSeconds(sec);
    	
    	boosterCards.SetActive(true);
    	
    	gameObject.SetActive(false);
    }
}
