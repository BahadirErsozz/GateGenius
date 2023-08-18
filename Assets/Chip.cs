using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[SelectionBase]
[ExecuteInEditMode]
public class Chip : MonoBehaviour
{
    [SerializeField] Pin[] inputtPins;
    [SerializeField] Pin[] outputtPins;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
