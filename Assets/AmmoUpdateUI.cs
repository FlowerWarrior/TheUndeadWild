using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUpdateUI : MonoBehaviour
{
    public Image ammoBarImage;
	public Text ammoBarText;
	
	private int frameCount;
	
    public void UpdateAmmoInfo(float ammo, float maxAmmo) 
    {
    	ammoBarImage.fillAmount = Mathf.Clamp(ammo / maxAmmo, 0, 1f);
    	ammoBarText.text = ((int)ammo).ToString() + "/" + ((int)maxAmmo).ToString();
    }
    
    public void Reloading(int maxAmmo, float sec)
    {
    	ammoBarText.text = "RELOADING";
    	StartCoroutine(Reload(maxAmmo, sec));
    }
    
    private IEnumerator Reload(int maxAmmo, float sec)
    {
    	yield return new WaitForSeconds(sec);
    	ammoBarImage.fillAmount = 1;
    	UpdateAmmoInfo(maxAmmo, maxAmmo);
    }
    
}
