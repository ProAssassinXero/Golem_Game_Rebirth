using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionSystem : MonoBehaviour
{
    public TimeSystem timeSystem;
    public RockSystem rockSystem;
    public Image[] storeRep;
    public Sprite[] stoneRepImageToAdd;
    public Sprite emptyInventorySlot;
    public GameObject[] rocks;
    public int CollectionIndex = 0;
    public int collectionCount = 0;
    public int DropOffCount = 0;
    public int firstDropOffCount = 5;
    public int secondDropOffCount = 10;
    public int lastDropOffCount = 15;
    public TextMeshPro dropOffText;
    public bool collisionCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rockSystem = GameObject.FindAnyObjectByType<RockSystem>();
    }

    // Update is called once per frame
    void Update()
    {
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

        // Update the maximum time based on the current drop-off count
        UpdateMaxTime();

        // Ensure the collection index does not exceed the length of the storeRep array
        if (CollectionIndex >= storeRep.Length)
        {
            CollectionIndex = storeRep.Length;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collisionCooldown)
        {
            AddToStore(collision);
        }
        

        // Check if the collided object has the tag "DropOff"
        if (collision.CompareTag("Circle"))
        {
            StoreSystem();

            DropOffCount = Mathf.Min(DropOffCount + collectionCount, lastDropOffCount);
            CollectionIndex = 0;
            collectionCount = 0;
            UpdateMaxTime();
        }

        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Circle"))
        {
            timeSystem.pauseTime = true; // Pause the time when the player is in contact with the drop-off area
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Circle"))
        {
            timeSystem.pauseTime = false; // Resume the time when the player exits the drop-off area
        }
    }


    public void AddToStore(Collider2D collision)
    {
        // Call the FindRocks method to update the rocks array
        rockSystem.FindRocks();

        // Check if the collided object has the tag "CommonRock" and if the collection index is within bounds
        if (collision.CompareTag("CommonRock") && CollectionIndex < storeRep.Length)
        {
            // Handle player collection logic
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            storeRep[CollectionIndex].GetComponent<Image>().sprite = stoneRepImageToAdd[0];
            storeRep[CollectionIndex].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            StartCoroutine(CollisionCooldownTimer()); // Start the collision cooldown timer
            CollectionIndex++; // Increment the collection index
            collectionCount++; // Increment the collection count for common rocks
        }

        // Check if the collided object has the tag "UncommonRock" and if the collection index is within bounds
        if (collision.CompareTag("UncommonRock") && CollectionIndex < storeRep.Length)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            storeRep[CollectionIndex].GetComponent<Image>().sprite = stoneRepImageToAdd[1];
            storeRep[CollectionIndex].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            StartCoroutine(CollisionCooldownTimer()); // Start the collision cooldown timer
            CollectionIndex++;
            collectionCount++; // Increment the collection count for uncommon rocks
        }
        // Check if the collided object has the tag "RareRock" and if the collection index is within bounds
        if (collision.CompareTag("RareRock") && CollectionIndex < storeRep.Length)
        {
            // Handle player collection logic
            collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<Collider2D>().enabled = false;
            storeRep[CollectionIndex].GetComponent<Image>().sprite = stoneRepImageToAdd[2];
            storeRep[CollectionIndex].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            StartCoroutine(CollisionCooldownTimer()); // Start the collision cooldown timer
            CollectionIndex++;
            collectionCount++; // Increment the collection count for rare rocks
        }
    }

    public void StoreSystem()
    {
        Debug.Log("StoreSystem called");
        for (int i = 0; i < storeRep.Length; i++)
        {
            if (storeRep[i].GetComponent<Image>().sprite == stoneRepImageToAdd[0])
            {
                timeSystem.time += 2f;
                storeRep[i].GetComponent<Image>().sprite = emptyInventorySlot;
                storeRep[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (storeRep[i].GetComponent<Image>().sprite == stoneRepImageToAdd[1])
            {
                timeSystem.time += 5f;
                storeRep[i].GetComponent<Image>().sprite = emptyInventorySlot;
                storeRep[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else if (storeRep[i].GetComponent<Image>().sprite == stoneRepImageToAdd[2])
            {
                timeSystem.time += 20f;
                storeRep[i].GetComponent<Image>().sprite = emptyInventorySlot;
                storeRep[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }

    private void UpdateMaxTime()
    {
        if (DropOffCount < firstDropOffCount)
        {
            timeSystem.maxTime = 60.4f;
        }
        else if (DropOffCount < secondDropOffCount)
        {
            timeSystem.maxTime = 75.4f;
        }
        else if (DropOffCount < lastDropOffCount)
        {
            timeSystem.maxTime = 90.4f;
        }
        else
        {
            timeSystem.maxTime = 120.4f;
        }
    }

    public IEnumerator CollisionCooldownTimer()
    {
        collisionCooldown = true; // Disable collision handling
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        collisionCooldown = false; // Enable collision handling
    }
}
