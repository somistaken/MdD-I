using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemData;
    private AudioSource soundEffect;

    private void Awake()
    {
        soundEffect = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        soundEffect.Play();

        if (NoteReaderUI.GetInstance() != null)
        {
            NoteReaderUI.GetInstance().OpenNote(itemData);
        }

        PlayerInventory.GetInstance().AddItem(itemData);

        Destroy(gameObject);
    }
}