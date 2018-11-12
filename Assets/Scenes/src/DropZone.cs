using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler,IPointerExitHandler,IPointerEnterHandler{

 // public Draggable.Slot typeOfItem = Draggable.Slot.WEAPON;

    public void OnPointerEnter(PointerEventData eventData)

    {  if (eventData.pointerDrag == null)

            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();

       
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        // Debug.Log("OnPointerExit");
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
       

    } 

    public void OnDrop(PointerEventData eventData )
    {

        Debug.Log(eventData.pointerDrag.name+ "was dropped on " + gameObject.name);
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
       
    }


}
