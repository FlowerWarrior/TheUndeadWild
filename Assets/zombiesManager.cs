using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zombiesManager : MonoBehaviour
{
	public GameObject zombiePrefab;	
	public GameObject waveCompletedText;
	
	public float zombiesAmountMulitplier = 1F;
	
	private Transform p_Transform;
	
	private int zombiesAlive = 0;
	private int currentWave = 0;
	
	public bool gameOver = false;
	
    // Start is called before the first frame update
    void Start()
    {
        p_Transform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        
        NextWave();
    }
    
    public void NextWave()
    {
    	currentWave++;
    	StartCoroutine(Wave(currentWave));
    }
    
    private IEnumerator NextWaveAfter(float sec)
    {
    	yield return new WaitForSeconds(sec);
    	NextWave();
    }

	public void GameOver()
	{
		gameOver = true;
		int oldHighscore = PlayerPrefs.GetInt("Highscore");
		
		Debug.Log(oldHighscore);	
		
		if (currentWave-1 > oldHighscore)
		{
			PlayerPrefs.SetInt("Highscore", currentWave-1);
			Debug.Log("newRecord");	
		}
	}

    public void ZombieKilled()
    {
    	zombiesAlive--;
    	
        if (zombiesAlive <= 0)
        {
        	Debug.Log("Wave Completed");
        	waveCompletedText.GetComponent<Text>().text = "WAVE " + currentWave.ToString() + " COMPLETED";
        	waveCompletedText.SetActive(true);
        	
        	//StartCoroutine(NextWaveAfter(4));
        }
    }
    
    private IEnumerator Wave(int difficulty)
    {
    	int zombiesAmount = 4 + difficulty ^ 2 / 2;
    	zombiesAmount = (int) (zombiesAmountMulitplier * (float)zombiesAmount);
    	
    	for (int i=0; i<zombiesAmount; i++)
    	{
    		SpawnZombie(difficulty);
    		
    		float timeRandomization = (float) Random.Range(0, 10) / 10F;
    		yield return new WaitForSeconds(2 - (0.1F * difficulty * zombiesAmountMulitplier) - timeRandomization);
    	}
    }
    
    void SpawnZombie(int toughness)
    {
    	float posX;
    	float posY;
    	
    	float min = 46.5F;
    	float max = 58F;
    	    
    	if (Random.Range(0,2) == 0)
    	{
    		if (Random.Range(0,2) == 0)
			{
				posX = Random.Range(min, max);
			}
			else 
			{
				posX = Random.Range(-min, -max);
			} 
			
    		posY = Random.Range(-max, max);
    	}
    	else 
    	{
    		if (Random.Range(0,2) == 0)
			{
				posY = Random.Range(min, max);
			}
			else 
			{
				posY = Random.Range(-min, -max);
			} 
			
    		posX = Random.Range(-min, min);
    	}   	
    	
    	Vector3 pos = new Vector3(posX, -1, posY);
		GameObject zombie = Instantiate(zombiePrefab, pos, transform.rotation);
		zombie.transform.parent = transform;
		zombie.GetComponent<ZombieController>().maxHealth = 15 + 5 * toughness;
		
		// Faster zombie sometimes
    	int n = Random.Range(0, 100/toughness);
    	if (n == 0)
    	{
    		float speedMultiplier = 1.5F;
    	
    		zombie.GetComponent<UnityEngine.AI.NavMeshAgent>().speed *= speedMultiplier;
    		zombie.GetComponent<Animator>().SetFloat("walkSpeed", speedMultiplier);
    	}
		
		zombiesAlive++;
    }
}
