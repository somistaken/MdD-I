using System;
using UnityEngine;

public class LampToggle : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private Light lampLight;

    private const string lamp = "lamp";

    private void Awake()
    {
        lampLight.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInputHandler.LampToggle() && PlayerInventory.GetInstance().IsItemInInventory(lamp))
        {
            ToggleLamp();
        }
    }

    private void ToggleLamp()
    {
        AudioManager.GetInstance().PlaySound(AudioManager.SoundType.lampToggle);
        if (lampLight.gameObject.activeSelf)
        {
            lampLight.gameObject.SetActive(false);
        }
        else
        {
            lampLight.gameObject.SetActive(true);
        }
    }
}
