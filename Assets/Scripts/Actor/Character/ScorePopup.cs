using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [SerializeField, Header("�㏸���x")]
    private float riseSpeed = 1.0f;
    [SerializeField, Header("�t�F�[�h�A�E�g����")]
    private float fadeOutTime = 0.5f;

    private float duration;
    private TextMeshPro text;

    private void Awake()
    {
    }

    public void Initialize(int score, float displayDuration)
    {
        duration = displayDuration;
        text = GetComponent<TextMeshPro>();

        // �X�R�A��\��
        if(text != null)
        {
            text.text = score.ToString();
        }

        // �n�_����ɃI�t�Z�b�g
        transform.position += Vector3.up;

        // UniTask�Ŕ񓯊��������J�n
        HandlePopupLifecycleAsync().Forget();
    }

    private async UniTaskVoid HandlePopupLifecycleAsync()
    {
        float elapsedTime = 0f;

        // �\���̎������Ԓ����[�v
        while (elapsedTime < duration)
        {
            // �㏸����
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        //�I�u�W�F�N�g��j��
        Destroy(gameObject);
    }
}
