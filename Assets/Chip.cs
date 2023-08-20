using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class Chip : MonoBehaviour
{
    [SerializeField] Pin[] inputtPins;
    [SerializeField] Pin[] outputtPins;
    [SerializeField] MODE mode;

    enum MODE {AND,OR,NOT};

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateOutputPin();
    }

    public void updateOutputPin()
    {
        outputtPins[0].SetHighlightState(compute());
    }

    public Pin.HighlightState compute()
    {
        bool result = mode switch
        {
            MODE.AND => computeAND(),
            MODE.OR => computeOR(),
            MODE.NOT => computeNOT(),
            _ => true,
        };
        if(result ) { return Pin.HighlightState.Highlighted; }
        else return Pin.HighlightState.None;
    }

    public bool computeAND()
    {
        bool result = true;
        foreach (Pin pin in inputtPins)
        {
            result = result && pin.IsHighlighted;
        }
        return result;
    }

    public bool computeOR()
    {
        bool result = true;
        foreach (Pin pin in inputtPins)
        {
            result = result || pin.IsHighlighted;
        }
        return result;
    }

    public bool computeNOT()
    {
        return !inputtPins[0].IsHighlighted;
    }
}
