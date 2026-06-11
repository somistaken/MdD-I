using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour, IInteractable
{
    private Animator doorAnim;
    private AudioSource doorSound;
    private bool doorIsOpen;
    private bool openedInward;
    private int doorStuckThreshold;
    private int doorAttempts;
    private NavMeshObstacle navObstacle;

    [Header("Animation State Names")]
    [SerializeField] private string animIdle = "Idle";
    [SerializeField] private string animOpenInward = "DoorOpenInward";
    [SerializeField] private string animOpenOutward = "DoorOpenOutward";
    [SerializeField] private string animCloseInward = "DoorCloseInward";
    [SerializeField] private string animCloseOutward = "DoorCloseOutward";

    private void Awake()
    {
        doorIsOpen = false;
        doorAttempts = 0;
        doorStuckThreshold = 5;
        doorAnim = GetComponent<Animator>();
        doorSound = GetComponent<AudioSource>();

        navObstacle = GetComponent<NavMeshObstacle>();
        if (navObstacle != null) navObstacle.carving = true;
    }

    public void Interact()
    {
        AnimatorStateInfo stateInfo = doorAnim.GetCurrentAnimatorStateInfo(0);

        if (doorAnim.IsInTransition(0) || (!stateInfo.IsName("Idle") && stateInfo.normalizedTime < 1f))
        {
            return;
        }

        if (doorAttempts > doorStuckThreshold)
        {
            if (PlayerInventory.GetInstance().IsItemInInventory("oil"))
            {
                doorAttempts = 0;
                PlayerInventory.GetInstance().RemoveItem("oil");
            }
            else
            {
                FeedbackUI.GetInstance().ShowMessage("La puerta está trabada. Necesito aceite...");
            }
            return;
        }

        doorSound.Play();

        doorIsOpen = !doorIsOpen;

        if (doorIsOpen)
        {
            if (navObstacle != null) navObstacle.carving = false;

            Vector3 directionToPlayer = (Camera.main.transform.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.right, directionToPlayer);

            if (dotProduct > 0)
            {
                doorAnim.Play("OpenInward");
                openedInward = true;
            }
            else
            {
                doorAnim.Play("OpenOutward");
                openedInward = false;
            }
        }
        else
        {
            if (navObstacle != null) navObstacle.carving = true;

            if (openedInward)
            {
                doorAnim.Play("CloseInward");
            }
            else
            {
                doorAnim.Play("CloseOutward");
            }
        }

        doorAttempts++;
    }
}