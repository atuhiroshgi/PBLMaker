using UnityEngine;
using UnityEngine.UI;

public class LoadButton : Button
{
    [SerializeField, Header("ResetButton�̎Q��")]
    private ResetButton resetButton;
    [SerializeField, Header("SlotButton���A�^�b�`�����I�u�W�F�N�g�̃v���n�u")]
    private GameObject slotButtonPrefab;
    [SerializeField, Header("SlotLoadButton��\������p�l��")]
    private Transform slotButtonPanel;
    [SerializeField, Header("�{�^�����m�̊Ԋu")]
    private float buttonSpacing = 400f;

    protected override void OnClick()
    {
        base.OnClick();
        OpenLoadPanel();

        foreach (Transform child in slotButtonPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject slotButton = Instantiate(slotButtonPrefab, slotButtonPanel);
            RectTransform rectTransform = slotButton.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2(
                (i - 1) * (rectTransform.rect.width + buttonSpacing),
                0
            );

            SlotLoadButton slotLoadButtonComponent = slotButton.GetComponent<SlotLoadButton>();
            slotLoadButtonComponent.Initialize(i + 1, resetButton);
        }
    }

    public void OpenLoadPanel()
    {
        slotButtonPanel.gameObject.SetActive(true);
    }

    public void CloseLoadPanel()
    {
        slotButtonPanel.gameObject.SetActive(false);
    }
}