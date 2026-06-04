using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] AudioSource footsteps;
    [SerializeField] AudioClip walkingClip;
    [SerializeField] AudioClip runningClip;
    private CharacterController characterController;
    public static Footsteps instance { get; private set; }

    private void Start()
    {
        instance = this;
        characterController = GetComponent<CharacterController>();
        footsteps.clip = walkingClip;
    }

    private void Update()
    {
        ToggleRun();
        FootstepsEnabler();
    }

    public void FootstepsEnabler()
    {
        if (!characterController.isGrounded || characterController.velocity == Vector3.zero)
        {
            footsteps.enabled = false;
        }
        else
        {
            footsteps.enabled = true;
        }
    }

    public void ToggleRun()
    {
        if (playerInputHandler.SprintTriggered)
        {
            footsteps.clip = runningClip;
        }
        else
        {
            footsteps.clip = walkingClip;
        }
    }
}
