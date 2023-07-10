using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[SelectionBase]
public class Plug : MonoBehaviour
{
    enum mode {active, inactive }

    [SerializeField] mode WireMode = mode.active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
