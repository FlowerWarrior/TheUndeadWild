using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	public GameObject menuCanvas;
	public GameObject gameplayCanvas;
	public Animation cameraAnim;
	public CameraFollow cameraScript;
	public PlayerController	playerController;
	public PlayerRotation playerRotation;
	public Animator playerAnimator;
	public zombiesManager zombiesMgr;
	public GameObject rifleParent;
	public GameObject pauseMenu;
	
	private bool isGameplay = false;
	private bool isPaused = false;
	
	public Texture2D cursorTexture;
	public Texture2D cursorTexturePressed;
	
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public void StartGame()
    {
    	menuCanvas.SetActive(false);
    	gameplayCanvas.SetActive(true);
    	cameraAnim.Play();
    	playerAnimator.SetBool("Gameplay", true);
    	rifleParent.SetActive(true);
    	StartCoroutine(CameraAnimationEnd());
    	isGameplay = true;
    }
    
    public void QuitGame()
    {
    	Application.Quit();
    }
    
    private IEnumerator CameraAnimationEnd()
    {
    	yield return new WaitForSeconds(1.0F);
    	playerRotation.enabled = true;
    	yield return new WaitForSeconds(0.2F);
    	cameraScript.enabled = true;
    	playerController.enabled = true;
    	zombiesMgr.enabled = true;
    }
    
    void Update()
    {
    	if (Input.GetMouseButton(0))
    	{
    		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

    	}
    	else 
    	{
    		Cursor.SetCursor(cursorTexturePressed, hotSpot, cursorMode);
    	}
    
    	if (Input.GetButtonDown("Cancel") && isGameplay)
    	{
    		if (isPaused)
    		{
    			ResumeGame();
    		}
    		else
    		{
    			// Pause
				Time.timeScale = 0;
				playerController.enabled = false;
				playerRotation.enabled = false;
				pauseMenu.SetActive(true);
				isPaused = true;
				playerController.Paused();
    		}
    	}
    }
    
    public void ResumeGame()
    {
    	pauseMenu.SetActive(false);
    	isPaused = false;
    	playerController.enabled = true;
    	playerRotation.enabled = true;
    	Time.timeScale = 1;
    }
}
