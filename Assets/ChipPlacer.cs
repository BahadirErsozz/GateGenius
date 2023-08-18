using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipPlacer : MonoBehaviour
{
    [SerializeField] Chip chipPrefab;
    [SerializeField] Transform chipHolder;
    [SerializeField] Transform DrawingPanel;
    [SerializeField] float rayLength = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); // center of the screen
            float rayLength = 500f;
            Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                Debug.Log(hit.transform.name, DrawingPanel.transform);
                if (hit.transform.gameObject.transform == DrawingPanel.transform)
                {
                    addGate(hit.point);

                }
            }

        }
    }


    Chip addGate(Vector3 pos)
    {
        var g = Instantiate(chipPrefab, parent: chipHolder);
        g.transform.position = pos;

        return g;
    }
}
