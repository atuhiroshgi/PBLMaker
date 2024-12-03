using UnityEngine;

public class SEManager : AudioManager
{
    private static SEManager instance;
    public static SEManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "SEManager";
                instance = go.AddComponent<SEManager>();
            }
            return instance;
        }
    }

    private AudioSource[] audioSources;
    private int maxSources = 20;

    private void Awake()
    {
        InitializeSingleton(ref instance);

        // AudioSourceを複数準備
        audioSources = new AudioSource[maxSources];
        for (int i = 0; i < maxSources; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public AudioSource Play(AudioClip audioClip, float volume = 1.0f, float pitch = 1.0f)
    {
        AudioSource availableSource = GetAvailableAudioSource();
        if (availableSource == null)
        {
            Debug.LogWarning("SEManager: 再生可能なAudioSourceがありません！");
            return null;
        }

        availableSource.clip = audioClip;
        availableSource.volume = volume * MasterVolume; // MasterVolumeを適用
        availableSource.pitch = pitch;
        availableSource.loop = false;
        availableSource.Play();

        return availableSource;
    }

    public override void Stop(AudioClip audioClip)
    {
        foreach(AudioSource source in audioSources)
        {
            if(source.isPlaying && source.clip == audioClip)
            {
                source.Stop();
            }
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }


    public override void OnVolumeChanged(float newVolume)
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying)
            {
                source.volume = newVolume;
            }
        }
    }
}
