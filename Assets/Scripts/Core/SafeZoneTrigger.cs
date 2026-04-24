using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemyAI != null)
            {
                enemyAI.isPlayerInSafeZone = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemyAI != null)
            {
                enemyAI.isPlayerInSafeZone = false;
            }
        }
    }
}