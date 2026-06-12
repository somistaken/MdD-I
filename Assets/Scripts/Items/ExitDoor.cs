using UnityEngine;

public class ExitDoor : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Animator doorAnim;
    [SerializeField] private AudioSource doorSound;

    private bool isLocked = true;
    private bool isOpen = false;

    public void Interact()
    {
        if (isLocked)
        {
            if (FeedbackUI.GetInstance() != null)
            {
                FeedbackUI.GetInstance().ShowMessage("No puedo abandonar la mansión sin quemar las pruebas");
                AudioManager.GetInstance().PlaySound(AudioManager.SoundType.dialogueMainDoor);
            }
            return;
        }

        if (!isOpen)
        {
            isOpen = true;

            if (doorSound != null)
            {
                doorSound.Play();
            }

            if (doorAnim != null)
            {
                doorAnim.Play("DoorOpenOutward");
            }

            if (FeedbackUI.GetInstance() != null)
            {
                FeedbackUI.GetInstance().ShowMessage("¡La puerta está abierta! ¡Sálvate!");
            }
        }
    }

    public void Unlock()
    {
        isLocked = false;
    }
}