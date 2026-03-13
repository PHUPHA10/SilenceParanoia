using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent ai;
    public Transform player;
    public Animator animator;

    [Header("Patrol")]
    public List<Transform> destinations = new List<Transform>();

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float chaseSpeed = 5f;

    [Header("Detection")]
    public float sightDistance = 10f;
    public float catchDistance = 1.5f;
    public Vector3 rayCastOffset = new Vector3(0, 1.5f, 0);

    [Header("Kill Control")]
    public MonoBehaviour[] playerDisableOnKill;
    public Camera playerCamera;

    bool walking = true;
    bool chasing = false;
    bool killing = false;

    Transform currentDest;
    int randNum;
    Transform chaseTarget;
    [Header("Kill Audio")]
    public AudioSource gunAudio;
    public AudioClip gunShotClip;


    void Start()
    {
        if (ai == null)
            ai = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (destinations != null && destinations.Count > 0)
        {
            randNum = Random.Range(0, destinations.Count);
            currentDest = destinations[randNum];
        }

        ai.speed = walkSpeed;
        ai.isStopped = false;

        walking = true;
        chasing = false;
        killing = false;

        chaseTarget = player;
    }

    void Update()
    {
        if (killing) return;

        HandleVision();

        if (chasing)
            ChasePlayer();
        else if (walking)
            Patrol();
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
            if (hit.collider.transform.root.CompareTag("Player"))
            {
                chaseTarget = player;
                StartChase();
            }
        }
    }

    void StartChase()
    {
        if (chasing || killing) return;

        chasing = true;
        walking = false;

        ai.isStopped = false;
        ai.speed = chaseSpeed;
    }

    void ChasePlayer()
    {
        if (ai == null || chaseTarget == null) return;

        ai.destination = chaseTarget.position;

        float distance = Vector3.Distance(
            ai.transform.position,
            chaseTarget.position
        );

        if (distance <= catchDistance)
        {
            KillPlayer();
        }
    }

    public void PlayGunShot()
    {
        if (gunAudio != null && gunShotClip != null)
        {
            gunAudio.pitch = Random.Range(0.95f, 1.05f);
            gunAudio.PlayOneShot(gunShotClip);
        }
    }

    void KillPlayer()
    {
        if (killing) return;
        killing = true;

        chasing = false;
        walking = false;


        if (ai != null)
        {
            ai.isStopped = true;
            ai.enabled = false;
        }

        if (player == null) return;


        foreach (var comp in playerDisableOnKill)
        {
            if (comp != null)
                comp.enabled = false;
        }

        if (playerCamera != null)
        {
            playerCamera.transform.localRotation = Quaternion.identity;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        float killDistance = 0.8f;
        Vector3 dir = (transform.position - player.position).normalized;
        dir.y = 0f;

        transform.position = player.position + dir * killDistance;

        transform.LookAt(new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        ));

        if (animator != null)
        {
            animator.Play("Action_gun", 0, 0f);
        }

        Debug.Log("PLAYER KILLED");
    }


    void Patrol()
    {
        if (ai == null || destinations == null || destinations.Count == 0)
            return;

        if (currentDest == null)
        {
            randNum = Random.Range(0, destinations.Count);
            currentDest = destinations[randNum];
        }

        ai.isStopped = false;
        ai.speed = walkSpeed;
        ai.destination = currentDest.position;

        if (!ai.pathPending && ai.remainingDistance <= ai.stoppingDistance)
        {
            randNum = Random.Range(0, destinations.Count);
            currentDest = destinations[randNum];
        }
    }


    public void CatchPlayerInHiding(Transform hidingTarget)
    {
        if (killing) return;

        chaseTarget = hidingTarget != null ? hidingTarget : player;
        StartChase();
    }
}
