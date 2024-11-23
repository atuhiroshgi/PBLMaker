using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelector : MonoBehaviour
{
    private static float IRIS_IN = 3f;      // �A�C���X�C����̃T�C�Y
    private static float IRIS_OUT = 17f;    // �A�C���X�A�E�g��̃T�C�Y
    private static float SCALE_SPEED = 8f;  // �X�P�[���̕ύX���x

    [SerializeField, Header("�X�L���p�l���̐e�I�u�W�F�N�g")]
    private GameObject skillPanel;
    [SerializeField, Header("�}�X�N����摜UI")]
    private Image unmaskImage;
    [SerializeField, Header("�X�L���p�l���̎q�I�u�W�F�N�g")]
    private SkillButton[] skillIcons;

    private Camera mainCamera;
    private Player player;
    private Vector3 targetScale;
    private bool isOpen = false;        // �X�L���I���p�l�����J���Ă��邩�ǂ���

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GetComponent<Player>();
        skillPanel.SetActive(false);

        // unmaskImage�̏����X�P�[����ݒ�
        targetScale = Vector3.one * IRIS_OUT;
        unmaskImage.transform.localScale = targetScale;
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute()) return;

        // ���N���b�N�������ꂽ�Ƃ�
        if (Input.GetMouseButtonDown(0))
        {
            // �}�E�X�ʒu����Ray���쐬
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Ray���I�u�W�F�N�g�ɓ��������ꍇ
            if(hit.collider != null && hit.collider.transform == transform)
            {
                OnObjectClicked();
            }
        }

        skillPanel.SetActive(isOpen);

        // unmaskImage�̃X�P�[�������炩�ɕύX
        unmaskImage.transform.localScale = Vector3.Lerp(unmaskImage.transform.localScale, targetScale, Time.deltaTime * SCALE_SPEED);
    }

    private void OnObjectClicked()
    {
        isOpen = !isOpen;
        targetScale = Vector3.one * (isOpen ? IRIS_IN : IRIS_OUT);

        if (isOpen)
        {
            bool[] playerSkills = player.GetPlayerSkills();
            for(int i = 0; i < playerSkills.Length; i++)
            {
                skillIcons[i].SetIsChecked(playerSkills[i]);
            }
        }
        else
        {
            for(int i = 0; i < skillIcons.Length; i++)
            {
                player.SetSkill(i, skillIcons[i].GetIsChecked());
            }
        }
    }

    /// <summary>
    /// �X�L���I���p�l�����J���Ă��邩�̃Q�b�^�[
    /// </summary>
    /// <returns></returns>
    public bool GetIsOpen()
    {
        return isOpen;
    }
}
