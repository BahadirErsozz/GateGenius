using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 5;
    
    public DoorController rayCasterObj;

    private string interactableTag = "Door";
 
    void Update()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayLength)) {
            if(hit.collider.CompareTag(interactableTag)) {
                if(Input.GetKeyDown(KeyCode.Mouse0)) {
                    GameObject.Find("GameManager").GetComponent<GameManager>().TriggerWonStage(1);
                }
                else if(Input.GetKeyDown(KeyCode.Mouse1)){
                    GameObject.Find("GameManager").GetComponent<GameManager>().TriggerWonStage(2);
                }
            }
        }
    }
}
