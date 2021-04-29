using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Plants;
using Plants.PlantActions;

//Created Zap 26/03
// refactored jay 22/04
public class ToolDrag : MonoBehaviour //IPointerDownHandler
{
    [SerializeField] TendingActions toolType;
    public Image image;

    Vector3 startingPostition;

    bool pickedUp;

    public Rect Rect => image.rectTransform.rect;

    public Vector3 WorldPosition => Camera.main.ScreenToWorldPoint(this.transform.position);

    public TendingActions ToolType => toolType;

    public RectTransform RectTransform { get; set; }

    public Vector3 originalScale;


    // Start is called before the first frame update
    void Start()
    {
        startingPostition = this.transform.localPosition;
        image = this.GetComponent<Image>();
        RectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        EventsManager.BindEvent(EventsManager.EventType.TryRemovePlant, TryTrowel);
        EventsManager.BindEvent(EventsManager.EventType.RemovedPlant, RemovePlant);
    }

    private void OnDisable()
    {
        EventsManager.UnbindEvent(EventsManager.EventType.TryRemovePlant, TryTrowel);
        EventsManager.UnbindEvent(EventsManager.EventType.RemovedPlant, RemovePlant);
    }



    void Update()
    {
        bool isMouseOver = ContainsMouse(Rect);

        if (Input.GetMouseButtonDown(0) && isMouseOver)
        {
            pickedUp = true;
            image.rectTransform.sizeDelta = Vector2.Scale(image.rectTransform.sizeDelta ,new Vector2(1.25f, 1.25f));
        }


        if (pickedUp)
        {
            this.transform.position = Input.mousePosition;
            //worldPosition = Camera.main.ScreenToWorldPoint(this.transform.position);

            if (!Input.GetMouseButton(0))
            {
                pickedUp = false;
                image.rectTransform.sizeDelta = Vector2.Scale(image.rectTransform.sizeDelta, new Vector2(0.8f, 0.8f));
                AttemptToTendPlant(); 
            }
        }
        else
        {
            this.transform.localPosition = startingPostition;
        }
    }

    private bool ContainsMouse(Rect myRect)
    {
        if (myRect.Contains(image.rectTransform.InverseTransformPoint(Input.mousePosition)) && GameManager.Instance.InOwnGarden)
        {
            return true;
        }

        return false;
    }

    void TryTrowel() {
        bool menuAnswer = true;
        // Zap, menu goes here
        if (menuAnswer) {
            RemovePlant();
        } else { 
            // Whatever the alternative is
        }
    }

    void RemovePlant() {
        // Change Score
        // Set location to available
        // Delete gameobject
    }


    void AttemptToTendPlant()
    {
        SlotManager currentGardenSlotManager = GameManager.Instance.SlotManagers[GameManager.Instance.PlayerWhosGardenIsCurrentlyVisible];
        Plant plant = currentGardenSlotManager.PlantOfSlotMouseIsIn(this);

        if (plant == null)
            return;

        if (ToolType != TendingActions.Removing)
        {
            plant.Tend(ToolType);
        }
        else {
            // Are You Sure?
            EventsManager.InvokeEvent(EventsManager.EventType.TryRemovePlant);          
        }


    }

    public Rect GetWorldRect()
    {
        Vector3[] corners = new Vector3[4];
        RectTransform.GetWorldCorners(corners);

        Vector3[] worldCorners = new Vector3[4];
        for (int i = 0; i < corners.Length; i++)
        {
            worldCorners[i] = Camera.main.ScreenToWorldPoint(corners[i]);
           // worldCorners[i].z = ZPos;
        }

        var size = worldCorners[2] - worldCorners[0];

        var center = worldCorners[0] + (size / 2f);

        var worldBounds = new Bounds(center, size);
        var worldRect = new Rect(worldBounds.min.x, worldBounds.min.y, worldBounds.size.x, worldBounds.size.y);
        return worldRect;
    }

}