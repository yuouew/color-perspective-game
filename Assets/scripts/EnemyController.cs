using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> waypoints;
    public float walkingSpeed = 2f;
    public bool isLooping = true;

    [Header("Chase")]
    public float chaseSpeed = 4f;
    public float detectRadius = 5f;
    public float maxChaseTime = 5f;
    public Transform player;

    private int currentIndex = 0;
    private bool isChasing = false;
    private float chaseTimer = 0f;

    void Update()
    {
        if (player == null || waypoints.Count == 0) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distanceToPlayer <= detectRadius)
        {
            isChasing = true;
            chaseTimer = 0f;
        }

        if (isChasing)
        {
            ChasePlayer();
            chaseTimer += Time.deltaTime;

            if (chaseTimer >= maxChaseTime || distanceToPlayer > detectRadius * 2f)
            {
                isChasing = false;
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Vector3 target = waypoints[currentIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, target, walkingSpeed * Time.deltaTime);
        transform.forward = (target - transform.position).normalized;

        if (Vector3.Distance(transform.position, target) <= 0.1f)
        {
            if (currentIndex < waypoints.Count - 1)
            {
                currentIndex++;
            }
            else if (isLooping)
            {
                currentIndex = 0;
            }
        }
    }

    void ChasePlayer()
    {
        Vector3 target = player.position;
        target.y = transform.position.y; // keeps height unchanged

        transform.position = Vector3.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);
        transform.forward = (target - transform.position).normalized;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
