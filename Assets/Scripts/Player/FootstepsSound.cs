using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FootstepsSound : MonoBehaviour
{
    private AudioSource audioSource;
    private CharacterController characterController;
    private bool isMoving;
    [SerializeField] float runningSpeedPitch;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        IsMoving();
        AudioHandler();
    }

    private void AudioHandler()
    {
        if (isMoving && !audioSource.isPlaying) audioSource.Play();

        if (!isMoving) audioSource.Stop();

        if (PlayerInputHandler.instance.SprintTriggered) audioSource.pitch = runningSpeedPitch;
        else audioSource.pitch = 1f;
    }

    private void IsMoving()
    {
        if (PlayerInputHandler.instance.MovementInput != Vector2.zero && characterController.isGrounded)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }
}
