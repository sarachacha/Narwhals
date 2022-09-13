using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    //public GameController GameController;
    
    //public static GameOverScreen instance;

    public void BankPoints(int totalPoints)
    {
        gameObject.SetActive(true);
        pointsText.text = "POINTS: " + totalPoints.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("GameScene_Level_1");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
