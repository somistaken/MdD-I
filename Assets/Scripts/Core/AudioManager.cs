using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        lampToggle,
        // agregar mas sonidos que hagan falta
    }

    [System.Serializable]
    public class Sound
    {
        public SoundType type;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [HideInInspector]
        public AudioSource source;
    }

    private static AudioManager instance;

    [SerializeField] private Sound[] allSounds;

    private Dictionary<SoundType, Sound> soundDict = new Dictionary<SoundType, Sound>();
    private Dictionary<SoundType, GameObject> musicDict = new Dictionary<SoundType, GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Ya hay una instancia de audio manager en la escena");
        }

        instance = this;

        foreach (var sound in allSounds)
        {
            soundDict[sound.type] = sound;
        }
    }
    public static AudioManager GetInstance()
    {
        return instance;
    }

    public void PlaySound(SoundType type)
    {
        if (!soundDict.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sonido tipo {type} no esta en el dict");
            return;
        }

        var soundObject = new GameObject($"Sound_{type}");
        var audioSrc = soundObject.AddComponent<AudioSource>();

        audioSrc.clip = s.clip;
        audioSrc.volume = s.volume;

        audioSrc.Play();

        Destroy(soundObject, s.clip.length);
    }
}
