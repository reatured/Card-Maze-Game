using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 offset;
    public Transform player;
    public float movingSpeed = 0.5f; 
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position; 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, movingSpeed * Time.deltaTime);
    }
}
