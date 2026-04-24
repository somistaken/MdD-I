using UnityEngine;

public class SafeZoneTrigger : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.GetInstance().ChangeMusic(AudioManager.SoundType.musicaSafeRoom);
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
            AudioManager.GetInstance().ChangeMusic(AudioManager.SoundType.musicaGeneral);
            if (enemyAI != null)
            {
                enemyAI.isPlayerInSafeZone = false;
            }
        }
    }
}