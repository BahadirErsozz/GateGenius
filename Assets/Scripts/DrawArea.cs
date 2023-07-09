using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawArea : MonoBehaviour
{
    [SerializeField] Gate GatePrefab;
    [SerializeField] Transform DrawingPanel;
    [SerializeField] Transform GateHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0F))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.gameObject.transform== DrawingPanel.transform)
                {
                    addGate(hit.point);

                }
            }
            
        }



    }


    Gate addGate(Vector3 pos)
    {
        var g = Instantiate(GatePrefab, parent: GateHolder);
        g.transform.position = pos;

        return g;
    }


}
