using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    public CollectionSystem collectionSystem;
    public GameObject player;
    // Store the last drop-off count to calculate the difference
    public TextMeshProUGUI timeText;
    public float time = 60f;
    public float maxTime = 60.4f;

    public bool pauseTime = false; // Flag to control whether time should be paused
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease time based on deltaTime
        if (!pauseTime)
        {
            time -= Time.deltaTime;
        }
        int seconds = Mathf.FloorToInt(time);
        timeText.text = seconds.ToString() + " / " + Mathf.RoundToInt(maxTime).ToString();

        // Ensure time does not go below zero
        if (time <= 0)
        {
            time = 0; // Disable the player GameObject when time reaches zero
            timeText.text = "0";
        }

        // Enable or disable the player GameObject based on the time value
        if (time >= 0.9f)
        {
            player.SetActive(true);
        }
        else if (time < 0.9f)
        {
            player.SetActive(false);
        }

        // Ensure time does not exceed maxTime
        if (time > maxTime)
        {
            time = maxTime;
        }
    }

    // Method to reduce time by 5 seconds when the player dashes
    public void DashTimeLoss()
    {
        time -= 5f;
    }
}
