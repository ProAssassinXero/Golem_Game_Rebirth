using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public TimeSystem timeSystem;
    public CollectionSystem collectionSystem;
    public GameObject gameOver;
    public GameObject credits;
    public float totalTime;
    public GameObject win;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI WinText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeSystem = GameObject.FindAnyObjectByType<TimeSystem>();
        collectionSystem = GameObject.FindAnyObjectByType<CollectionSystem>();
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (timeSystem.time <= 0.9f)
            {
                gameOver.SetActive(true);
                Time.timeScale = 0;
            }

            if (timeSystem.time >= 120)
            {
                win.SetActive(true);
                Time.timeScale = 0;
            }
            if (timeSystem.time < 120)
            {
                totalTime = 0;
                totalTime += Time.deltaTime;
                int seconds = Mathf.FloorToInt(totalTime);
            }
            

            WinText.text = "You weren't suppose to do this... " + "\n" + "Nor were you survive this long..." + "\n" + "You achieved immortality...";
            timeText.text = "Your Time: " + Mathf.RoundToInt(totalTime).ToString() + " seconds";
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
    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }
    public void CloseCredits()
    {
        credits.SetActive(false);
    }
    public void OpenCredits()
    {
        credits.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
