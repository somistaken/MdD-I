using UnityEngine;

public class OwlAlert : MonoBehaviour
{
    [Header("Alert Settings")]
    [SerializeField] private EnemyAI enemyToAlert;
    [SerializeField] private string playerTag = "Player";

    [Header("Optional")]
    [SerializeField] private bool destroyAfterAlert = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (AudioManager.GetInstance() != null)
            {
                AudioManager.GetInstance().PlaySound(AudioManager.SoundType.owlAlert);
            }

            enemyToAlert.ReceiveAlert(transform.position);

            if (destroyAfterAlert)
            {
                gameObject.SetActive(false);
            }
        }
    }
}