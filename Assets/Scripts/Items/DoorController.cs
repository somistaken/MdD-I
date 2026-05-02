using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour, IInteractable
{
    private Animator doorAnim;
    private bool doorIsOpen;
    private bool openedInward;
    private int doorStuckThreshold;
    private int doorAttempts;
    private NavMeshObstacle navObstacle;

    private void Awake()
    {
        doorIsOpen = false;
        doorAttempts = 0;
        doorStuckThreshold = 5;
        doorAnim = GetComponent<Animator>();

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
                Debug.Log("La puerta está trabada. Se necesita aceite.");
            }
            return;
        }

        doorIsOpen = !doorIsOpen;

        if (doorIsOpen)
        {
            if (navObstacle != null) navObstacle.carving = false;

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
            if (navObstacle != null) navObstacle.carving = true;

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