using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class MouseController : MonoBehaviour
{

    public event System.Action<Wire> WireCreated;

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

    bool creatingWireFromPin => IsCreatingWire && wireStartPin != null;
    bool creatingWireFromWire => IsCreatingWire && wireStartWire != null;

    List<Wire> allConnectedWires;
    HashSet<(PinType, PinType)> validConnectionsLookup;

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
        float rayLength = 10f;
        Ray ray = Camera.main.ViewportPointToRay(rayOrigin);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red);
        if (IsCreatingWire)
        {

            Debug.Log("CreatingWire");  
            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider.CompareTag("Pin"))
                    {
                        Debug.Log(hit.point);
                        Pin pin = hit.transform.gameObject.GetComponent<Pin>();
                        OnMouseReleasedOverPin(pin);
                    }
                    else
                    {
                        OnWorkAreaPressed(hit.point);
                    }
                    
                }
                else if (hit.collider.CompareTag("DrawArea"))
                {
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
        wireStartPin = pinToStartFrom;
        return StartCreatingWire(startPos, holder);
    }

    Wire StartCreatingWire(Vector3 point,Transform holder)
    {
        wireUnderConstruction = Instantiate(wirePrefab,parent: holder);
        wireUnderConstruction.SourcePin = wireStartPin;
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

    void OnWorkAreaPressed(Vector3 pos)
    {
        if (IsCreatingWire)
        {
            wireUnderConstruction.AddAnchorPoint(pos);
        }
    }

    void OnMouseReleasedOverPin(Pin pin)
    {
        if (IsCreatingWire)
        {
            TryMakeConnection(wireUnderConstruction.SourcePin,pin);
        }
    }

    void TryMakeConnection(Pin startPin, Pin endPin)
    {

            if (creatingWireFromWire)
            {
                JoinFromWire(wireStartWire, endPin);
            }
            MakeConnection(startPin, endPin);
        
    }


    void JoinFromWire(Wire targetWire, Pin targetWirePin)
    {
        bool copyToSource = targetWire.SourcePin == targetWirePin;
        List<Vector3> targetWirePoints = new List<Vector3>(targetWire.AnchorPoints);
        List<Vector3> newWirePoints = new List<Vector3>(wireUnderConstruction.AnchorPoints);

        Vector3 joinPoint = newWirePoints[0];
        joinPoint = ClosestPointOnPath(joinPoint, targetWirePoints, out int closestSegmentIndex);
        newWirePoints[0] = joinPoint;

        int anchorIndex = (copyToSource) ? closestSegmentIndex : closestSegmentIndex + 1;
        do
        {
            newWirePoints.Insert(0, targetWirePoints[anchorIndex]);
            anchorIndex += copyToSource ? -1 : 1;
        }
        while (anchorIndex >= 0 && anchorIndex <= targetWirePoints.Count - 1);

        // Insert the join point into the wire we're connecting to, to make the connection look more convincing
        //targetWirePoints.Insert(closestSegmentIndex + 1, joinPoint);
        //targetWire.SetAnchorPoints(targetWirePoints, true);
        wireUnderConstruction.SetAnchorPoints(newWirePoints, true);
    }

    void MakeConnection(Pin startPin, Pin endPin)
    {

        wireUnderConstruction.AddAnchorPoint(endPin.transform.position);

        wireUnderConstruction.ConnectWireToPins(startPin, endPin);

        Wire connectedWire = wireUnderConstruction;
        StopCreatingWire();
        //OnWireConnected(connectedWire);
    }

    void OnWireConnected(Wire wire)
    {
        allConnectedWires.Add(wire);
        WireCreated?.Invoke(wire);
    }



    public static Vector2 ClosestPointOnPath(Vector3 p, IList<Vector3> path, out int closestSegmentIndex)
    {
        Vector3 cp = path[0];
        float bestDst = float.MaxValue;
        closestSegmentIndex = 0;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 newP = ClosestPointOnLineSegment(path[i], path[i + 1], p);
            float sqrDst = (p - newP).sqrMagnitude;
            if (sqrDst < bestDst)
            {
                bestDst = sqrDst;
                cp = newP;
                closestSegmentIndex = i;
            }
        }

        return cp;
    }

    public static Vector3 ClosestPointOnPath(Vector3 p, IList<Vector3> path)
    {
        return ClosestPointOnPath(p, path, out _);
    }

    public static Vector3 ClosestPointOnLineSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 p)
    {
        Vector3 aB = lineEnd - lineStart;
        Vector3 aP = p - lineStart;
        float sqrLenAB = aB.sqrMagnitude;
        // Handle case where start/end points are in same position (i.e. line segment is just a single point)
        if (sqrLenAB == 0)
        {
            return lineStart;
        }

        float t = Mathf.Clamp01(Vector3.Dot(aP, aB) / sqrLenAB);
        return lineStart + aB * t;
    }
}





