using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterCardsManager : MonoBehaviour
{
    public Sprite imgWalkBoost;
    public Sprite imgJumpBoost;
    public Sprite imgAmmoBoost;
    public Sprite imgBulletsBoost;
    public Sprite imgHealthBoost;
    public Sprite imgDamageBoost;
    public Sprite imgMoreEnemies;
    public Sprite imgFasterReload;
    
    private int[] cardUseLimits = {8, 8, 20, 6, 0, 20, 20, 6};
    private int[] cardUses = {0, 0, 0, 0, 0, 0, 0, 0};
    
    private bool[] cardUnavailable = {false, false, false};
    
    public GameObject ButtonLeft;
    public GameObject ButtonCenter;
    public GameObject ButtonRight;
    
    public PlayerController p_Controller;
    public zombiesManager m_zombiesManager;
    public Healthbar healthbarScript;
    public AmmoUpdateUI ammoUIScript;
    public AudioReloadSpeedManager audioReloadMgr;
    
    private int selectedButton;
    private int[] numbersArray = {0, 0, 0};
    
    private bool wasLastDisplayed = false;
        
    void OnEnable()
    {
    	// Dispaly only every two waves
    	if (wasLastDisplayed)
    	{
    		wasLastDisplayed = false;
    		ContinueGame();
    	}
    	else
    	{
    		// When decided to show
    		
    		wasLastDisplayed = true;
    		
    		p_Controller.enabled = false;
			p_Controller.gameObject.GetComponent<Animator>().SetFloat("horizontalSpeed", 0F);
			p_Controller.gameObject.GetComponent<Animator>().SetFloat("verticalSpeed", 0F);
			p_Controller.gameObject.GetComponent<AudioSource>().Stop();
			p_Controller.isWalking = false;
			
			for (int i = 0; i < 3; i++)
			{
				  // Random numbers without repetition
				  int number = 0;
				  bool exists = true;
				  
				  while (exists)
				  {
				  	  number = Random.Range(1,9);
					  exists = false;
					  for (int j = 0; j < numbersArray.Length; j++)
					  {
					  	if (number == numbersArray[j])
					  	{
					  		exists = true;
					  		break;
					  	}
					  }
				  }
				  
				  
				  numbersArray[i] = number;
				  
				  // Choose adequate text and image
				  Sprite newSprite = imgWalkBoost;
				  string newText = "NO CARD";
				  
				  switch(number)
				  {
				  	case 1:
						newSprite = imgWalkBoost;
						newText = "MOVE FASTER";
				  		break;
				  	
				  	case 2:
						newSprite = imgJumpBoost;
						newText = "JUMP HIGHER";
				  		break;
				  	
				  	case 3:
						newSprite = imgAmmoBoost;
						newText = "MORE AMMO";
				  		break;
				  		
				  	case 4:
						newSprite = imgBulletsBoost;
						newText = "FASTER BULLETS";
				  		break;
				  		
				  	case 5:
						newSprite = imgHealthBoost;
						newText = "RESTORE 50% HEALTH";
				  		break;
				  		
					case 6:
						newSprite = imgDamageBoost;
						newText = "INCREASE DAMAGE";
						break;
						
					case 7:
						newSprite = imgMoreEnemies;
						newText = "MORE ENEMIES";
						break;
						
					case 8:
						newSprite = imgFasterReload;
						newText = "FASTER RELOAD";
						break;
			 	 }
			  	
			  	Color32 m_Color = new Color32(0, 0, 0, 160);
			  	cardUnavailable[i] = false;
			  	
			  	// Check for unavailable cards
			  	if (cardUses[number-1] == cardUseLimits[number-1] && cardUseLimits[number-1] != 0)
			  	{
			  		m_Color = new Color32(145, 0, 0, 160);
			  		cardUnavailable[i] = true;
			  		newText = "NOT AVAILABLE";
			  	}
			  	
				 // Update cards with new data
				 Transform b_Transform;
				  	
				 switch(i)
				 {
					case 0:
						b_Transform = ButtonLeft.GetComponent<Transform>();
						ButtonLeft.GetComponent<Image>().color = m_Color;
					  	
						b_Transform.GetChild(0).GetComponent<Text>().text = newText;
						b_Transform.GetChild(1).GetComponent<Image>().sprite = newSprite;
						break;
						
					case 1:
						b_Transform = ButtonCenter.GetComponent<Transform>();
						ButtonCenter.GetComponent<Image>().color = m_Color;
					  	
						b_Transform.GetChild(0).GetComponent<Text>().text = newText;
						b_Transform.GetChild(1).GetComponent<Image>().sprite = newSprite;
						break;
						
					case 2:
						b_Transform = ButtonRight.GetComponent<Transform>();
						ButtonRight.GetComponent<Image>().color = m_Color;
					  	
						b_Transform.GetChild(0).GetComponent<Text>().text = newText;
						b_Transform.GetChild(1).GetComponent<Image>().sprite = newSprite;
						break;
				 }
			}
    	}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Skipped()
    {
    	ContinueGame();
    }
    
    public void CardSelected(int index) // CARDS: 1, 2, 3
    {
    	int number = numbersArray[index - 1];
    	
    	if (!cardUnavailable[index-1])
    	{
			switch (number)
			{	  
			  	case 1:
					//newText = "MOVE FASTER";
					p_Controller.speed *= 1.05F;
					p_Controller.gameObject.GetComponent<Animator>().speed *= 1.05f;
					p_Controller.gameObject.GetComponent<AudioSource>().pitch *= 1.05f;
					p_Controller.gameObject.GetComponent<AudioSource>().volume *= 1.05f;
			  		break;
			  	
			  	case 2:
					//newText = "JUMP HIGHER";
					p_Controller.jumpForce += 1;
					p_Controller.gameObject.GetComponent<Animator>().SetFloat("jumpSpeed", p_Controller.gameObject.GetComponent<Animator>().GetFloat("jumpSpeed") - 0.1F);
					//p_Controller.gameObject.GetComponent<Animator>().jumpSpeed -= 0.1F;
			  		break;
			  	
			  	case 3:
					//newText = "MORE AMMO";
					p_Controller.maxAmmo = (int)(1.25F * p_Controller.maxAmmo);
					ammoUIScript.UpdateAmmoInfo(p_Controller.currentAmmo, p_Controller.maxAmmo);
					
			  		break;
			  		
			  	case 4:
					//newText = "FASTER BULLETS";
					p_Controller.bulletSpeed *= 1.25F; 
			  		break;
			  		
			  	case 5:
					//newText = "RESTORE 50% HEALTH";
					p_Controller.health += p_Controller.maxHealth / 2;
					
					if (p_Controller.health > p_Controller.maxHealth)
					{
						p_Controller.health = p_Controller.maxHealth;
					}
					
					healthbarScript.UpdateHealthBar(p_Controller.health, p_Controller.maxHealth);
			  		break;
			  		
				case 6:
					//newText = "MORE DAMAGE";
					p_Controller.damage *= 1.25F;
					break;
					
				case 7:
					//newText = "MORE ENEMIES";
					m_zombiesManager.zombiesAmountMulitplier *= 1.25F;
					break;
					
				case 8:
					//newText = "FASTER RELOAD";
					p_Controller.reloadTime *= 0.75F;
					audioReloadMgr.IncreaseSpeed();
					break;
			}
			
			cardUses[number-1]++;
    		ContinueGame();
		}
    }
    
    public void ContinueGame()
    {
    	// Start next wave
  		p_Controller.enabled = true;
  		m_zombiesManager.NextWave();
		gameObject.SetActive(false);
    }
    
    public void ButtonLeftOnClick()
    {
    	CardSelected(1);
    }
    
    public void ButtonCenterOnClick()
    {
    	CardSelected(2);
    }
    
    public void ButtonRightOnClick()
    {
    	CardSelected(3);
    }
}
