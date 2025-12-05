using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public Transform player;
    public List<Transform> destinations;

    public float walkSpeed = 2f;
    public float chaseSpeed = 5f;
    public float sightDistance = 10f;
    public float catchDistance = 1.5f;
    public Vector3 rayCastOffset = new Vector3(0, 1.5f, 0);

    bool walking = true;
    bool chasing = false;

    Transform currentDest;
    int randNum;

    void Start()
    {
        if (destinations.Count > 0)
        {
            randNum = Random.Range(0, destinations.Count);
            currentDest = destinations[randNum];
        }
        walking = true;
        chasing = false;
    }

    void Update()
    {
        HandleVision();

        if (chasing)
        {
            ChasePlayer();
        }
        else if (walking)
        {
            Patrol();
        }
    }

    void HandleVision()
    {
        if (player == null) return;

        Vector3 origin = transform.position + rayCastOffset;
        Vector3 dir = (player.position - origin).normalized;

        Debug.DrawRay(origin, dir * sightDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, sightDistance))
        {
            Debug.Log("Enemy ray hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Player"))
            {
                if (!chasing)
                    Debug.Log(">>> SWITCH TO CHASE <<<");

                chasing = true;
                walking = false;
                ai.speed = chaseSpeed;
            }
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        ai.destination = player.position;

        float distance = Vector3.Distance(player.position, ai.transform.position);
        if (distance <= catchDistance)
        {
            Debug.Log("Caught player (test mode)");
            // ????????????? ????????????????????????????
            chasing = false;
            walking = false;
            ai.isStopped = true;
        }
    }

    void Patrol()
    {
        if (currentDest == null) return;

        ai.speed = walkSpeed;
        ai.destination = currentDest.position;

        if (!ai.pathPending && ai.remainingDistance <= ai.stoppingDistance)
        {
            // ????????????
            randNum = Random.Range(0, destinations.Count);
            currentDest = destinations[randNum];
        }
    }
}
