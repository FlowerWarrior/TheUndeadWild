using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
	public Image healthBarImage;
	public Text healthBarText;
	
    public void UpdateHealthBar(float health, float maxHealth) 
    {
    	healthBarImage.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1f);
    	healthBarText.text = ((int)health).ToString() + "/" + ((int)maxHealth).ToString();
    }
}
