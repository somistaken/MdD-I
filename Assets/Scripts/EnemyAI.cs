using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatisPlayer;

    //Patrullaje
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Intento de captura
    public float timeBetweenTries;
    bool alreadyTriedCapture;

    //Estados
    public float sightRange, captureRange;
    public bool playerInSightRange, playerInCaptureRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Chequeo de vision y rango
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
        playerInCaptureRange = Physics.CheckSphere(transform.position, captureRange, whatisPlayer);

        if (!playerInSightRange &&  !playerInCaptureRange) Patrolling();
        if (playerInSightRange && !playerInCaptureRange) ChasePlayer();
        if (playerInSightRange && playerInCaptureRange) CapturePlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Se llego al punto deseado
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void CapturePlayer()
    {
        //Nos aseguramos que el enemigo no se mueva cuando trata de capturar
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyTriedCapture)
        {
            //Agreguese codigo del flavor, llamar animaciones, etc.
            alreadyTriedCapture = true;
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
    }
}
