using System;
using UnityEngine;

public class LampToggle : MonoBehaviour
{
    [Header("InputHandler")]
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
            Debug.Log("lamp should work");
        }
    }

    private void ToggleLamp()
    {
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
