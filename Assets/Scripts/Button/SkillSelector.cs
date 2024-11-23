using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelector : MonoBehaviour
{
    private static float IRIS_IN = 3f;      // アイリスイン後のサイズ
    private static float IRIS_OUT = 17f;    // アイリスアウト後のサイズ
    private static float SCALE_SPEED = 8f;  // スケールの変更速度

    [SerializeField, Header("スキルパネルの親オブジェクト")]
    private GameObject skillPanel;
    [SerializeField, Header("マスクする画像UI")]
    private Image unmaskImage;
    [SerializeField, Header("スキルパネルの子オブジェクト")]
    private SkillButton[] skillIcons;

    private Camera mainCamera;
    private Player player;
    private Vector3 targetScale;
    private bool isOpen = false;        // スキル選択パネルが開いているかどうか

    private void Awake()
    {
        mainCamera = Camera.main;
        player = GetComponent<Player>();
        skillPanel.SetActive(false);

        // unmaskImageの初期スケールを設定
        targetScale = Vector3.one * IRIS_OUT;
        unmaskImage.transform.localScale = targetScale;
    }

    private void Update()
    {
        if (ExecuteManager.Instance.GetIsExecute()) return;

        // 左クリックが押されたとき
        if (Input.GetMouseButtonDown(0))
        {
            // マウス位置からRayを作成
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            // Rayがオブジェクトに当たった場合
            if(hit.collider != null && hit.collider.transform == transform)
            {
                OnObjectClicked();
            }
        }

        skillPanel.SetActive(isOpen);

        // unmaskImageのスケールを滑らかに変更
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
    /// スキル選択パネルが開いているかのゲッター
    /// </summary>
    /// <returns></returns>
    public bool GetIsOpen()
    {
        return isOpen;
    }
}
