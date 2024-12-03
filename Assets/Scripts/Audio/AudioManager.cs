using UnityEngine;

public abstract class AudioManager : MonoBehaviour
{
    // ���ʂ̉��ʃv���p�e�B
    private float masterVolume = 1.0f;

    public float MasterVolume
    {
        get => masterVolume;
        set
        {
            masterVolume = Mathf.Clamp01(value);
            OnVolumeChanged(masterVolume);
        }
    }

    /// <summary>
    /// ���ʂ��ύX���ꂽ���ɌĂ΂�郁�\�b�h(�C���^�[�t�F�[�X)
    /// </summary>
    /// <param name="newVolume">�ύX��̉���</param>
    ///
    public abstract void OnVolumeChanged(float newVolume);

    /// <summary>
    /// AudioClip���w�肵�Ď~�߂郁�\�b�h(�C���^�[�t�F�[�X)
    /// </summary>
    /// <param name="audioClip"></param>
    public abstract void Stop(AudioClip audioClip);

    protected void InitializeSingleton<T>(ref T instance) where T : AudioManager
    {
        if (instance == null)
        {
            instance = (T)this; // �L���X�g���đ��
            DontDestroyOnLoad(this.gameObject);
        }
        else if (!ReferenceEquals(instance, this)) // �Q�Ɣ�r���g�p
        {
            Destroy(gameObject);
        }
    }
}
