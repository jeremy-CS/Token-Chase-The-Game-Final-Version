using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //List of possible spawn (for token/player/AI)
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] patrolPoints;
    private List<int> LastRandoms = new List<int>();

    //List of gameObjects
    public GameObject token;
    public GameObject chaser;
    public GameObject seeker;
    public GameObject player;

    //CountDown variables
    [SerializeField] Text countDownText;
    [SerializeField] GameObject countDownUI;

    //Game variables
    [SerializeField] private int NumberOfTokens;
    [SerializeField] private int NumberOfChasers;
    [SerializeField] private bool MovePlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        if (Time.timeScale == 1) Time.timeScale = 0;
        
        SpawnPlayers();
        SpawnTokens();
        StartCountDown();
    }

    public void SpawnTokens()
    {
        int node;

        //Spawn Tokens
        for (int i = 0; i < NumberOfTokens; i++)
        {
            node = GetRandom();
            Vector3 tokenPos = new Vector3(spawnPoints[node].transform.position.x, 2f, spawnPoints[node].transform.position.z);
            Instantiate(token, tokenPos, Quaternion.identity).SetActive(true);
        }
    }

    public void SpawnPlayers()
    {
        int node;

        if (MovePlayer)
        {
            //Spawn Player
            node = GetRandom();
            player.transform.position = spawnPoints[node].transform.position;
        }

        //Spawn Seeker
        node = GetRandom();
        seeker.transform.position =  spawnPoints[node].transform.position;

        //Spawn Chasers
        for (int i=0; i<NumberOfChasers; i++)
        {
            node = GetRandom();
            while (patrolPoints[node].transform.position == seeker.transform.position || patrolPoints[node].transform.position == player.transform.position)
            {
                node = GetRandom();
            }
            
            Instantiate(chaser, patrolPoints[node].transform.position, Quaternion.identity).SetActive(true);
        }
    }

    //Make a 3,2,1.. go countdown before the game starts
    public void StartCountDown()
    {
        countDownUI.SetActive(true);
        StartCoroutine(Countdown(4));
    }

    IEnumerator Countdown(int seconds)
    {
        Time.timeScale = 0;

        while (seconds > 0)
        {
            if (seconds == 1)
            {
                countDownText.text = "GO!!";
            }
            else
            {
                //Show countdown
                countDownText.text = "GAME STARTING..." + (seconds - 1).ToString();
            }

            yield return new WaitForSecondsRealtime(1);
            seconds--;
        }

        countDownUI.SetActive(false);
        Time.timeScale = 1;
    }

    public int GetRandom()
    {
        int rand = Random.Range(0, spawnPoints.Length);
        bool searching = true;

        //Ensure the last random values are different from the new one
        while (searching)
        {
            searching = false;

            foreach (int pastRandom in LastRandoms)
            {
                if (pastRandom == rand)
                {
                    rand = Random.Range(0, spawnPoints.Length);
                    searching = true;
                    break;
                }
            }
        }

        LastRandoms.Add(rand);
        return rand;
    }
}
