using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        lampToggle,
        musicaSafeRoom,
        musicaGeneral
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

    // audioSource para la musica
    private AudioSource musicSource;

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

    public void ChangeMusic(SoundType type)
    {
        // error si el sonido no existe en el dict
        if (!soundDict.TryGetValue(type, out Sound track))
        {
            Debug.LogWarning($"Musica {type} no esta en el dict");
            return;
        }

        if (musicSource == null)
        {
            var container = new GameObject("soundtrackObject");
            musicSource = container.AddComponent<AudioSource>();
            musicSource.loop = true;
        }

        musicSource.clip = track.clip;
        musicSource.Play();
    }
}
