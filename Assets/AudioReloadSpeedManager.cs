using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReloadSpeedManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    private int currentSpeed = 1;
    
    public void IncreaseSpeed()
    {
    	currentSpeed++;
    	
    	// Normalize speed level
    	if (currentSpeed > audioClips.Length)
    	{
    		currentSpeed = audioClips.Length;
    	}
    	
    	GetComponent<AudioSource>().clip = audioClips[currentSpeed-1];
    }
}
