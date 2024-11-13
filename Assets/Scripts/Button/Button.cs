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
    /// このクラスがアタッチされたオブジェクトがクリックされたときに呼ばれるメソッド
    /// </summary>
    protected virtual void OnClick()
    {
    }

    /// <summary>
    /// このクラスがアタッチされたオブジェクトの上でクリックが押されたときの処理
    /// </summary>
    protected virtual void OnPointerDown()
    {
    }

    /// <summary>
    /// このクラスがアタッチされたオブジェクトの上でクリックが離されたときの処理
    /// </summary>
    protected virtual void OnPointerUp()
    {
    }
}