using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 0.5f;
    [SerializeField] float LevelExitSlowMoFactor = 0.2f;
    [SerializeField] AudioClip LevelWinSFX;
    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LoadNextLevel());
        AudioSource.PlayClipAtPoint(LevelWinSFX, Camera.main.transform.position);
    }

    IEnumerator LoadNextLevel()
    {
        Time.timeScale = LevelExitSlowMoFactor;
        yield return new WaitForSecondsRealtime(LevelLoadDelay);
        Time.timeScale = 1f;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
