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
            Debug.Log("won");

            foreach (string noteID in requiredNoteIDs)
            {
                PlayerInventory.GetInstance().RemoveItem(noteID);
            }

        }
        else
        {
            Debug.Log("notes left around");
        }
    }
}