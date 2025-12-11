using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent ai;
    public Transform player;              // ???????? (???????????)
    public List<Transform> destinations;

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("Detection")]
    public float sightDistance = 10f;
    public float catchDistance = 1.5f;
    public Vector3 rayCastOffset = new Vector3(0, 1.5f, 0);

    bool walking = true;
    bool chasing = false;

    Transform currentDest;
    int randNum;

    // ??????????? chase (???? = player, ?????????????????????????????)
    private Transform chaseTarget;

    public bool IsChasing => chasing;

    void Start()
    {
        if (destinations.Count > 0)
        {
            randNum = Random.Range(0, destinations.Count);
            currentDest = destinations[randNum];
        }

        walking = true;
        chasing = false;

        if (ai != null)
        {
            ai.speed = walkSpeed;
            ai.isStopped = false;
        }

        // ?????????????? player ????????
        chaseTarget = player;
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
        if (player == null || ai == null) return;

        Vector3 origin = transform.position + rayCastOffset;
        Vector3 dir = (player.position - origin).normalized;

        Debug.DrawRay(origin, dir * sightDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, sightDistance))
        {
            // ??? root tag "Player" ????? collider ??????????
            if (hit.collider.transform.root.CompareTag("Player"))
            {
                // ???????????? player ???? ? ? ??????? chaseTarget ????????? player
                chaseTarget = player;
                StartChase();
            }
        }
    }

    void StartChase()
    {
        if (ai == null) return;

        if (!chasing)
            Debug.Log(">>> SWITCH TO CHASE <<<");

        chasing = true;
        walking = false;
        ai.isStopped = false;
        ai.speed = chaseSpeed;
    }

    void ChasePlayer()
    {
        if (ai == null) return;

        // ???????????????? ??? fallback ???? player
        if (chaseTarget == null)
        {
            chaseTarget = player;
            if (chaseTarget == null) return;
        }

        ai.destination = chaseTarget.position;

        float distance = Vector3.Distance(chaseTarget.position, ai.transform.position);
        if (distance <= catchDistance)
        {
            Debug.Log("Enemy reached target (player / hiding spot)!");

            chasing = false;
            walking = false;
            ai.isStopped = true;

            // TODO: ??? logic ??? / jumpscare / load scene ??????
        }
    }

    void Patrol()
    {
        if (ai == null || currentDest == null) return;

        ai.isStopped = false;
        ai.speed = walkSpeed;
        ai.destination = currentDest.position;

        if (!ai.pathPending && ai.remainingDistance <= ai.stoppingDistance)
        {
            if (destinations.Count > 0)
            {
                randNum = Random.Range(0, destinations.Count);
                currentDest = destinations[randNum];
            }
        }
    }

    /// <summary>
    /// ??? script ???????????????? ?????????? patrol
    /// </summary>
    public void StopChase()
    {
        if (!chasing || ai == null) return;

        Debug.Log("Enemy stop chasing and return to patrol.");

        chasing = false;
        walking = true;
        ai.isStopped = false;
        ai.speed = walkSpeed;

        if (destinations.Count > 0)
        {
            randNum = Random.Range(0, destinations.Count);
            currentDest = destinations[randNum];
        }

        // ??????????? player ????
        chaseTarget = player;
    }

    /// <summary>
    /// ???????? QTE ?????????????? 3 ????? ????????????????
    /// target = ?????????????? / ??????????????????
    /// </summary>
    public void CatchPlayerInHiding(Transform hidingTarget)
    {
        Debug.Log("Enemy detected player while hiding -> START CHASE to hiding target!");

        // ??????????????????????? / player ?????????????????
        if (hidingTarget != null)
            chaseTarget = hidingTarget;
        else
            chaseTarget = player; // ??? null

        StartChase();
    }
}
