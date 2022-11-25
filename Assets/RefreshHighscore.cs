using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshHighscore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    	int wave = PlayerPrefs.GetInt("Highscore");
        GetComponent<Text>().text = "HIGHSCORE: WAVE " + wave.ToString();
    }
}
