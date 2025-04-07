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

    [Header("Wall")]
    [SerializeField] private List<GameObject> wallObjects;
    public float wallDetectRadius = 5f;
    public LayerMask wall;

    private int currentIndex = 0;
    private bool isChasing = false;
    private float chaseTimer = 0f;

    private bool wallsActivated = false;

    void Update()
    {
        if (player == null || waypoints.Count == 0) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (!isChasing && distanceToPlayer <= detectRadius)
        {
            isChasing = true;
            chaseTimer = 0f;
            ActivateNearbyWalls();
        }

        if (isChasing)
        {
            ChasePlayer();
            chaseTimer += Time.deltaTime;

            if (chaseTimer >= maxChaseTime || distanceToPlayer > detectRadius * 2f)
            {
                isChasing = false;
                DeactivateWalls();
            }
        }
        else
        {
            EnemyWalk();
        }
    }

    void EnemyWalk()
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

    void ActivateNearbyWalls()
    {
        if (wallsActivated) return;

        foreach (GameObject wall in wallObjects)
        {
            if (wall != null)
            {
                float distanceToPlayer = Vector3.Distance(wall.transform.position, player.position);
                if (distanceToPlayer <= wallDetectRadius)
                {
                    wall.SetActive(true);
                    Debug.Log("Activated wall near player: " + wall.name);
                }
            }
        }

        wallsActivated = true;
        Debug.Log("Walls activated around the player!");
    }

    void DeactivateWalls()
    {
        foreach (GameObject wall in wallObjects)
        {
            if (wall != null)
            {
                wall.SetActive(false);
            }
        }

        wallsActivated = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
