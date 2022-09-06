using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

   GameObject[] RespawnPoints;

    public int totalPoints;
    public int multiplier = 1;

    public int lives = 3;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPoints(int points)
    {
        totalPoints += points * multiplier;
    }

    public void IncreaseMultiplier()
    {
        multiplier += 1;
    }
    
    public void ResetMultiplier()
    {
        multiplier = 1;
    }


    public void PlayerDie()
    {
        Player.SetActive(false);

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
