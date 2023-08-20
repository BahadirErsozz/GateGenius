using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class IOPin : MonoBehaviour
{
    enum IOmode {INPUT,OUTPUT}
    

    [SerializeField] Color activeColor;
    [SerializeField] Color inActiveColor;
    [SerializeField] IOmode mode;
    [SerializeField] public Pin.HighlightState state;
    [SerializeField] MeshRenderer display;

    [SerializeField] Pin pin;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == IOmode.INPUT)
        {
            pin.SetHighlightState(state);
            
        }
        else if (mode == IOmode.OUTPUT)
        {
            state = pin.activeHighlightState;
        }
        UpdateDisplayState();
    }


    public void UpdateDisplayState()
    {
        Pin.HighlightState activeHighlightState = state;
        Color col = activeHighlightState switch
        {
            Pin.HighlightState.None => inActiveColor,
            Pin.HighlightState.Highlighted => activeColor,
            _ => Color.black
        };

        display.material.color = col;
    }
    public void SetHighlightState(Pin.HighlightState state)
    {
        this.state = state;
    }


}
