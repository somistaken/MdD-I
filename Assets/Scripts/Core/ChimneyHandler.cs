using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ChimneyHandler : MonoBehaviour, IInteractable
{
    [Header("Notes for Wincon")]
    [SerializeField] private string[] requiredNoteIDs;

    [Header("Final Event Setup")]
    [SerializeField] private ExitDoor mainExitDoor;
    [SerializeField] private EnemyAI pharLapAI;

    [Header("Atmosphere Setup")]
    [Tooltip("Los objetos padre que contienen las luces (ej: [Lighting] y Lights)")]
    [SerializeField] private Transform[] lightContainers;
    [SerializeField] private Color finalEventColor = Color.red;
    [SerializeField] private float finalEventIntensityMultiplier = 1.5f;

    private string UIMessage = "Aún faltan notas por encontrar...";
    private FireSoundHandler fireSoundHandler;

    private void Start()
    {
        fireSoundHandler = GetComponent<FireSoundHandler>();
    }
    public void Interact()
    {
        bool hasAllNotes = true;

        foreach (string noteID in requiredNoteIDs)
        {
            if (!PlayerInventory.GetInstance().IsItemInInventory(noteID))
            {
                hasAllNotes = false;
                break;
            }
        }

        if (hasAllNotes)
        {
            if (mainExitDoor != null)
            {
                mainExitDoor.Unlock();
            }

            if (pharLapAI != null)
            {
                pharLapAI.TriggerFinalChase();
            }

            TriggerRedAlertLights();

            foreach (string noteID in requiredNoteIDs)
            {
                PlayerInventory.GetInstance().RemoveItem(noteID);
            }

            UIMessage = "La casa se esta incendiando! Tengo que correr...";
        }
        else
        {
            if (FeedbackUI.GetInstance() != null)
            {
                FeedbackUI.GetInstance().ShowMessage(UIMessage);
            }
        }
    }

    private void TriggerRedAlertLights()
    {
        fireSoundHandler.StartFire();

        if (lightContainers == null || lightContainers.Length == 0) return;

        foreach (Transform container in lightContainers)
        {
            if (container == null) continue;

            Light[] lightsInContainer = container.GetComponentsInChildren<Light>();

            foreach (Light light in lightsInContainer)
            {
                if (light.gameObject.CompareTag("Player")) continue;

                light.color = finalEventColor;
                light.intensity *= finalEventIntensityMultiplier;
            }
        }

        
    }
}