using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public GameObject outerHealthBar;
    public GameObject innerHealthBar;

    public void setHealth(float percentage)
    {
        Debug.Log(percentage);
        if(innerHealthBar != null)
        {
            float newScale = percentage;
            if (percentage >= 0.9)
            {
                newScale = 0.9f;
            }

            if(percentage < 0.1)
            {
                newScale = 0.1f;
            }

            innerHealthBar.transform.localScale = new Vector3 (newScale,innerHealthBar.transform.localScale.y,innerHealthBar.transform.localScale.z);
            innerHealthBar.transform.localPosition = new Vector3 ((newScale / 2.2f) - 0.4f, innerHealthBar.transform.localPosition.y, innerHealthBar.transform.localPosition.z);
        }
    }
}
