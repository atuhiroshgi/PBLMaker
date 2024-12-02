using UnityEngine;

public class DataButtonManager : Button
{
    [SerializeField, Header("SaveButton��LoadButton�̎Q��")]
    private GameObject[] buttons;

    private bool isOpen = false;

    private void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    protected override void OnClick()
    {
        base.OnClick();

        isOpen = !isOpen;
        ToggleWindow(isOpen);
    }

    public void ToggleWindow(bool isOpen)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(isOpen);
        }
    }
}
