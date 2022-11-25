using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRemove : MonoBehaviour
{
	bool fade = false;
	private float fadeSpeed = 0.5F;

    private void OnEnable()
    {
        StartCoroutine(DestroyAfter(4));
    }

    // Update is called once per frame
    private void Update()
    {
        if (fade)
        {
        	//Debug.Log("Fading...");
        	transform.position -= new Vector3 (0, fadeSpeed * Time.deltaTime, 0);
        }
    }
    
    private IEnumerator DestroyAfter(float sec)
    {
    	yield return new WaitForSeconds(sec);
    	GetComponent<Animator>().enabled = false;
    	GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    	fade = true;
    	Destroy(gameObject, 2);
    }
}
