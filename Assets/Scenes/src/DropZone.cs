using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    // public Draggable.Slot typeOfItem = Draggable.Slot.WEAPON;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log ());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log(eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerDrag.name + ' ' + gameObject.name);
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

        if (d != null)
        {
            d.parentToReturnTo = this.transform;
        }
    }
}
