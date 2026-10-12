using TMPro;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public CollectionSystem collectionSystem;
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
            time = 0;
            timeText.text = "0";
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

    // Method to store the system and calculate time based on the color of the storeRep images
    public void StoreSystem()
    {
        for (int i = 0; i < collectionSystem.storeRep.Length; i++)
        {
            if (collectionSystem.storeRep[i].color == Color.red)
            {
                time += 20f;
                collectionSystem.storeRep[i].color = Color.white;
            }
            else if (collectionSystem.storeRep[i].color == Color.yellow)
            {
                time += 5f;
                collectionSystem.storeRep[i].color = Color.white;
            }
            else if (collectionSystem.storeRep[i].color == Color.green)
            {
                time += 2f;
                collectionSystem.storeRep[i].color = Color.white;
            }
        }
    }
}
