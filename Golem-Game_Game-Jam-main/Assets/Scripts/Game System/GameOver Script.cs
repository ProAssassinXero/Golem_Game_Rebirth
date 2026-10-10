using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public TimeSystem timeSystem;
    public CollectionSystem collectionSystem;
    public GameObject gameOver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeSystem = GameObject.FindAnyObjectByType<TimeSystem>();
        collectionSystem = GameObject.FindAnyObjectByType<CollectionSystem>();
    }
    void Update()
    {
        if (timeSystem.time <= 0.9f)
        {
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Update is called once per frame
    public void Restart()
    {
        SceneManager.LoadScene(0);
        gameOver.SetActive(true);
        Time.timeScale = 1;
        timeSystem.time = 60;
        timeSystem.maxTime = 60.9f;
        collectionSystem.CollectionCount = 0;
        collectionSystem.DropOffCount = 0;
    }

}
