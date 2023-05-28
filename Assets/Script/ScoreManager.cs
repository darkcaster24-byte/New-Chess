using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Singleton

    private static ScoreManager _instance = null;

    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreManager>();

                if (_instance == null)
                {
                    Debug.LogError("Fatal Error: ScoreManager not Found");
                }
            }

            return _instance;
        }
    }

    #endregion

    public int CurrentScore { get { return currentScore; } }

    private int currentScore;

    private void Start()
    {
        ResetCurrentScore();
    }

    public void ResetCurrentScore()
    {
        currentScore = 0; 
    }

    public void IncrementCurrentScore(int block)
    {
        if(block == 0 || block==2){
            currentScore+=2;
        }
        if(block == 1 || block==3){
            currentScore+=1;
        }
    }
}
