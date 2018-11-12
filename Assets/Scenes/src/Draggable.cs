using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{


    Transform parentToReturnTo = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
    
    }

public void OnDrag(PointerEventData eventData)
{
      Debug.Log("OnDrag");
      // this.transform.position = eventData.position;
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 1.0f; //distance of the plane from the camera
        this.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
  }

public void OnEndDrag(PointerEventData eventData)
{
    Debug.Log("OnEndDrag");
    
}
 }
