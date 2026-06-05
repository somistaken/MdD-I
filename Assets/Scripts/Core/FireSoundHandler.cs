using System.Collections;
using UnityEngine;

public class FireSoundHandler : MonoBehaviour
{
    private AudioSource fireSound;
    [SerializeField] private float fireDistance;
    [SerializeField] private float fireTime;

    private void Start()
    {
        fireSound = GetComponent<AudioSource>();
    }

    public void StartFire()
    {
        StartCoroutine(IncreaseFireSound(fireSound.maxDistance, fireDistance, fireTime));
    }
    public IEnumerator IncreaseFireSound(float oldValue, float newValue, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            fireSound.maxDistance = Mathf.Lerp(oldValue, newValue, t / duration);
            yield return null;
        }

        fireSound.maxDistance = newValue;
    }
}
