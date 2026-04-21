using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    private Animator doorAnim;
    private bool doorIsOpen;
    private int doorStuckTreshhold;
    private int doorAttempts;
    private bool animating = false;

    private void Awake()
    {
        doorIsOpen = false;
        doorAnim = GetComponent<Animator>();
        doorAttempts = 0;
        doorStuckTreshhold = 5;
    }

    public void Interact()
    {
        // return si la puerta esta en movimiento
        if (doorAnim.GetCurrentAnimatorStateInfo(0).length > doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            return;
        }

        // despues de abrir la puerta 5 veces return hasta que tengas aceite en el inventario (hay que balancear)
        if (doorAttempts > doorStuckTreshhold)
        {
            if (PlayerInventory.GetInstance().IsItemInInventory("oil"))
            {
                doorAttempts = 0;
                PlayerInventory.GetInstance().RemoveItem("oil");
            }
            // audio de puerta trabada
            Debug.Log("Door stuck");
            return;
        }

        if (!doorIsOpen)
        {
            doorIsOpen = true;
            doorAnim.Play("DoorOpen");
        }
        else
        {
            doorIsOpen = false;
            doorAnim.Play("DoorClose");
            doorAttempts++;
        }
    }
}
