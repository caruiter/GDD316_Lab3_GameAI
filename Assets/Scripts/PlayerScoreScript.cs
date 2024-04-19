using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//this is a script for keeping track of the player's score and updating the UI

public class PlayerScoreScript : MonoBehaviour
{
    [SerializeField] GameObject tutorialPanel;
    public bool panelPresent;
    private float ct;
    private int sec;

    [SerializeField] GameObject EndScreen;
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;

    public int score;
    [SerializeField] TextMeshProUGUI scoreText; 


    // Start is called before the first frame update
    void Start()
    {
        tutorialPanel.SetActive(true);
        panelPresent = true;

        EndScreen.SetActive(false);

        score = 0;
        scoreText.text = "Berries: 0/15";
    }

    public void UpScore() //player scores a point, update the score
    {
        score++;
        scoreText.text = "Berries: " + score + "/15";
        if (score >= 15) // check if player has enough points to win
        {
            OpenWinScreen();
        }
    }

    public void OpenWinScreen()
    {
        EndScreen.SetActive(true);
        winText.SetActive(true);
        loseText.SetActive(false);
        Time.timeScale = 0;
    }

    public void OpenLoseScreen()
    {
        EndScreen.SetActive(true);
        winText.SetActive(false);
        loseText.SetActive(true);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (panelPresent) // show the tutorial panel for 20 seconds, then set inactive
        {
            ct += Time.deltaTime;

            if(ct >= 1)
            {
                ct = 0;
                sec++;
                if (sec >= 20)
                {
                    tutorialPanel.SetActive(false);
                    panelPresent = false;
                }
            }
        }
    }

    public void RestartGame() //button restarts the game
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
}
