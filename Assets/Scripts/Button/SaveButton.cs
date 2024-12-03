using UnityEngine;
using UnityEngine.UI;

public class SaveButton : Button
{
    [SerializeField, Header("SlotButtonクラスを使ったオブジェクトのプレハブ")]
    private GameObject slotButtonPrefab;
    [SerializeField, Header("slotButtonPrefabを設置するPanel")]
    private Transform slotButtonPanel;
    [SerializeField, Header("slotButtonの間隔")]
    private float buttonSpacing = 400f;

    protected override void OnClick()
    {
        base.OnClick();
        OpenSavePanel();

        // クリア後に新規生成
        foreach (Transform child in slotButtonPanel)
        {
            Destroy(child.gameObject);
        }

        // スロットボタンを3つ生成
        for (int i = 0; i < 3; i++)
        {
            GameObject slotButton = Instantiate(slotButtonPrefab, slotButtonPanel);
            RectTransform rectTransform = slotButton.GetComponent<RectTransform>();

            // X軸方向にボタンをずらす
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