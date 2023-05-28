using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITime : MonoBehaviour
{
    #region Singleton

    private static UITime _instance = null;

    public static UITime Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UITime>();

                if (_instance == null)
                {
                    Debug.LogError("Fatal Error: UITime not Found");
                }
            }

            return _instance;
        }
    }

    #endregion

    Image timerBar;
    public float maxTime;
    float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft>0){
            timeLeft-= Time.deltaTime;
            timerBar.fillAmount = timeLeft/maxTime;
        }else{
            GameFlowManager.Instance.GameOver();
        }
    }

    public void ResetTimer()
    {
        timeLeft = maxTime;
    }
}
