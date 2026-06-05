using UnityEngine;
using UnityEngine.Audio;

public class SafeZoneTrigger : MonoBehaviour
{
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private AudioMixerSnapshot general;
    [SerializeField] private AudioMixerSnapshot safeRoom;
    [SerializeField] private float musicFade;
    private int enterAttempts;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            safeRoom.TransitionTo(musicFade);
            if (enemyAI != null)
            {
                enemyAI.isPlayerInSafeZone = true;
            }

            switch (enterAttempts)
            {
                case 0:
                    AudioManager.GetInstance().PlaySound(AudioManager.SoundType.dialogueSafeRoom1);
                    enterAttempts++;
                    break;
                case 1:
                    AudioManager.GetInstance().PlaySound(AudioManager.SoundType.dialogueSafeRoom2);
                    enterAttempts++;
                    break;
                default:
                    break;
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