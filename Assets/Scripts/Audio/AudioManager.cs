using UnityEngine;

public abstract class AudioManager : MonoBehaviour
{
    // 共通の音量プロパティ
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
    /// 音量が変更された時に呼ばれるメソッド(インターフェース)
    /// </summary>
    /// <param name="newVolume">変更後の音量</param>
    ///
    public abstract void OnVolumeChanged(float newVolume);

    /// <summary>
    /// AudioClipを指定して止めるメソッド(インターフェース)
    /// </summary>
    /// <param name="audioClip"></param>
    public abstract void Stop(AudioClip audioClip);

    protected void InitializeSingleton<T>(ref T instance) where T : AudioManager
    {
        if (instance == null)
        {
            instance = (T)this; // キャストして代入
            DontDestroyOnLoad(this.gameObject);
        }
        else if (!ReferenceEquals(instance, this)) // 参照比較を使用
        {
            Destroy(gameObject);
        }
    }
}
