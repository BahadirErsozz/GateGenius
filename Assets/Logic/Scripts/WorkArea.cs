using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkArea : MonoBehaviour
{
    public event System.Action WorkAreaResized;

    public float Width { get; private set; }
    public float Height { get; private set; }

    [SerializeField] float Z = 1;
    [SerializeField] MeshRenderer background;
    [SerializeField] BoxCollider2D backgroundCollider;
    [SerializeField] MeshRenderer[] outlineEdges;
    [SerializeField] Transform inputBar;
    [SerializeField] Transform outputBar;

    [SerializeField, Range(0, 1)] float widthT;
    [SerializeField, Range(0, 1)] float heightT;
    [SerializeField, Range(0, 1)] float thicknessT;
    [SerializeField] float offsetY;
    [SerializeField] float ioBarPadding;

    [SerializeField] Color outlineCol;
    [SerializeField] Color backgroundCol;
    bool needsUpdate;

    private void Start()
    {
        needsUpdate = true;
    }

    public void SetUp()
    {
        needsUpdate = true;
        SetWidthAndHeight();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public bool ContainsPoint(Vector2 worldPoint)
    {
        Vector2 min = backgroundCollider.bounds.min;
        Vector2 max = backgroundCollider.bounds.max;
        return worldPoint.x >= min.x && worldPoint.x <= max.x && worldPoint.y >= min.y && worldPoint.y <= max.y;
    }

    public bool OutOfBounds(Bounds bounds)
    {
        Vector2 min = backgroundCollider.bounds.min;
        Vector2 max = backgroundCollider.bounds.max;

        return bounds.min.x < min.x || bounds.max.x > max.x || bounds.min.y < min.y || bounds.max.y > max.y;
    }

    public bool AnyOutOfBounds(Bounds[] bounds)
    {
        foreach (Bounds b in bounds)
        {
            if (OutOfBounds(b))
            {
                return true;
            }
        }
        return false;
    }

    void UpdateDisplay()
    {

        if (needsUpdate)
        {
            needsUpdate = false;

            SetWidthAndHeight();
            //float orthoSize = cam.orthographicSize;
            float orthoSize = 5;
            float thickness = thicknessT * orthoSize * 0.1f;

            const float referenceOrthoSize = 5;

            float posY = offsetY * orthoSize / referenceOrthoSize;
            transform.localPosition = Vector3.up * posY;

            // Set background
            background.transform.localPosition = new Vector3(0, 0, RenderOrder.Background);
            background.transform.localScale = new Vector3(Width + thickness / 2, Height + thickness / 2, 1);
            background.material.color = backgroundCol;

            // Set outline
            outlineEdges[0].material.color = outlineCol;

            outlineEdges[0].transform.localPosition = new Vector3(-Width / 2, 0, RenderOrder.BackgroundOutline);
            outlineEdges[1].transform.localPosition = new Vector3(Width / 2, 0, RenderOrder.BackgroundOutline);
            outlineEdges[2].transform.localPosition = new Vector3(0, -Height / 2, RenderOrder.BackgroundOutline);
            outlineEdges[3].transform.localPosition = new Vector3(0, Height / 2, RenderOrder.BackgroundOutline);

            outlineEdges[0].transform.localScale = new Vector3(thickness, Height, 1);
            outlineEdges[1].transform.localScale = new Vector3(thickness, Height, 1);
            outlineEdges[2].transform.localScale = new Vector3(Width + thickness, thickness, 1);
            outlineEdges[3].transform.localScale = new Vector3(Width + thickness, thickness, 1);

            // Set input / output bar
            float ioBarWidth = 1;
            inputBar.localPosition = new Vector3(-(Width + ioBarWidth + ioBarPadding) / 2, 0, RenderOrder.Background);
            inputBar.localScale = new Vector3(ioBarWidth, Height, 1);

            outputBar.localPosition = new Vector3((Width + ioBarWidth + ioBarPadding) / 2, 0, RenderOrder.Background);
            outputBar.localScale = new Vector3(ioBarWidth, Height, 1);
        }
    }

    void SetWidthAndHeight()
    {
        float screenHeightWorld = 5 * 2;
        float screenWidthWorld = screenHeightWorld * 16 / 9;
        Width = screenWidthWorld * widthT;
        Height = screenHeightWorld * heightT;
        WorkAreaResized?.Invoke();
    }

    public void VidHelper_ExpandView()
    {
        inputBar.gameObject.SetActive(false);
        outputBar.gameObject.SetActive(false);
        widthT = 0.966f;
        needsUpdate = true;
    }

    void OnValidate()
    {
        if (Application.isEditor)
        {
            needsUpdate = true;
        }
    }
}
