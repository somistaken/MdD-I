using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    private Animator doorAnim;
    private bool doorIsOpen = false;
    private string doorOpen = "DoorOpen";
    private string doorClose = "DoorClose";

    private void Awake()
    {
        doorAnim = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (!doorIsOpen)
        {
            doorAnim.Play(doorOpen, 0, 0.0f);
            doorIsOpen = true;
        }
        else
        {
            doorAnim.Play(doorClose, 0, 0.0f);
            doorIsOpen = false;
        }
    }
}
