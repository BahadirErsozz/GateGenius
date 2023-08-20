using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Pin : MonoBehaviour
{
    public enum HighlightState { None, Highlighted, HighlightedInvalid }
    public enum PinType { Unassigned, ChipInputPin, ChipOutputPin, SubChipInputPin, SubChipOutputPin }

    [SerializeField] float interactionRadius;
    [SerializeField] Color defaultCol;
    [SerializeField] Color highlightedCol;
    [SerializeField] Color highlightedInvalidCol;
    [SerializeField] MeshRenderer display;
    [SerializeField] public HighlightState activeHighlightState;
    public bool IsHighlighted => activeHighlightState != HighlightState.None;

    public bool IsSourcePin => pinType is PinType.ChipInputPin or PinType.SubChipOutputPin;
    public bool IsTargetPin => !IsSourcePin;
    public PinType GetPinType() => pinType;


    PinType pinType;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUp(string name)
    {
        GetComponent<CircleCollider2D>().radius = interactionRadius;
        gameObject.name = $"Pin ({name})";
    }


    public void SetHighlightState(HighlightState state)
    {
        //Debug.Log("HELP!");
        activeHighlightState = state;
        Color col = state switch
        {
            HighlightState.None => defaultCol,
            HighlightState.Highlighted => highlightedCol,
            HighlightState.HighlightedInvalid => highlightedInvalidCol,
            _ => Color.black
        };

        //display.material.color = col;
    }
}
