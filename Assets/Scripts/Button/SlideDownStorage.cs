using UnityEngine;
using Cysharp.Threading.Tasks;

public class SlideDownStorage : Button
{
    [SerializeField, Header("BlockStorage���Q��")]
    private GameObject blockStorage;
    [SerializeField, Header("�X���C�h�ɂ����鎞��")]
    private float slideDuration = 0.5f;

    private bool isSliding = false;

    protected override void OnClick()
    {
        base.OnClick();

        if (blockStorage != null && !isSliding)
        {
            // �X���C�h��Ԃ�
            SlideToTargetPositionAsync(blockStorage, slideDuration).Forget();
        }
    }

    /// <summary>
    /// UniTask�Ŕ񓯊��ɃX���C�h�������s��
    /// </summary>
    /// <param name="target">�������I�u�W�F�N�g</param>
    /// <param name="duration">�X���C�h����b��</param>
    /// <returns></returns>
    private async UniTaskVoid SlideToTargetPositionAsync(GameObject target, float duration)
    {
        isSliding = true; // �X���C�h���t���O�𗧂Ă�
        Vector3 startPosition = target.transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x, 540, startPosition.z); // Y���W�̂�0�ɐݒ�
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Y���W�݂̂���
            float newY = Mathf.Lerp(startPosition.y, targetPosition.y, elapsedTime / duration);
            target.transform.position = new Vector3(startPosition.x, newY, startPosition.z);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();  // ���̃t���[���܂őҋ@
        }

        // �Ō�Ɉʒu�𐳊m�ɐݒ�
        target.transform.position = targetPosition;
        isSliding = false;
    }
}
