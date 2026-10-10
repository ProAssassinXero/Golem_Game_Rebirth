using System.Collections;
using UnityEngine;

public class RockSystem : MonoBehaviour
{
    public float time = 0f;
    public int randomRock;
    public CollectionSystem collectionSystem; // Reference to the CollectionSystem script

    void Start()
    {
        StartCoroutine(SpawnRocks());
    }

    void Update()
    {

        if (collectionSystem.rocks[randomRock].activeSelf)
        {
            randomRock = Random.Range(0, collectionSystem.rocks.Length);
        }
    }
    public IEnumerator SpawnRocks()
    {
        time = Random.Range(0, 5);
        randomRock = Random.Range(0, collectionSystem.rocks.Length);
        yield return new WaitForSeconds(time);
        collectionSystem.rocks[randomRock].SetActive(true); // Activate a random rock from the array

        StartCoroutine(SpawnRocks());
    }
}
