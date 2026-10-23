using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public GameObject buttonPressSound;
    public TimeSystem timeSystem;
    public CollectionSystem collectionSystem;
    public GameObject pauseScreen;
    public bool isPaused;
    public bool isGameOver;
    public GameObject controlScreen;
    public GameObject gameOver;
    public GameObject credits;
    public float totalTime;
    public GameObject win;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI WinText;
    public GameObject explosionSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeSystem = GameObject.FindAnyObjectByType<TimeSystem>();
        collectionSystem = GameObject.FindAnyObjectByType<CollectionSystem>();
    }
    void Update()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (timeSystem.time < 0.9f)
            {

                gameOver.SetActive(true);
                Time.timeScale = 0;
                isGameOver = true;
            }

            if (timeSystem.time >= 120)
            {
                win.SetActive(true);
                Time.timeScale = 0;
                Debug.Log("You Win");
            }
            
            totalTime += Time.deltaTime;

            int seconds = Mathf.FloorToInt(totalTime);
            timeText.text = "Your Time: " + seconds.ToString() + " seconds";


            WinText.text = "You weren't suppose to do this... " + "\n" + "Nor were you survive this long..." + "\n" + "You achieved immortality...";
            timeText.text = "Your Time: " + Mathf.RoundToInt(totalTime).ToString() + " seconds";
        }

    }

    // Update is called once per frame
    public void Restart()
    {
        SceneManager.LoadScene(1);
        gameOver.SetActive(true);
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
        timeSystem.time = 60;
        timeSystem.maxTime = 60.9f;
        collectionSystem.DropOffCount = 0;
        Instantiate(buttonPressSound, transform.position, Quaternion.identity);
    }
    public void LevelRestart()
    {
        SceneManager.LoadScene(1);
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
        timeSystem.time = 60;
        timeSystem.maxTime = 60.9f;
        collectionSystem.DropOffCount = 0;
        Instantiate(buttonPressSound, transform.position, Quaternion.identity);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Instantiate(buttonPressSound, transform.position, Quaternion.identity);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Instantiate(buttonPressSound, transform.position, Quaternion.identity);
    }
    public void CloseCredits()
    {
        credits.SetActive(false);
        Instantiate(buttonPressSound, transform.position, Quaternion.identity);
    }
    public void OpenCredits()
    {
        credits.SetActive(true);
        Instantiate(buttonPressSound, transform.position, Quaternion.identity);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void openControlScreen()
    {
        Time.timeScale = 0;
        controlScreen.SetActive(true);
        pauseScreen.SetActive(false);
        Instantiate(buttonPressSound, transform.position, Quaternion.identity);
    }
    public void closeControlScreen()
    {
        Time.timeScale = 1;
        isPaused = false;
        controlScreen.SetActive(false);
        Instantiate(buttonPressSound, transform.position, Quaternion.identity);
    }
    public void OpenPauseScreen()
    {
        Instantiate(buttonPressSound, transform.position, Quaternion.identity);
        if (isPaused)
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            isPaused = true;
        }

    }

}
