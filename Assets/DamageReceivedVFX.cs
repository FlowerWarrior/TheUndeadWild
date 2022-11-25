using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageReceivedVFX : MonoBehaviour
{
	public Image img;
	
	private Color32 m_color;
	
    // Start is called before the first frame update
    public void Hit()
    {
    	m_color = new Color32(255, 57, 57, 150);
    }
    
    private void Start()
    {
   		StartCoroutine(ColorAnimation());
   	} 
    
    private IEnumerator ColorAnimation()
    {
    	while (true)
    	{
    		if (m_color.a > 0)
    		{
    			m_color.a -= 1;
		    	img.color = m_color;
    		}
         	yield return new WaitForSeconds(0.003F);
    	}
    }
}
