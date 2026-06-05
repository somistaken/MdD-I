using System.Collections;
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

            StartCoroutine(PlaySafeRoomDialogue());
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

            StopAllCoroutines();
        }
    }

    public IEnumerator PlaySafeRoomDialogue()
    {
        yield return new WaitForSeconds(7f);

        if (enterAttempts == 0)
        {
            AudioManager.GetInstance().PlaySound(AudioManager.SoundType.dialogueSafeRoom1);
            enterAttempts++;
        }
        else if (enterAttempts == 1)
        {
            AudioManager.GetInstance().PlaySound(AudioManager.SoundType.dialogueSafeRoom2);
            enterAttempts++;
        }
    }
}