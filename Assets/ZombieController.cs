using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
	public float maxHealth = 20F;
	private float health;
	public float damage;

	private bool canWalk = false;
    private Transform playerTransform;
    private PlayerController playerController;
    private Animator m_Animator;
    private UnityEngine.AI.NavMeshAgent agent;
    
    public AudioClip[] audiosZombieHit;
    public AudioClip audioZombieDeath;
    
    private float distance;
    
    // Start is called before the first frame update
    void Start()
    {
       health = maxHealth;
       agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
       m_Animator = GetComponent<Animator>();
       playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
       playerController  = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
       StartCoroutine(StandingUpY());
    }
    
    private IEnumerator StandingUpY()
    {
    	float distance = 1F;
    	float sec = 2F;
    	float smoothness = 60F;
    	for (int i=0; i<smoothness; i++)
    	{
    		float deltaY = distance/smoothness;
			transform.position += new Vector3(0, deltaY, 0);
			yield return new WaitForSeconds(sec/smoothness);
    	}
    }
    
    public void Ready()
    {
    	agent.enabled = true;
    	canWalk = true;
    	
    	GetComponent<AudioSource>().Play(0);
    }
    
    public void ReceiveDamage(float damage)
    {
    	health -= 10;
    	
    	if (health <= 0)
    	{
    		Death();
    	}
    	else
    	{
    		StartCoroutine(HitImpact());
    	}
    }
    
    private IEnumerator HitImpact()
    {
        GetComponent<AudioSource>().Stop();
    	canWalk = false;
    	agent.enabled = false;
    	m_Animator.SetTrigger("Hit");
    	yield return new WaitForSeconds(0.5F);
    	agent.enabled = true;
    	canWalk = true;
    	GetComponent<AudioSource>().Play(0);
    }
    
    public void Death()
    {
        // Death audio
        AudioSource.PlayClipAtPoint(audioZombieDeath, transform.position);
        
    	m_Animator.SetBool("isAttacking", false);
    	m_Animator.SetBool("isAlive", false);
    	GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    	GetComponent<CapsuleCollider>().enabled = false;
    	transform.parent.gameObject.GetComponent<zombiesManager>().ZombieKilled();
    	GetComponent<ZombieRemove>().enabled = true;
    	GetComponent<ZombieController>().enabled = false;
    }
    
    void Update()
    {
    	if (!transform.parent.gameObject.GetComponent<zombiesManager>().gameOver)
    	{
    	   transform.LookAt(new Vector3 (playerTransform.position.x, transform.position.y, playerTransform.position.z));
    
		   distance = Vector3.Distance(transform.position, playerTransform.position);  
		    
		   if (distance < 2.5F)
		   {
		      m_Animator.SetBool("isAttacking", true);
		      canWalk = false;
		      GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		   }
		   else
		   {
		      m_Animator.SetBool("isAttacking", false);
		   }
		   
		   if (canWalk)
		   {
		      agent.destination = playerTransform.position;
		   }
		   		
    	}
    	else
    	{
    	    GetComponent<AudioSource>().Stop();
    		m_Animator.SetBool("isAttacking", false);
    		m_Animator.SetBool("Gameover", true);
    		GetComponent<ZombieController>().enabled = false;
    		GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
    	}
    }
    
    public void CheckPunchHit()
    {
    	StartCoroutine(PunchDamage());
    }
    
    public IEnumerator PunchDamage()
	{
		float animTime = 0.866F;
		yield return new WaitForSeconds(animTime);
		
		GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
		canWalk = true;
		distance = Vector3.Distance(transform.position, playerTransform.position); 
		 
		if (distance < 2.5F)
		{
			// Play audio
			int num = Random.Range(0, audiosZombieHit.Length);
			AudioSource.PlayClipAtPoint(audiosZombieHit[num], transform.position);
			
			// Deal damage
			playerTransform.gameObject.GetComponent<PlayerController>().ReceiveDamage(damage);
		}
	}
}
