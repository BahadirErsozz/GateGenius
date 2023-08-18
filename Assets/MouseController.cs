using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    enum Mode
    {
        AND,
        OR,
        NOT
    }

    [SerializeField] Chip chipAnd;
    [SerializeField] Chip chipOr;
    [SerializeField] Chip chipNot;
    private Chip chipPrefab;
    [SerializeField] Mode mode;
  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(mode)
        {
            case Mode.AND: chipPrefab = chipAnd ; break;
            case Mode.OR: chipPrefab = chipOr ; break;
            case Mode.NOT:  chipPrefab = chipNot; break;
        }


        Vector3 rayOrigin = new Vector3(0.5f, 0.5f, 0f); // center of the screen
        float rayLength = 500f;
        Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
        if (Input.GetMouseButtonDown(0))
        {      
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                Debug.Log(hit.transform.name);
                if (hit.collider.CompareTag("DrawArea"))
                {
                    Transform holder = hit.transform.Find("Chips");
                    addGate(hit.point, holder);

                }
            }

        }
    }

    Chip addGate(Vector3 pos,Transform chipHolder)
    {
        var g = Instantiate(chipPrefab, parent: chipHolder);
        g.transform.position = pos;

        return g;
    }
}


