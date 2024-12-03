using UnityEngine;
using UnityEngine.UI;

public class SaveButton : Button
{
    [SerializeField, Header("SlotButton�N���X���g�����I�u�W�F�N�g�̃v���n�u")]
    private GameObject slotButtonPrefab;
    [SerializeField, Header("slotButtonPrefab��ݒu����Panel")]
    private Transform slotButtonPanel;
    [SerializeField, Header("slotButton�̊Ԋu")]
    private float buttonSpacing = 400f;

    protected override void OnClick()
    {
        base.OnClick();
        OpenSavePanel();

        // �N���A��ɐV�K����
        foreach (Transform child in slotButtonPanel)
        {
            Destroy(child.gameObject);
        }

        // �X���b�g�{�^����3����
        for (int i = 0; i < 3; i++)
        {
            GameObject slotButton = Instantiate(slotButtonPrefab, slotButtonPanel);
            RectTransform rectTransform = slotButton.GetComponent<RectTransform>();

            // X�������Ƀ{�^�������炷
            rectTransform.anchoredPosition = new Vector2(
                (i - 1) * (rectTransform.rect.width + buttonSpacing),
                0
            );

            SlotButton slotButtonComponent = slotButton.GetComponent<SlotButton>();
            slotButtonComponent.Initialize(i + 1);
        }
    }

    public void OpenSavePanel()
    {
        slotButtonPanel.gameObject.SetActive(true);
    }

    public void CloseSavePanel()
    {
        slotButtonPanel.gameObject.SetActive(false);
    }
}