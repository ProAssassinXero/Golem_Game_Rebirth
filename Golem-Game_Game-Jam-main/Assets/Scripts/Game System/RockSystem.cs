using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSystem : MonoBehaviour
{
    public GameObject[] rocksC;
    public GameObject[] rocksU;
    public GameObject[] rocksR;
    public List<GameObject> rocks = new List<GameObject>();
    public float time = 0f;
    public int randomRock;
    public CollectionSystem collectionSystem; // Reference to the CollectionSystem script

    void Start()
    {
        StartCoroutine(SpawnRocks());
        FindRocks();
    }
    public IEnumerator SpawnRocks()
    {
        foreach (var rock in rocks)
        {
            float randomTime = Random.Range(3f, 5f); // Random time between 3 and 5 seconds
            yield return new WaitForSeconds(randomTime); // Wait for the random time before spawning the next rock
            randomRock = Random.Range(0, rocks.Count); // Get a random index from the list of rocks
            if (!rocks[randomRock].GetComponent<SpriteRenderer>().enabled)
            {
                rocks[randomRock].GetComponent<SpriteRenderer>().enabled = true; // Enable the rock's sprite renderer to make it visible
                rocks[randomRock].GetComponent<Collider2D>().enabled = true; // Enable the rock's collider to make it interactable
                collectionSystem.collectionCount++; // Increment the collection count in the CollectionSystem script
                rocks.RemoveAt(randomRock); // Remove the spawned rock from the list to avoid spawning it again
            }
        }
    }

    public void FindRocks()
    {
        StartCoroutine(SpawnRocks());
        rocksC = GameObject.FindGameObjectsWithTag("CommonRock"); // Find all rocks in the scene with the tag "CommonRock"
        rocksU = GameObject.FindGameObjectsWithTag("UncommonRock"); // Find all rocks in the scene with the tag "UncommonRock"
        rocksR = GameObject.FindGameObjectsWithTag("RareRock"); // Find all rocks in the scene with the tag "RareRock"

        for (int i = 0; i < rocksC.Length; i++)
        {
            if (!rocksC[i].GetComponent<SpriteRenderer>().enabled)
            {
                rocks.Add(rocksC[i]);
            }
        }
        for (int i = 0; i < rocksU.Length; i++)
        {
            if (!rocksU[i].GetComponent<SpriteRenderer>().enabled)
            {
                rocks.Add(rocksU[i]);
            }
        }
        for (int i = 0; i < rocksR.Length; i++)
        {
            if (!rocksR[i].GetComponent<SpriteRenderer>().enabled)
            {
                rocks.Add(rocksR[i]);
            }
        }
    }
}
