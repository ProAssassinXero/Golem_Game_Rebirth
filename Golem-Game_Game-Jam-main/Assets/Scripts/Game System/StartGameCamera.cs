using UnityEngine;

public class StartGameCamera : MonoBehaviour
{
    public GameObject player;
    public Camera camera;
    public bool doneZooming;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        camera.orthographicSize = 30f;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player GameObject is active and adjust the camera position accordingly
        if (player.activeSelf == true)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        }
        else if (player.activeSelf == false)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(0f, 0f, gameObject.transform.position.z), 0.5f * Time.unscaledDeltaTime);
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 30f, 0.5f * Time.unscaledDeltaTime);
        }

        // Smoothly zoom the camera in to a size of 8 when the player is active and keep the zoom at 8 when done zooming
        if (camera.orthographicSize > 8f && !doneZooming)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, 8f, 1f * Time.unscaledDeltaTime);

        }
        else if (camera.orthographicSize <= 8f || camera.orthographicSize < 8.009f)
        {
            doneZooming = true;
            camera.orthographicSize = 8f;
        }
    }
}
