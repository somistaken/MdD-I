using UnityEngine;
using System.Collections;

public class AtmosphereObserver : MonoBehaviour
{
    [Header("Configuración Visual")]
    [SerializeField] private Color nightmareColor = Color.red;
    [SerializeField] private float intensityMultiplier = 1.5f;
    [SerializeField] private float transitionDuration = 2f;

    [Tooltip("Las bombillas físicas hijas de este contenedor (opcional)")]
    [SerializeField] private MeshRenderer[] lampMeshes;

    private Light[] childLights;
    private Color[] startColors;
    private float[] startIntensities;

    private void Awake()
    {
        childLights = GetComponentsInChildren<Light>();

        startColors = new Color[childLights.Length];
        startIntensities = new float[childLights.Length];

        for (int i = 0; i < childLights.Length; i++)
        {
            startColors[i] = childLights[i].color;
            startIntensities[i] = childLights[i].intensity;
        }
    }

    private void OnEnable()
    {
        ChimneyHandler.OnFinalSequenceTriggered += StartNightmareTransition;
    }

    private void OnDisable()
    {
        ChimneyHandler.OnFinalSequenceTriggered -= StartNightmareTransition;
    }
    private void StartNightmareTransition()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            for (int i = 0; i < childLights.Length; i++)
            {
                if (childLights[i].gameObject.CompareTag("Player")) continue;

                childLights[i].color = Color.Lerp(startColors[i], nightmareColor, t);
                childLights[i].intensity = Mathf.Lerp(startIntensities[i], startIntensities[i] * intensityMultiplier, t);
            }

            if (lampMeshes != null)
            {
                foreach (MeshRenderer renderer in lampMeshes)
                {
                    if (renderer != null)
                    {
                        Color lerpedColor = Color.Lerp(Color.white, nightmareColor, t);
                        renderer.material.SetColor("_EmissionColor", lerpedColor * intensityMultiplier);
                    }
                }
            }

            yield return null;
        }
    }
}