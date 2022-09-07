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

    GameObject[] RespawnPoints;

    public TextMeshProUGUI scoreText;

    public int totalPoints;
    public int storedPoints;
    public int multiplier = 1;

    public int lives = 3;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        if(RespawnPoints == null)
        {
            RespawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        }
    }

    public void GetPoints(int points)
    {
        storedPoints += points;
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
    }

    public void ResetMultiplier()
    {
        multiplier = 1;
    }


    public void PlayerDie()
    {
        Player.SetActive(false);

        ResetMultiplier();

        ResetStoredPoints();

        PlayerRespawn();
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

        if(lives <= 0)
        {
            EndGame();
        }
    }

    

    public void EndGame()
    {

    }
}
