using UnityEngine;

public class InteractableTest : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Interact()
    {
        Debug.Log(Random.Range(0, 100));
    }
}
