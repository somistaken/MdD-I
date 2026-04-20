using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;

    //Patrullaje
    [Header("Patrol Settings")]
    [SerializeField] private float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet;

    //Intento de captura
    [Header("Capture Settings")]
    [SerializeField] private float timeBetweenTries;
    private bool alreadyTriedCapture;

    //Estados
    [Header("Detection Ranges")]
    [SerializeField] private float sightRange;
    [SerializeField] private float captureRange;
    [Tooltip("Eye height for raycast")]
    [SerializeField] private float eyeHeightOffset = 1.5f;

    private bool playerInSightRange;
    private bool playerInCaptureRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Chequeo de vision y rango
        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInCaptureRange = Physics.CheckSphere(transform.position, captureRange, whatIsPlayer);
        playerInSightRange = CheckLineOfSight(sightRange);
        playerInCaptureRange = CheckLineOfSight(captureRange);

        if (!playerInSightRange &&  !playerInCaptureRange) Patrolling();
        if (playerInSightRange && !playerInCaptureRange) ChasePlayer();
        if (playerInSightRange && playerInCaptureRange) CapturePlayer();
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

            //actual check
            if (Physics.Raycast(origin, direction, out RaycastHit hit, range, whatIsGround))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    private void Patrolling()
    {
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
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
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
