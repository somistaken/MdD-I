using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        lampToggle,
        itemPickup,
        notePickup,
        notesBurned,
        dialogueHouseOnFire,
        dialogueRespawn1,
        dialogueRespawn2,
        dialogueMainDoor,
        dialogueSafeRoom1,
        dialogueSafeRoom2,
        // agregar mas sonidos que hagan falta
    }

    [System.Serializable]
    public class Sound
    {
        public SoundType type;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        public bool isDialogue;
        [HideInInspector]
        public AudioSource source;
    }

    private static AudioManager instance;
    [SerializeField] private AudioMixerGroup sfxGroup;
    [SerializeField] private AudioMixerGroup voiceGroup;
    [SerializeField] private Sound[] allSounds;

    private Dictionary<SoundType, Sound> soundDict = new Dictionary<SoundType, Sound>();

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

        if (s.isDialogue) audioSrc.outputAudioMixerGroup = voiceGroup;
        else audioSrc.outputAudioMixerGroup = sfxGroup;

        audioSrc.Play();

        Destroy(soundObject, s.clip.length);
    }
}
