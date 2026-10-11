using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public TimeSystem timeSystem;
    public GameObject player;
    public GameObject npc;
    public Vector2 direction;
    public float overshootingDistance = 10f;
    public float speed = 30.0f;
    public Vector2 startPos;
    public Vector2 finalDestination;
    private bool initialized = false;
    public bool knockback;

    void Start()
    {
        timeSystem = GameObject.FindGameObjectWithTag("TimeSystem").GetComponent<TimeSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy the projectile if it collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            knockback = true; // Set knockback to true when the projectile hits the player
            timeSystem.time -= 5f; // Reduce time by 5 seconds when the projectile hits the player
            gameObject.GetComponent<Collider2D>().enabled = false; // Disable the collider to prevent further collisions
            gameObject.GetComponent<SpriteRenderer>().enabled = false; // Optionally hide the projectile visually
            player.GetComponent<Rigidbody2D>().AddForce(direction * 10f, ForceMode2D.Impulse); // Apply knockback force to the player
        }
        if (knockback == true)
        {
            StartCoroutine(DestroyProjectileAfterTime());
        }
        // Check if the projectile has already been initialized to avoid re-initialization
        if (initialized) return;

        // Check if the collided object has the NPC_1 script
        NPC_1 npcScript = collision.GetComponent<NPC_1>();
        if (npcScript == null) return;

        // Initialize the projectile's movement towards the player
        npc = collision.gameObject;
        player = npcScript.player;
        if (player == null) return;

        // Calculate the direction from the NPC to the player and set the final destination
        startPos = npc.transform.position;
        direction = ((Vector2)player.transform.position - startPos).normalized;
        finalDestination = (Vector2)player.transform.position + direction * overshootingDistance;

        initialized = true;
    }

    private void Update()
    {
        // If the projectile hasn't been initialized, don't move it
        if (!initialized) return;
        // Move the projectile towards the final destination
        transform.position = Vector2.MoveTowards(transform.position, finalDestination, speed * Time.deltaTime);

        // Destroy the projectile if it reaches the final destination
        if (((Vector2)transform.position - finalDestination).sqrMagnitude < 0.0001f)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyProjectileAfterTime()
    {
        yield return new WaitForSeconds(5f);
        knockback = false; // Reset knockback to false after the projectile is destroyed
        Destroy(gameObject);
    }
}