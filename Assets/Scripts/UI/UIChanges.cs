using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChanges : MonoBehaviour
{
    //Timer variables
    public float timeRemaining = 300f;
    private string minutes;
    private string seconds;
    private bool timerIsRunning = false;
    private bool timerDone = false;

    //Player/Seeker reference
    public Player playerRef;
    public Seeker_AI seekerRef;

    //Counter variables
    public static int evaderCounter = 0;
    public static int chaserCounter = 0;

    //UI variables
    public Text timerText;
    public Text playerTokenText;
    public Text seekerTokenText;
    public Text playerBombText;
    public Text seekerBombText;
    private int numOfTokens = 0;

    //Game Over UI
    public GameObject gameOverUI;
    public Text winnerText;

    private void Awake()
    {
        evaderCounter = 0;
        chaserCounter = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        seekerRef = GameObject.FindGameObjectWithTag("Seeker").GetComponent<Seeker_AI>();
        numOfTokens = GameObject.FindGameObjectsWithTag("Token").Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            //Update timer
            UpdateTimer();

            //Update Tokens
            UpdateTokens();

            //Update Bomb count
            UpdateFreezeBomb();
        }

        if (GameObject.FindGameObjectsWithTag("Token").Length == 0 || timerDone)
        {
            //End game
            EndGame(false);
        }
    }

    //Update timer on-screen
    public void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            minutes = Mathf.FloorToInt(timeRemaining / 60).ToString();
            seconds = Mathf.FloorToInt(timeRemaining % 60).ToString("00");
        }
        else
        {
            Debug.Log("Timer has run out");
            timeRemaining = 0;
            minutes = "0";
            seconds = "00";
            timerIsRunning = false;
            timerDone = true;
        }
        //Update Timer text
        timerText.text = minutes + "." + seconds;
    }

    //Update token counters
    public void UpdateTokens()
    {
        if (playerRef != null)
            playerTokenText.text = "player: " + playerRef.tokenCounter + " / "+numOfTokens+" tokens";

        seekerTokenText.text = "seeker: " + Seeker_AI.tokenCounter + " / "+numOfTokens+" tokens";
    }

    //Update Freeze Bomb count
    public void UpdateFreezeBomb()
    {
        if (playerRef != null)
            playerBombText.text = playerRef.freezeAmmo + " x freeze bomb";

        if (seekerRef != null)
            seekerBombText.text = seekerRef.FreezeBombAmmo + " x freeze bomb";
    }

    //End Game
    public void EndGame(bool playerFrozen)
    {
        Time.timeScale = 0;
        gameOverUI.SetActive(true);

        //Display the winner's side
        if (playerRef.tokenCounter > Seeker_AI.tokenCounter && !playerFrozen)
        {
            winnerText.text = "player win!!";
        }
        else if (playerRef.tokenCounter < Seeker_AI.tokenCounter || playerFrozen)
        {
            winnerText.text = "seeker win...";
        }
        else
        {
            winnerText.text = "draw...";
        }
    }
}
