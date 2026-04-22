using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    private Image image;
    private Button button;

    public void Initialize(ItemSO item)
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        image.sprite = item.sprite;
        transform.localScale = Vector3.one;
    }
}
