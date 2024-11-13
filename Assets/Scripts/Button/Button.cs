using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDown();
    }
    
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUp();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    /// <summary>
    /// ���̃N���X���A�^�b�`���ꂽ�I�u�W�F�N�g���N���b�N���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    /// </summary>
    protected virtual void OnClick()
    {
    }

    /// <summary>
    /// ���̃N���X���A�^�b�`���ꂽ�I�u�W�F�N�g�̏�ŃN���b�N�������ꂽ�Ƃ��̏���
    /// </summary>
    protected virtual void OnPointerDown()
    {
    }

    /// <summary>
    /// ���̃N���X���A�^�b�`���ꂽ�I�u�W�F�N�g�̏�ŃN���b�N�������ꂽ�Ƃ��̏���
    /// </summary>
    protected virtual void OnPointerUp()
    {
    }
}