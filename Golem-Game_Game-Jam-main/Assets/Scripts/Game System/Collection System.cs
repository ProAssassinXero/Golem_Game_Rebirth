using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionSystem : MonoBehaviour
{
    public TimeSystem timeSystem;
    public Image[] storeRep;
    public GameObject[] rocks;
    public int CollectionIndex = 0;
    public int CollectionCount = 0;
    public int DropOffCount = 0;
    public int firstDropOffCount = 5;
    public int secondDropOffCount = 10;
    public int lastDropOffCount = 15;
    public TextMeshPro collectionText;
    public TextMeshPro dropOffText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the collection text with the current counts
        collectionText.text = "Collection Count: " + CollectionCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the drop-off text based on the current drop-off count and thresholds
        if (DropOffCount < firstDropOffCount)
        {
            dropOffText.text = DropOffCount.ToString() + " / " + firstDropOffCount;
        }
        else if (DropOffCount < secondDropOffCount)
        {
            dropOffText.text = DropOffCount.ToString() + " / " + secondDropOffCount;
        }
        else if (DropOffCount < lastDropOffCount)
        {
            dropOffText.text = DropOffCount.ToString() + " / " + lastDropOffCount;
        }
        else if (DropOffCount >= lastDropOffCount)
        {
            DropOffCount = lastDropOffCount; // Cap the drop-off count at the last drop-off count
            dropOffText.text = DropOffCount.ToString() + " / " + lastDropOffCount;
        }

        if (DropOffCount >= firstDropOffCount && DropOffCount < secondDropOffCount)
        {
            timeSystem.maxTime = 75f; // Increase the max time to 75 seconds when the last drop-off count is reached
        }
        else if(DropOffCount >= secondDropOffCount && DropOffCount < lastDropOffCount)
        {
            timeSystem.maxTime = 90f; // Increase the max time to 90 seconds when the second drop-off count is reached
        }
        else if (DropOffCount >= lastDropOffCount)
        {
            timeSystem.maxTime = 120f; // Increase the max time to 120 seconds when the first drop-off count is reached
        }



        // Ensure the collection index does not exceed the length of the storeRep array
        if (CollectionIndex >= storeRep.Length)
        {
            CollectionIndex = storeRep.Length;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the tag "CommonRock" and if the collection index is within bounds
        if (collision.CompareTag("CommonRock") && CollectionIndex < storeRep.Length)
        {
            // Handle player collection logic
            collision.gameObject.SetActive(false); // Deactivate the collected rock

            storeRep[CollectionIndex].color = Color.green; // Change the color of the current image to green
            CollectionIndex++; // Increment the collection index
            CollectionCount++; // Increment the collection count
            collectionText.text = "Collection Count: " + CollectionCount.ToString();

        }

        // Check if the collided object has the tag "UncommonRock" and if the collection index is within bounds
        if (collision.CompareTag("UncommonRock") && CollectionIndex < storeRep.Length)
        {
            collision.gameObject.SetActive(false);
            storeRep[CollectionIndex].color = Color.yellow;
            CollectionIndex++;
            CollectionCount++;
            collectionText.text = "Collection Count: " + CollectionCount.ToString();

        }
        // Check if the collided object has the tag "RareRock" and if the collection index is within bounds
        if (collision.CompareTag("RareRock") && CollectionIndex < storeRep.Length)
        {
            // Handle player collection logic
            collision.gameObject.SetActive(false);
            storeRep[CollectionIndex].color = Color.red;
            CollectionIndex++;
            CollectionCount++;
            collectionText.text = "Collection Count: " + CollectionCount.ToString();
        }


        // Check if the collided object has the tag "DropOff"
        if (collision.CompareTag("DropOff"))
        {
            timeSystem.StoreSystem();

            DropOffCount += CollectionCount; // Update the drop-off count with the current collection count
            CollectionCount = 0; // Reset the collection count
            CollectionIndex = 0; // Reset the collection index

            collectionText.text = "Collection Count: " + CollectionCount.ToString();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("DropOff"))
        {
            timeSystem.pauseTime = true; // Pause the time when the player is in contact with the drop-off area
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DropOff"))
        {
            timeSystem.pauseTime = false; // Resume the time when the player exits the drop-off area
        }
    }
}
