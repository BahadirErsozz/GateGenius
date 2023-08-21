using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnvanterControl : MonoBehaviour
{
    public Transform[] imageList;
    public Color active;
    public Color inactive;

    int activeIndex = 0;

    public MouseController mouseController;

    private void Start()
    {
        foreach (var item in imageList)
        {
            item.GetComponent<Image>().color = inactive;
        }
        imageList[0].GetComponent<Image>().color = active;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {   
            imageList[activeIndex].GetComponent<Image>().color = inactive;
            if(activeIndex == 0) {
                mouseController.setMode("NONE");
            }
            else {
                imageList[0].GetComponent<Image>().color = active;
                activeIndex = 0 ;
                mouseController.setMode("AND");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            imageList[activeIndex].GetComponent<Image>().color = inactive;
            if(activeIndex == 1) {
                mouseController.setMode("NONE");

            }
            else {
                imageList[1].GetComponent<Image>().color = active;
                activeIndex = 1;
                mouseController.setMode("NOT");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            imageList[activeIndex].GetComponent<Image>().color = inactive;
            if(activeIndex == 2) {
                mouseController.setMode("NONE");

            }
            else {
                imageList[2].GetComponent<Image>().color = active;
                activeIndex = 2;
                mouseController.setMode("OR");
            }
        }
    }




}
