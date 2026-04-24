using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        lampToggle,
        musicSafeRoom,
        musicGeneral
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

    // singleton
    private static AudioManager instance;

    // setear sonidos y el tipo correspondiente desde inspector
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

        ChangeMusic(SoundType.musicGeneral);
    }

    // llamar al singleton
    public static AudioManager GetInstance()
    {
        return instance;
    }

    // llamar este metodo para reproducir un sonido
    public void PlaySound(SoundType type)
    {
        // error si el sonido no existe en el dict
        if (!soundDict.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sonido tipo {type} no esta en el dict");
            return;
        }

        // crea un gameObject con el sonido
        var soundObject = new GameObject($"Sound_{type}");
        var audioSrc = soundObject.AddComponent<AudioSource>();

        // asigna propiedades del sonido
        audioSrc.clip = s.clip;
        audioSrc.volume = s.volume;

        // reproducir sonido
        audioSrc.Play();

        // destruir objecto
        Destroy(soundObject, s.clip.length);
    }

    // cambia la musica general
    public void ChangeMusic(SoundType type)
    {
        // error si el sonido no existe en el dict
        if (!soundDict.TryGetValue(type, out Sound track))
        {
            Debug.LogWarning($"Musica {type} no esta en el dict");
            return;
        }

        if (!musicDict.TryGetValue(type, out GameObject musicObject))
        {
            var container = new GameObject($"Music_{type}");
            musicDict[type] = container;
            var audioSrc = container.AddComponent<AudioSource>();
            audioSrc.clip = track.clip;
            audioSrc.volume = track.volume;
            audioSrc.loop = true;
            audioSrc.Play();
        }

        foreach (var value in musicDict.Values)
        {
            value.GetComponent<AudioSource>().volume = 0f;
        }

        musicDict[type].GetComponent<AudioSource>().volume = track.volume;
    }
}
