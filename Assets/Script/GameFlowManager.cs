using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    #region Singleton

    private static GameFlowManager _instance = null;

    public static GameFlowManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameFlowManager>();

                if (_instance == null)
                {
                    Debug.LogError("Fatal Error: GameFlowManager not Found");
                }
            }

            return _instance;
        }
    }

    #endregion
    
    [Header("UI")]
    public GameOverUI GameOverUI;

    public bool IsGameOver { get { return isGameOver; } }

    public bool isGameOver = false;

    private void Start()
    {
        isGameOver = false;
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        GameOverUI.Show();
    }
}
