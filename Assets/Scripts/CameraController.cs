using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    GameObject player;

    public float lerpRate;
    public float yOffset;
    public float zOffset;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Lerp the cameras position to the players position
        Vector3 newPos;
        newPos.x = Mathf.Lerp(transform.position.x, player.transform.position.x, lerpRate);
        newPos.y = Mathf.Lerp(transform.position.y, player.transform.position.y + yOffset, lerpRate);
        newPos.z = Mathf.Lerp(transform.position.z, player.transform.position.z - zOffset, lerpRate);
        transform.position = newPos;
    }
}
