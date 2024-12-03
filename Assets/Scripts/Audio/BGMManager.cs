using UnityEngine;

public class BGMManager : AudioManager
{
    private static BGMManager instance;
    public static BGMManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = "AudioManager";
                instance = go.AddComponent<BGMManager>();
            }
            return instance;
        }
    }

    private AudioSource audioSource;

    private void Awake()
    {
        InitializeSingleton(ref instance);

        // AudioSource�̐ݒ�
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Play(AudioClip audioClip, float volume = 1.0f, float delay = 0f, float pitch = 1.0f, bool loop = true, bool allowOverlap = false)
    {
        if(!allowOverlap && audioSource.isPlaying)
        {
            // ���ݗ����Ă���BGM���~
            audioSource.Stop();
        }

        // AudioSource�̐ݒ�
        audioSource.clip = audioClip;
        audioSource.volume = volume * MasterVolume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;

        // �Đ�
        audioSource.PlayDelayed(delay);
    }

    public override void Stop(AudioClip audioClip)
    {
        if(audioSource.clip == audioClip && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public override void OnVolumeChanged(float newVolume)
    {
        if(audioSource != null)
        {
            audioSource.volume = newVolume;
        }
    }
}
