using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    private Animator doorAnim;
    private bool doorIsOpen;
    private int doorStuckTreshhold;
    private int doorAttempts;

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
            Debug.Log("Door is stuck");
            return;
        }

        if (!doorIsOpen)
        {
            doorIsOpen = true;
            doorAnim.Play("DoorOpen");
            //StartCoroutine(DisableCollision());
        }
        else
        {
            doorIsOpen = false;
            doorAnim.Play("DoorClose");
            doorAttempts++;
            //StartCoroutine(DisableCollision());
        }
    }

    // quiero desactivar la colision cuando la puerta esta en movimiento
    // de esta manera el pj se queda trabado si estas en el lugar de la puerta cuando la colision se activa de nuevo
    //private IEnumerator DisableCollision()
    //{
    //    gameObject.GetComponent<Collider>().enabled = false;

    //    yield return new WaitForSeconds(1f);

    //    gameObject.GetComponent<Collider>().enabled = true;
    //}
}
