using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
	public float speed;
	public float damage;
	public GameObject particlesPrefab;
	
	public AudioClip[] audiosBodyhit;
	public float volume = 1F;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfter(3));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    
    IEnumerator DestroyAfter(float t)
	{
		yield return new WaitForSeconds(t);
		Destroy(gameObject);
	}
	
	void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
        	collision.gameObject.GetComponent<ZombieController>().ReceiveDamage(damage);
        	
        	Vector3 pos = transform.position;
			Instantiate(particlesPrefab, pos, transform.rotation);
			
			// Play body hit audio
			int variation = Random.Range(0,3);
			AudioSource.PlayClipAtPoint(audiosBodyhit[variation], pos, volume);
			
        	Destroy(gameObject);
        }
    }
}
