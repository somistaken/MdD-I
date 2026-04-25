using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Transform safeZoneSpawnPoint;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stareSound;

    [Header("Movement Speeds")]
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float chaseSpeed;

    //Patrullaje
    [Header("Patrol Settings")]
    [SerializeField] private float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet;

    //Intento de captura
    [Header("Capture Settings")]
    [SerializeField] private float timeBetweenTries;
    [SerializeField] private int maxCaptures = 3;
    private int currentCaptures = 0;
    private bool alreadyTriedCapture;

    [Header("Detection Ranges")]
    [SerializeField] private float sightRange;
    [SerializeField] private float captureRange;
    [Tooltip("Eye height for raycast")]
    [SerializeField] private float eyeHeightOffset;

    [Header("Detection Timers")]
    [SerializeField] private float timeToSpotPlayer;
    private float currentSpotTimer;
    private bool hasSpottedPlayer;

    //Sistema de memoria
    [Header("Investigation Settings")]
    [SerializeField] private float investigateTime;
    private Vector3 lastKnownPosition;
    private bool isInvestigating;
    private float currentInvestigateTimer;

    //Estados
    private bool playerInSightRange;
    private bool playerInCaptureRange;
    private bool hasPlayedStareSound;
    public bool isPlayerInSafeZone;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isPlayerInSafeZone)
        {
            if (hasSpottedPlayer || isInvestigating)
            {
                walkPointSet = false;
                agent.ResetPath();
            }

            playerInSightRange = false;
            playerInCaptureRange = false;
            hasSpottedPlayer = false;
            isInvestigating = false;
            currentSpotTimer = 0f;
            currentInvestigateTimer = 0f;
            hasPlayedStareSound = false;

            Patrolling();
            return;
        }

        playerInSightRange = CheckLineOfSight(sightRange);
        playerInCaptureRange = CheckLineOfSight(captureRange);

        //Si el player esta en rango de captura cerca no espera
        if (playerInCaptureRange)
        {
            hasSpottedPlayer = true;
            isInvestigating = false;
        }

        //Ver al player
        else if (playerInSightRange)
        {
            if (isInvestigating)
            {
                hasSpottedPlayer = true;
                isInvestigating = false;
                lastKnownPosition = player.position;
            }
            else
            {
                if (!hasPlayedStareSound)
                {
                    audioSource.PlayOneShot(stareSound);
                    hasPlayedStareSound = true;
                }

                currentSpotTimer += Time.deltaTime;

                if (currentSpotTimer >= timeToSpotPlayer)
                {
                    hasSpottedPlayer = true;
                    lastKnownPosition = player.position;
                }
            }
        }

        //El player se escondio
        else
        {
            currentSpotTimer = 0f;

            if (!hasSpottedPlayer && !isInvestigating)
            {
                hasPlayedStareSound = false;
            }

            if (hasSpottedPlayer && !isInvestigating)
            {
                isInvestigating = true;
                lastKnownPosition = player.position;
            }
        }

        if (hasSpottedPlayer && playerInCaptureRange)
        {
            CapturePlayer();
        }
        else if (hasSpottedPlayer && !isInvestigating)
        {
            ChasePlayer();
            if (playerInSightRange) lastKnownPosition = player.position;
        }
        else if (isInvestigating)
        {
            Investigate();
        }
        else
        {
            if (currentSpotTimer > 0)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;

                Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
                transform.LookAt(lookPos);
            }
            else
            {
                Patrolling();
            }
        }

        
    }

    private bool CheckLineOfSight(float range)
    {
        // Dentro de la esfera?
        if (Physics.CheckSphere(transform.position, range, whatIsPlayer))
        {
            //raycast
            Vector3 origin = transform.position + Vector3.up * eyeHeightOffset;
            Vector3 target = player.position + Vector3.up * eyeHeightOffset;
            Vector3 direction = target - origin;

            float distanceToPlayer = direction.magnitude;

            //actual check
            if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distanceToPlayer, whatIsGround))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    private void Patrolling()
    {
        agent.speed = patrolSpeed;

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);

            if (agent.pathStatus == NavMeshPathStatus.PathPartial || agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                walkPointSet = false;
                return;
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                walkPointSet = false;
            }
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, NavMesh.AllAreas))
        {   
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    private void CapturePlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 lookPosition = player.position;
        lookPosition.y = transform.position.y;
        transform.LookAt(lookPosition);

        if (!alreadyTriedCapture)
        {
            alreadyTriedCapture = true;
            currentCaptures++;
            if (currentCaptures >= maxCaptures)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                SendPlayerToSafeZone();
            }
        }
    }

    private void ResetCapture()
    {
        alreadyTriedCapture = false;
    }

    private void SendPlayerToSafeZone()
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.position = safeZoneSpawnPoint.position;

        if (cc != null) cc.enabled = true;

        hasSpottedPlayer = false;
        isInvestigating = false;
        currentSpotTimer = 0f;

        Invoke(nameof(ResetCapture), timeBetweenTries);
    }

    //Sistema de memoria aka recordar la ultima pos del player
    private void Investigate()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(lastKnownPosition);

        
        Vector3 distanceToPoint = transform.position - lastKnownPosition;
        distanceToPoint.y = 0;

        
        if (distanceToPoint.sqrMagnitude < 2f)
        {
            agent.ResetPath();

            currentInvestigateTimer += Time.deltaTime;

            if (currentInvestigateTimer >= investigateTime)
            {
                hasSpottedPlayer = false;
                isInvestigating = false;
                currentInvestigateTimer = 0f;

                hasPlayedStareSound = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, captureRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up * eyeHeightOffset, transform.forward * sightRange);
    }
}
