using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{   
    [SerializeField] private Animator doorAnimator = null;
    [SerializeField] private int animationDuration = 1;

    public Camera doorCamera;
    public Camera mainCamera;
    public int doorId;
    
    private bool isDoorOpen = false;
    private bool inBetweenAnimation = false;
    
    private IEnumerator PauseBetweenAnimation() {
        inBetweenAnimation = true;
        yield return new WaitForSeconds(animationDuration);
        inBetweenAnimation = false;
        mainCamera.enabled = true;
        doorCamera.enabled = false;
    }

    public bool PlayAnimation() {
        if (!isDoorOpen && !inBetweenAnimation) {
            doorCamera.enabled = true;
            mainCamera.enabled = false;
            doorAnimator.Play("DoorOpen", 0, 0.0f);
            isDoorOpen = true;
            StartCoroutine(PauseBetweenAnimation());
            return false;
        }
        return true;
    }
}
