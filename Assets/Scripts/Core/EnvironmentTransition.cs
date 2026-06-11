using UnityEngine;
using System.Collections;

public class EnvironmentTransition : MonoBehaviour
{
    [Header("Configuración de Niebla")]
    [Tooltip("Densidad de la niebla cuando estás afuera")]
    public float outsideFogDensity = 0.05f;
    [Tooltip("Densidad de la niebla cuando estás adentro")]
    public float insideFogDensity = 0.0f;

    [Header("Configuración de Audio")]
    [Tooltip("El AudioSource que reproduce el ambiente exterior")]
    public AudioSource ambientAudioSource;
    [Tooltip("Volumen del ambiente estando afuera")]
    public float outsideVolume = 1.0f;
    [Tooltip("Volumen del ambiente estando adentro (0 para silencio total)")]
    public float insideVolume = 0.2f;

    [Header("Ajustes de Transición")]
    [Tooltip("Tiempo en segundos que tarda en hacer el cambio completo")]
    public float transitionDuration = 2.0f;

    private Coroutine currentTransition;


    void Start()
    {
        RenderSettings.fog = true;

        RenderSettings.fogDensity = outsideFogDensity;
        ambientAudioSource.volume = outsideVolume;
    }

    private void Update()
    {
        Debug.Log("Densidad de niebla actual: " + RenderSettings.fogDensity);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("¡ALGO DETECTADO!: " + other.name + " con el Tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            if (currentTransition != null) StopCoroutine(currentTransition);
            currentTransition = StartCoroutine(TransitionEnvironment(insideFogDensity, insideVolume));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentTransition != null) StopCoroutine(currentTransition);
            currentTransition = StartCoroutine(TransitionEnvironment(outsideFogDensity, outsideVolume));
        }
    }

    private IEnumerator TransitionEnvironment(float targetFog, float targetVolume)
    {
        float startFog = RenderSettings.fogDensity;

        float startVolume = 0f;
        if (ambientAudioSource != null)
        {
            startVolume = ambientAudioSource.volume;
        }

        float timeElapsed = 0;

        while (timeElapsed < transitionDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / transitionDuration;

            RenderSettings.fogDensity = Mathf.Lerp(startFog, targetFog, t);

            if (ambientAudioSource != null)
            {
                ambientAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            }

            yield return null;
        }

        RenderSettings.fogDensity = targetFog;
        if (ambientAudioSource != null) ambientAudioSource.volume = targetVolume;
    }
}