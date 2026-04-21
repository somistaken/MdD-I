using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public Sprite sprite;
    public string id;
    public string itemName;
    [TextArea] public string description;
}
