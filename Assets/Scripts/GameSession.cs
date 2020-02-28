using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    private void Awake()
    {
        int numGameObject = FindObjectsOfType<GameSession>().Length;
        if (numGameObject > 1)
        {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = "Lives: " + playerLives.ToString();
        scoreText.text = "Score: " + score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1) {
            TakeLife();
        }
        else {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = "Score: " + score.ToString();
    }

    public void AddToLives(int livesToAdd)
    {
        Debug.Log("Add Lives");
        this.playerLives += 1;
        livesText.text = "Lives: " + playerLives.ToString();
    }

    private void TakeLife()
    {
        this.playerLives -= 1;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = "Lives: " + playerLives.ToString();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
