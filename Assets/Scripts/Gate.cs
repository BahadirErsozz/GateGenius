using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class Gate : MonoBehaviour
{
    private enum Type {
        AND,
        OR,
        NAND,
        XOR
    }

    [SerializeField] Type GateType;
    [SerializeField] Plug input1;
    [SerializeField] Plug input2;
    [SerializeField] Plug output;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetUp(string name)
    {
        gameObject.name = $"Pin ({name})";
    }
}
