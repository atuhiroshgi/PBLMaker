using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class TimeSetter : MonoBehaviour
{
    [SerializeField, Header("�c��b����\������e�L�X�g")]
    private TextMeshProUGUI countDownText;

    [SerializeField, Header("�J�E���g�_�E������(�b)")]
    private int countDownTime = 10;

    private void Start()
    {
        // �J�E���g�_�E���������J�n
        StartCountDownAsync().Forget();
    }

    private async UniTaskVoid StartCountDownAsync()
    {
        // ���s�\��ԂɂȂ�܂őҋ@
        await UniTask.WaitUntil(() => ExecuteManager.Instance.GetIsExecute());

        // �J�E���g�_�E�����J�n
        await StartCountDown();
    }

    private async UniTask StartCountDown()
    {
        int remainingTime = countDownTime;

        while (remainingTime > 0)
        {
            // �c�莞�Ԃ��e�L�X�g�ɕ\��
            countDownText.text = $"{remainingTime}";

            // 1�b�ҋ@
            await UniTask.Delay(1000);

            // �c�莞�Ԃ����炷
            remainingTime--;
        }

        // �J�E���g�_�E���I�����̏����i�K�v�ɉ����Ēǉ��j
        countDownText.text = "0";
    }
}
