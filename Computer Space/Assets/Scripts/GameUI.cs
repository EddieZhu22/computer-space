using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public float playerScore;

    public float enemyScore;

    public GameObject endScreen;

    [SerializeField] private Text playerText, enemyText, timeText, playerFinalResult;
    [SerializeField] private float timeFloat;
    void Update()
    {
        timeFloat += Time.deltaTime;

        //Convert Player Scores into text

        playerText.text = playerScore.ToString();
        enemyText.text = enemyScore.ToString();
        timeText.text = timeFloat.ToString("#");

        //After Game is finished; change this value to adjust how long the game lasts

        //Quiting
        if(Input.GetKeyDown("q")){
            Quit();
        }
        if(timeFloat > 100){
            endScreen.SetActive(true);
            //End Conditions
            if(playerScore > enemyScore){
                playerFinalResult.text = "YOU WIN!";
            }
            else if(playerScore < enemyScore){
                playerFinalResult.text = "YOU LOSE!";
            }
            else if(playerScore == enemyScore){
                playerFinalResult.text = "Bruh how'd you tie this makes no sense";
            }
        }
    }
    public void Again(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit(){
        Application.Quit();
    }
}
