using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{   
    [SerializeField] private Animator doorAnimator = null;
    [SerializeField] private int animationDuration = 1;

    public Camera doorCamera;
    public Camera mainCamera;
    
    
    private bool isDoorOpen = false;
    private bool inBetweenAnimation = false;
    
    private IEnumerator PauseBetweenAnimation() {
        inBetweenAnimation = true;
        yield return new WaitForSeconds(animationDuration);
        inBetweenAnimation = false;
        mainCamera.enabled = true;
        doorCamera.enabled = false;
    }

    public void PlayAnimation() {
        if (!isDoorOpen && !inBetweenAnimation) {
            mainCamera.enabled = false;
            doorCamera.enabled = true;
            doorAnimator.Play("DoorOpen", 0, 0.0f);
            isDoorOpen = true;
            StartCoroutine(PauseBetweenAnimation());
        }
    }
}
