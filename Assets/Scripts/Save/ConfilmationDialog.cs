using UnityEngine;

public class ConfilmationDialog : MonoBehaviour
{
    [SerializeField, Header("�_�C�A���O�p�l���̎Q��")]
    private GameObject dialogPanel;

    private System.Action onConfilm;

    public void Show(System.Action confilmAction)
    {
        dialogPanel.SetActive(true);
        onConfilm = confilmAction;
    }

    public void Confilm()
    {
        onConfilm?.Invoke();
        Close();
    }

    public void Cancel()
    {
        Close();
    }

    private void Close()
    {
        dialogPanel.SetActive(false);
    }
}
