using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Plants;
using Plants.PlantActions;

//Created Zap 26/03
public class ToolDrag : MonoBehaviour //IPointerDownHandler
{
    Color green;
    [SerializeField] TendingActions toolType;
    public Image myImage;
    Rect myRect;
    Vector3 startingPostition;
    bool isMouseOver;
    Vector3 worldPosition;
    bool isPickedUp;
    public CanvasScaler myCanvasScaler;
   
    
    // Start is called before the first frame update
    void Start()
    {
        green = Color.green;
        startingPostition = this.transform.localPosition;
        myImage = this.GetComponent<Image>();
        //Debug.Log(myImage.rectTransform.rect.Contains);
    }
    
    //public void OnPointerDown(PointerEventData eventData) { Debug.Log(eventData.pointerId); }
    // Update is called once per frame
    void Update()
    {
        myRect = myImage.rectTransform.rect;
       // Debug.Log(myCanvasScaler.referenceResolution.x/ (Screen.currentResolution.width));
        //myRect.x += this.transform.position.x;
        //myRect.y += this.transform.position.y;
        //Debug.Log(myRect);
        //EditorGUI.DrawRect(new Rect(myRect.position,myRect.size), green );
        //Debug.Log(myImage.rectTransform.rect);
        if (myRect.Contains(myImage.rectTransform.InverseTransformPoint(Input.mousePosition)))
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
                TendPlant(); //this is where you would tend the plant 
            }
        }
        else
        {
            this.transform.localPosition = startingPostition;
        }
    }
    public Vector3 GetWorldPosition()
    {
        return worldPosition;
    }

    void TendPlant()
    {
        var a = GameManager.Instance.slotManagers[GameManager.Instance.CurrentVisibleGarden];
        SlotControls slot = a.SlotMouseIsIn();

        if (slot == null)
            return;


        slot.plantsInThisSlot[0].GetComponent<Plants.Plant>().Tend(ToolType);


    }
    public TendingActions ToolType => toolType;
    //needs to have a getter for what kind of tool it is
    //I agree but like how, I don't know how the tools are listed, used a string for now, perhaps use a dictionary in the slots?

}
