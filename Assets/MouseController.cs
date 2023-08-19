using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class MouseController : MonoBehaviour
{
    enum Mode
    {
        AND,
        OR,
        NOT
    }

    public bool IsCreatingWire => wireUnderConstruction != null;

    [SerializeField] Chip chipAnd;
    [SerializeField] Chip chipOr;
    [SerializeField] Chip chipNot;
    [SerializeField] Mode mode;
    [SerializeField] Wire wirePrefab;
    

    private Chip chipPrefab;
    Pin wireStartPin;
    Wire wireStartWire;
    Wire wireUnderConstruction;
    Wire wireUnderMouse;
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
        if (IsCreatingWire)
        {
            //Debug.Log("CreatingWire");  
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (hit.collider.CompareTag("DrawArea")) { 
                    Debug.Log(hit.point);
                    UpdateActiveWire(hit.point);
                }

            }
        }
        else if (Input.GetMouseButtonDown(0))
        {      

            if (Physics.Raycast(ray, out hit, rayLength))
            {
                Debug.Log(hit.transform.name);
            
                if (hit.collider.CompareTag("DrawArea"))
                {
                    Transform holder = hit.transform.Find("Chips");
                    addGate(hit.point, holder);

                }
                else if (hit.collider.CompareTag("Pin"))
                {
                    Transform wireHolder = hit.transform.parent.Find("Wires");
                    Pin pin = hit.transform.gameObject.GetComponent<Pin>();
                    StartCreatingWire(pin, hit.point, wireHolder);
                }
            }

        }
    }


    Wire StartCreatingWire(Pin pinToStartFrom,Vector3 startPos, Transform holder)
    {
        Pin wireStartPin = pinToStartFrom;
        return StartCreatingWire(startPos, holder);
    }

    Wire StartCreatingWire(Vector3 point,Transform holder)
    {
        wireUnderConstruction = Instantiate(wirePrefab,parent: holder);
        wireUnderConstruction.SetStartPosition(point);
        return wireUnderConstruction;
    }

    void UpdateActiveWire(Vector3 point)
    {
        if (Input.GetMouseButtonDown(1))
        {
            CancelWire();
        }
        else
        {
            Debug.Log("UpdateActiveWire:" + point.ToString());
            wireUnderConstruction.DrawToPoint(point);
        }
    }
    void CancelWire()
    {
        if (IsCreatingWire)
        {
            wireUnderConstruction.DeleteWire();
            StopCreatingWire();
        }
    }

    void StopCreatingWire()
    {
        wireStartPin = null;
        wireStartWire = null;
        wireUnderConstruction = null;
    }


    Chip addGate(Vector3 pos,Transform chipHolder)
    {
        var g = Instantiate(chipPrefab, parent: chipHolder);
        g.transform.position = pos;

        return g;
    }



}


