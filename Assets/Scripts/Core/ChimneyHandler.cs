using UnityEngine;
using UnityEngine.Audio;
using System;

public class ChimneyHandler : MonoBehaviour, IInteractable
{
    public static event Action OnFinalSequenceTriggered;

    [Header("Notes for Wincon")]
    [SerializeField] private string[] requiredNoteIDs;

    [Header("Final Event Setup")]
    [SerializeField] private ExitDoor mainExitDoor;
    [SerializeField] private EnemyAI pharLapAI;
    [SerializeField] private AudioMixerSnapshot endGame;

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
            if (mainExitDoor != null) mainExitDoor.Unlock();
            if (pharLapAI != null) pharLapAI.TriggerFinalChase();

            if (fireSoundHandler != null) fireSoundHandler.StartFire();
            if (endGame != null) endGame.TransitionTo(2f);

            AudioManager.GetInstance().PlaySound(AudioManager.SoundType.notesBurned);
            AudioManager.GetInstance().PlaySound(AudioManager.SoundType.dialogueHouseOnFire);

            OnFinalSequenceTriggered?.Invoke();

            foreach (string noteID in requiredNoteIDs)
            {
                PlayerInventory.GetInstance().RemoveItem(noteID);
            }

            UIMessage = "¡El caballo se enojo! Tengo que correr...";
        }
        else
        {
            if (FeedbackUI.GetInstance() != null)
            {
                FeedbackUI.GetInstance().ShowMessage(UIMessage);
            }
        }
    }
}