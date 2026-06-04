using UnityEngine;
using UnityEngine.Audio;

public class SafeZoneTrigger : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private AudioMixerSnapshot general;
    [SerializeField] private AudioMixerSnapshot safeRoom;
    [SerializeField] private float musicFade;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            safeRoom.TransitionTo(musicFade);
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
            general.TransitionTo(musicFade);
            if (enemyAI != null)
            {
                enemyAI.isPlayerInSafeZone = false;
            }
        }
    }
}