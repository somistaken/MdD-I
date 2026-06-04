using UnityEngine;

public class ChimneyHandler : MonoBehaviour, IInteractable
{
    [Header("notes for wincon")]
    [SerializeField] private string[] requiredNoteIDs;

    [Header("Final Event Setup")]
    [SerializeField] private ExitDoor mainExitDoor;
    [SerializeField] private EnemyAI pharLapAI;

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

            foreach (string noteID in requiredNoteIDs)
            {
                PlayerInventory.GetInstance().RemoveItem(noteID);
            }
        }
        else
        {
            if (FeedbackUI.GetInstance() != null)
            {
                FeedbackUI.GetInstance().ShowMessage("Aún faltan encontrar algunas notas...");
            }
        }
    }
}