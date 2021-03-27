using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Created Zap 26/03
public class ToolDrag : MonoBehaviour //IPointerDownHandler
{
    Image myImage;
    Rect myRect;
    Vector3 startingPostition;
    bool isMouseOver;
    Vector3 worldPosition;
    bool isPickedUp;
    // Start is called before the first frame update
    void Start()
    {
        startingPostition = this.transform.localPosition;
        myImage = this.GetComponent<Image>();
        //Debug.Log(myImage.rectTransform.rect.Contains);
    }
    
    //public void OnPointerDown(PointerEventData eventData) { Debug.Log(eventData.pointerId); }
    // Update is called once per frame
    void Update()
    {
        myRect = myImage.rectTransform.rect;
        myRect.x += this.transform.position.x;
        myRect.y += this.transform.position.y;
        //Debug.Log(myImage.rectTransform.rect);
        if (myRect.Contains(Input.mousePosition))
        {
            isMouseOver = true;
            //Debug.Log("in");
        }
        else
        {
            isMouseOver = false;
            //Debug.Log("out");
        }
        if (Input.GetMouseButtonDown(0) && isMouseOver)
        {
            isPickedUp = true;
        }
        if (isPickedUp)
        {
            this.transform.position = Input.mousePosition;
            worldPosition = Camera.main.ScreenToWorldPoint(this.transform.position);

            if (!Input.GetMouseButton(0))
            {
                isPickedUp = false;
            }
        }
        else
        {
            this.transform.localPosition = startingPostition;
        }
    }
}
