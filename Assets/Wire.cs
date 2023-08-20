using DLS.ChipCreation;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public event System.Action<Wire> WireDeleted;
    public Pin SourcePin { get; private set; }
    public Pin TargetPin { get; private set; }
    public bool IsConnected { get; private set; }
    public ReadOnlyCollection<Vector3> AnchorPoints => new(anchorPoints);
    public Vector3 CurrentDrawToPoint { get; private set; }

   

    [SerializeField] WireRenderer wireRenderer;
    [SerializeField] EdgeCollider2D edgeCollider;
    [SerializeField] float wireThickness;
    [SerializeField] float selectedThicknessPadding;
    [SerializeField] float wireCurveAmount;
    [SerializeField] int wireCurveResolution;
    [SerializeField] MeshRenderer busConnectionDot;

    [SerializeField] Color color;



    public List<Vector3> anchorPoints;
    Vector3[] drawPoints;
    bool isDeleted;
    void Awake()
    {
        wireRenderer.SetThickness(GetThickness(false));
        transform.position = new Vector3(0, 0, RenderOrder.WireEdit);
        edgeCollider.enabled = false;


        anchorPoints = new List<Vector3>();
        drawPoints = new Vector3[0];
        SetColour(color);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateLineRenderer()
    {
        UpdateLineRenderer(anchorPoints.ToArray());
    }

    void UpdateLineRenderer(Vector3[] points)
    {
        wireRenderer.SetAnchorPoints(points, wireCurveAmount, wireCurveResolution);
    }
    public void AddAnchorPoint(Vector3 point)
    {
        // Don't add point if too close to previous anchor point
        if (anchorPoints.Count == 0 || (anchorPoints[^1] - point).sqrMagnitude > 0.00001f)
        {
            anchorPoints.Add(point);
        }
        else
            Debug.Log("TOO CLOSE");
    }

    public void SetAnchorPoints(IList<Vector3> points, bool updateGraphics)
    {
        anchorPoints = new List<Vector3>(points);
        if (updateGraphics)
        {
            UpdateLineRenderer();
        }
    }

    public void DeleteWire()
    {
        if (!isDeleted)
        {
            isDeleted = true;
            Destroy(gameObject);
        }
    }

    public void ConnectWireToPins(Pin SourcePin, Pin TargetPin)
    {
        IsConnected = true;

        // Update renderer
        UpdateLineRenderer();
        wireRenderer.SetThickness(GetThickness(false));


    }

    float GetThickness(bool isSelected)
    {
        return wireThickness + (isSelected ? selectedThicknessPadding : 0);
    }
    void SetColour(Color col, float fadeDuration = 0)
    {
        busConnectionDot.sharedMaterial.color = col;
        wireRenderer.SetColour(col, fadeDuration);
    }

    public void SetHighlightState(bool highlighted)
    {
        wireRenderer.SetThickness(GetThickness(highlighted));
    }

    public void DrawToPoint(Vector3 targetPoint)
    {
        Debug.Log("DrawToPoint:" + targetPoint.ToString());
        // Only draw wire to target point if an anchor point exists, and target point is not on top of last anchor point
        if (anchorPoints.Count > 0 && (anchorPoints[^1] - targetPoint).sqrMagnitude > 0.001f)
        {
            CurrentDrawToPoint = targetPoint;
            List<Vector3> points = new List<Vector3>(anchorPoints);
            points.Add(targetPoint);
            UpdateLineRenderer(points.ToArray());
        }
    }

    public void SetStartPosition(Vector3 pos)
    {
        transform.position = pos;
        AddAnchorPoint(pos);
    }



}
