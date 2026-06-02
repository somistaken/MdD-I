using UnityEngine;

public class ChimneyHandler : MonoBehaviour, IInteractable
{
    [Header("notes for wincon")]
    [SerializeField] private string[] requiredNoteIDs;

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
            Debug.Log("won"); // Esto lo cambiamos después para la secuencia final

            foreach (string noteID in requiredNoteIDs)
            {
                PlayerInventory.GetInstance().RemoveItem(noteID);
            }
        }
        else
        {
            // LLAMAMOS A LA UI EN LUGAR DEL DEBUG.LOG
            if (FeedbackUI.GetInstance() != null)
            {
                FeedbackUI.GetInstance().ShowMessage("Aún faltan notas por encontrar...");
            }
        }
    }
}