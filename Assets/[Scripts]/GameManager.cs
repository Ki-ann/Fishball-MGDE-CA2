using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private float timeLeft = 30f;
    public Text timer, result;
    private bool gameEnd = false;
    public bool win;
    public GameObject endScreen;
    public EnemyAI enemyAIController;
    public PlayerBehaviour playerController;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timerRun();
        if (timeLeft < 0.01f)
        {
            gameEnd = true;
            Time.timeScale = 0;
        }

        if (gameEnd)
        {
            showEndGameScreen();
        }
    }

    public void CheckHP()
    {
        Debug.Log("Check Damage");
        //compare hp??
    }

    public void timerRun()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timer.text = timeLeft.ToString("F2");
        }

    }

    public void showEndGameScreen()
    {
        if (win)
        {
            result.text = "You Win";
            result.color = Color.green;
        }
        else if (!win)
        {
            result.text = "You Lose";
            result.color = Color.red;
        }
        endScreen.SetActive(true);
    }

    public void hideEndGameScreen()
    {
        endScreen.SetActive(false);
    }

    
}
