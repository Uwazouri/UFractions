using UnityEngine;

/// <summary>
/// Attach this script to a UI object in a canvas and call its show and hide functions.
/// The object will move from either the left or right side of the parent canvas.
/// </summary>
[RequireComponent(typeof(RectTransform), typeof(CanvasRenderer))]
public class SlideUIElement : MonoBehaviour
{
    private float yCenter;

    /// <summary>
    /// Will the UI object be position on the left part of the canvas.
    /// </summary>
    public bool leftOrientation = false;
    public bool centerObjectY = true;
    public bool centerObjectX = false;

    /// <summary>
    /// Will the UI object be hidden on start.
    /// </summary>
    public bool hideOnStart = true;

    private bool show = false;
    private bool hide = false;
    private bool on = false;

    /// <summary>
    /// Initially positions the UI object on start.
    /// </summary>
	void Start ()
    {
        if (this.centerObjectY)
            this.yCenter = 0;
        else
            this.yCenter = this.transform.localPosition.y;

        if (this.hideOnStart)
        {
            if (!leftOrientation)
                this.transform.localPosition = new Vector3(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.xMax, this.yCenter, 0);
            else
                this.transform.localPosition = new Vector3(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.xMin - GetComponent<RectTransform>().rect.width, this.yCenter, 0);
        }
        else
        {
            if (!leftOrientation)
                this.transform.localPosition = new Vector3(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.xMax - GetComponent<RectTransform>().rect.width, this.yCenter, 0);
            else
                this.transform.localPosition = new Vector3(GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.xMin, this.yCenter, 0);
        }
    }

    /// <summary>
    /// Update hides or shows the RectTransform according to Hide and Show calls.
    /// </summary>
    void Update()
    {
        if (this.show)
        {
            this.on = true;
            Rect parentCanvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect;
            if (parentCanvasRect == null)
                print("SlideUIPanel could not find canvas rect");
            if (!this.centerObjectX)
            {
                if (!this.leftOrientation)
                {
                    if (this.transform.localPosition.x <= parentCanvasRect.xMax - GetComponent<RectTransform>().rect.width)
                        this.show = false;
                    else
                        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(parentCanvasRect.xMax - GetComponent<RectTransform>().rect.width, this.yCenter, 0), 25);
                }
                else
                {
                    if (this.transform.localPosition.x >= parentCanvasRect.xMin)
                        this.show = false;
                    else
                        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(parentCanvasRect.xMin, this.yCenter, 0), 25);
                }
            }
            else
            {
                print("ISISIA!");
                if (this.transform.position.x == parentCanvasRect.xMin + parentCanvasRect.width*0.25f)
                    this.show = false;
                else
                    this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(parentCanvasRect.xMin + parentCanvasRect.width * 0.25f, this.yCenter, 0), 25);
            }
        }
        else if (this.hide)
        {
            this.on = false;
            Rect parentCanvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect;
            if (parentCanvasRect == null)
                print("SlideUIPanel could not find canvas rect");
            else if (!this.leftOrientation)
            {
                if (this.transform.localPosition.x >= parentCanvasRect.xMax)
                    this.hide = false;
                else
                    this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(parentCanvasRect.xMax, this.yCenter, 0), 25);
            }
            else
            {
                if (this.transform.localPosition.x <= parentCanvasRect.xMin - GetComponent<RectTransform>().rect.width)
                    this.hide = false;
                else
                    this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, new Vector3(parentCanvasRect.xMin - GetComponent<RectTransform>().rect.width, this.yCenter, 0), 25);
            }
        }
    }

    /// <summary>
    /// Shows the UI object this script is attached to.
    /// </summary>
    public void Show()
    {
        this.hide = false;
        this.show = true;
    }

    /// <summary>
    /// Hides the UI object this script is attached to.
    /// </summary>
    public void Hide()
    {
        this.show = false;
        this.hide = true;
    }

    public void Toggle()
    {
        if (this.on)
        {
            this.hide = true;
            this.show = false;
        }
        else
        {
            this.show = true;
            this.hide = false;
        }
    }
}
