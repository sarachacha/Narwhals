using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField]
    GameObject Player;

    [SerializeField]
    GameObject[] RespawnPoints;

    GameObject activeDiver;
    [SerializeField]
    GameObject[] Divers;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI heldText;
    public TextMeshProUGUI livesText;

    public int totalPoints;
    public int storedPoints;
    public int multiplier = 0;

    public int lives = 1;

    public GameOverScreen GameOverScreen;
    

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        livesText.text = "LIVES: " + lives.ToString();

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        if(RespawnPoints.Length == 0)
        {
            RespawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        }

        if (Divers.Length == 0)
        {
            Divers = GameObject.FindGameObjectsWithTag("Diver");

            for(int i = 0; i < Divers.Length; i++)
            {
                Divers[i].SetActive(false);
            }
        }
    }

    public void GetPoints(int points)
    {
        storedPoints += points;

        heldText.text = "TRASH: " + storedPoints.ToString();

        if (activeDiver == null && Divers != null && Divers.Length > 0)
        {
            activeDiver = Divers[Random.Range(0, Divers.Length)];

            activeDiver.SetActive(true);
        }
    }

    public void BankPoints()
    {
        totalPoints += storedPoints * multiplier;

        ResetStoredPoints();

        if (scoreText != null)
            scoreText.text = "POINTS: " + totalPoints.ToString();
    }

    public void IncreaseMultiplier()
    {
        multiplier += 1;
    }

    public void ResetStoredPoints()
    {
        storedPoints = 0;

        if (heldText != null)
            heldText.text = "TRASH: " + storedPoints.ToString();

        if (activeDiver != null)
        {
            activeDiver.SetActive(false);

            activeDiver = null;
        }
    }

    public void ResetMultiplier()
    {
        multiplier = 0;
    }


    public void PlayerDie()
    {
        Player.SetActive(false);

        ResetMultiplier();

        ResetStoredPoints();

        LoseLife();

        if (lives > 0)
        {
            PlayerRespawn();
        }
        
    }

    public void PlayerRespawn()
    {
        Random.Range(0, RespawnPoints.Length);

        if(RespawnPoints != null && RespawnPoints.Length > 0)
        {
            Player.transform.position = RespawnPoints[Random.Range(0, RespawnPoints.Length)].transform.position;
        }
        else
        {
            Player.transform.position = Vector3.zero;
        }

        Player.SetActive(true);
    }
    
    public void LoseLife()
    {
        lives -= 1;

        livesText.text = "LIVES: " + lives.ToString();

        if (lives <= 0)
        {
            EndGame();
        }


    }

    public void EndGame()
    {
        GameOverScreen.BankPoints(totalPoints);
        
    }
}
