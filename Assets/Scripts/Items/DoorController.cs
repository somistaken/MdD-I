using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    private Animator doorAnim;
    private bool doorIsOpen;
    private bool openedInward;
    private int doorStuckThreshold;
    private int doorAttempts;

    private void Awake()
    {
        doorIsOpen = false;
        doorAttempts = 0;
        doorStuckThreshold = 5;
        doorAnim = GetComponent<Animator>();
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
                Debug.Log("La puerta está trabada. Se necesita aceite.");
            }
            return;
        }

        doorIsOpen = !doorIsOpen;

        if (doorIsOpen)
        {
            Vector3 directionToPlayer = (Camera.main.transform.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(transform.right, directionToPlayer);

            if (dotProduct > 0)
            {
                doorAnim.Play("DoorOpenInward");
                openedInward = true;
            }
            else
            {
                doorAnim.Play("DoorOpenOutward");
                openedInward = false;
            }
        }
        else
        {
            if (openedInward)
            {
                doorAnim.Play("DoorCloseInward");
            }
            else
            {
                doorAnim.Play("DoorCloseOutward");
            }
        }

        doorAttempts++;
    }
}