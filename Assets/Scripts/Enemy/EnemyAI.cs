using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.VirtualTexturing;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
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

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
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
            hasPlayedStareSound = false;

            if (hasSpottedPlayer && !isInvestigating)
            {
                isInvestigating = true;
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
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Se llego al punto deseado
        if (distanceToWalkPoint.sqrMagnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calcular punto random para desplazarse
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
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
        //Nos aseguramos que el enemigo no se mueva cuando trata de capturar
        agent.SetDestination(transform.position);

        Vector3 lookPosition = player.position;
        lookPosition.y = transform.position.y;
        transform.LookAt(lookPosition);

        if (!alreadyTriedCapture)
        {
            alreadyTriedCapture = true;
            //Agreguese codigo del flavor, llamar animaciones, etc.
            Invoke(nameof(ResetCapture), timeBetweenTries);
        }
    }

    private void ResetCapture()
    {
        alreadyTriedCapture = false;
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
