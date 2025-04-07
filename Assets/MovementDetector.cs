using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDetector : MonoBehaviour
{
    private Rigidbody rb;
    public bool isMoving;

    private Vector3 spawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        spawnPoint = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > 0.01f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    public void Respawn()
    {
        gameObject.transform.position = spawnPoint;
    }
}