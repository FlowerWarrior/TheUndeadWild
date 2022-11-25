using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public float maxHealth = 100F;
	public float health;
	public float damage;
	public float jumpForce;
	public float bulletSpeed = 20F;
	public int maxAmmo = 8;
	public int currentAmmo;
	public float reloadTime = 2F;
	
	public Transform rifleTransform;
	
	public AudioSource audioShoot;
	public AudioSource audioFootsteps;
	public AudioSource audioSourceReload;
	public AudioSource audioJumpUp;
	public AudioSource audioJumpDown;
	
    private float volume = 1F;
	
    public float speed = 6.0F;
    public float fireRate = 2F;
	public float gravity = 20.0F;
	public GameObject projectilePrefab;
	public DamageReceivedVFX damageVFX;
	public Healthbar healthbar;
	public AmmoUpdateUI ammoUI;
	public zombiesManager zombiesMgr;
	public GameObject gameOverText;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController m_Controller;
	private Animator m_Animator;
	
	private float m_HorizontalMovement;
	private float m_VerticalMovement;
	
	private bool canShoot = true;
	private bool isReloading = false;
	
	private bool isFalling = false;
	public bool isWalking = false;
	
	void Start()
	{
		health = maxHealth;
		currentAmmo = maxAmmo;
		
		m_Controller = GetComponent<CharacterController>();
		m_Animator = GetComponent<Animator>();
	}
	
	void Shoot()
	{
		currentAmmo--;
		ammoUI.UpdateAmmoInfo(currentAmmo, maxAmmo);
		Vector3 position = rifleTransform.position + rifleTransform.forward * 1.1F;
		position.y += 0.1F;
		GameObject bullet = Instantiate(projectilePrefab, position, transform.rotation);
		bullet.GetComponent<ProjectileMovement>().damage = damage;
		bullet.GetComponent<ProjectileMovement>().speed = bulletSpeed;
		
		//AudioSource.PlayClipAtPoint(audioShoot, position, volume);
		audioShoot.Play(0);
		
		if (currentAmmo == 0)
		{
			StartCoroutine(Reload());
		}
	}
	
	IEnumerator ShootDelay()
	{
		yield return new WaitForSeconds(1/fireRate);
		canShoot = true;
	}
		
	public void ReceiveDamage(float damage)
	{
		if (health > 0)
		{
			damageVFX.Hit();
			health -= damage;
			healthbar.UpdateHealthBar(health, maxHealth);
			
			if (health <= 0)
			{
				Death();
			}
		}		
	}
	
	private void Death()
	{
		m_Animator.SetBool("isAlive", false);
		damageVFX.gameObject.SetActive(false);
		zombiesMgr.GameOver();
		gameOverText.SetActive(true);
		StartCoroutine(LoadSceneAfter(8));
		GetComponent<PlayerRotation>().enabled = false;
		GetComponent<PlayerController>().enabled = false;
	}
			
	private IEnumerator LoadSceneAfter(float sec)
	{
		yield return new WaitForSeconds(sec);
		SceneManager.LoadScene("SampleScene");
	}
	
	private IEnumerator Reload()
	{
		if (!isReloading)
		{
			canShoot = false;
			isReloading = true;
			currentAmmo = maxAmmo;
			ammoUI.Reloading(maxAmmo, reloadTime);
			
			// Play reload audio
			audioSourceReload.Play(0);
			
			yield return new WaitForSeconds(reloadTime + 0.1F);
			
			//ammoUI.UpdateAmmoInfo(currentAmmo, maxAmmo);
			canShoot = true;
			isReloading = false;
		}
	}
	
	public void StartedFalling()
	{
		isFalling = true;
	}
			
	public void Paused()
	{
		isWalking = false;
        audioFootsteps.Stop();
	}
			
	void Update() 
	{
		bool cursorOverButton = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
		
		if (Input.GetButton("Fire1") && canShoot && !cursorOverButton && !isReloading)
		{
			if (currentAmmo > 0)
			{
				Shoot();
				canShoot = false;
				StartCoroutine(ShootDelay());
			}
			else 
			{
				StartCoroutine(Reload());
			}
			
		}
		
		if (currentAmmo < maxAmmo && Input.GetKeyDown("r") && canShoot && !isReloading)
		{
			StartCoroutine(Reload());
		}
		
		// Character is on ground (built-in functionality of Character Controller)
		if (m_Controller.isGrounded) 
		{		
			m_Animator.SetBool("isGrounded", true);
		
			m_HorizontalMovement = Input.GetAxis("Horizontal");
			m_VerticalMovement = Input.GetAxis("Vertical");
		
			// Use input up and down for direction, multiplied by speed
			moveDirection = new Vector3(m_HorizontalMovement, transform.position.y, m_VerticalMovement);
			moveDirection *= speed;

			if (Input.GetButton("Jump"))
			{
				moveDirection += new Vector3(0, jumpForce, 0);
				audioFootsteps.Stop();
				audioJumpUp.Play(0);
				m_Animator.SetTrigger("Jump");
			}

			//Debug.Log(transform.forward);
            m_Animator.SetFloat("horizontalSpeed", (transform.forward.z) * moveDirection.x + -transform.forward.x * moveDirection.z);
            m_Animator.SetFloat("verticalSpeed", (transform.forward.x) * moveDirection.x + (transform.forward.z) * moveDirection.z );
            
            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
            	if (!isWalking) 
            	{
            		isWalking = true;
		        	// Enable footsteps audio
		        	audioFootsteps.Play(0);
            	}
            }
            else if (moveDirection.x == 0 && moveDirection.y == 0)
            {
            	isWalking = false;
            	// Stop footsteps audio
            	audioFootsteps.Stop();
            }
            
            // Jumping things
            if (isFalling == true)
			{
				isFalling = false;
				audioJumpDown.Play(0);
				if (moveDirection.x != 0 || moveDirection.z != 0)
				{
					audioFootsteps.Play(0);
				}
			}
		}
		else if (!m_Controller.isGrounded)
		{
			m_Animator.SetBool("isGrounded", false);
		}
		
		// Apply gravity manually.
		moveDirection.y -= gravity * Time.deltaTime;
		// Move Character Controller
		m_Controller.Move(moveDirection * Time.deltaTime);
		
		// Map bounds
		
		float normalizedX = transform.position.x;
		float normalizedZ = transform.position.z;
		
		float limit = 43.6F;
		
		if (normalizedZ > limit)
		{
			normalizedZ = limit;
		}
		else if (normalizedZ < -limit)
		{
			normalizedZ = -limit;
		}
		
		if (normalizedX > limit)
		{
			normalizedX = limit;
		}
		else if (normalizedX < -limit)
		{
			normalizedX = -limit;
		}
		
		transform.position = new Vector3(normalizedX, transform.position.y, normalizedZ);
	}
}
