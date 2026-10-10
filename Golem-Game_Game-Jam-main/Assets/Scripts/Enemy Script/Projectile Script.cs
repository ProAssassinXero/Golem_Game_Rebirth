using System.Collections;
using TMPro;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public NPC_HolderScript npc_HolderScript;
    public GameObject player; // Reference to the player GameObject
    public GameObject npc; // Reference to the NPC GameObject
    public NPC_1 npcScript; // Reference to the NPC_1 script
    public float overshootingDistance = 10f; // Distance to overshoot the player
    public float speed = 30.0f;
    public Vector2 playerLastPos;
    public Vector2 startPos;
    public Vector2 finalDestination;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npc_HolderScript = Object.FindAnyObjectByType<NPC_HolderScript>();
        player = npc_HolderScript.player;
        npc = npc_HolderScript.npc;
        npcScript = npc_HolderScript.nPC_1;
        
        playerLastPos = player.transform.position;
        startPos = npc.transform.position;
        Vector2 direction = (playerLastPos - startPos).normalized;
        finalDestination = playerLastPos + (direction * overshootingDistance);
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector2.MoveTowards(transform.position, finalDestination, speed * Time.deltaTime);
        if (gameObject.transform.position.x == finalDestination.x && gameObject.transform.position.y == finalDestination.y)
        {
            Destroy(gameObject);
        }
    }
}
