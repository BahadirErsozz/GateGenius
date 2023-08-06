using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{   
    [SerializeField] private Animator doorAnimator = null;
    [SerializeField] private int animationDuration = 1;
    
    private bool isDoorOpen = false;
    private bool inBetweenAnimation = false;
    
    private IEnumerator PauseBetweenAnimation() {
        inBetweenAnimation = true;
        yield return new WaitForSeconds(animationDuration);
        inBetweenAnimation = false;
    }

    public void PlayAnimation() {
        if (!isDoorOpen && !inBetweenAnimation) {
            doorAnimator.Play("DoorOpen", 0, 0.0f);
            isDoorOpen = true;
            StartCoroutine(PauseBetweenAnimation());
        } else if (!inBetweenAnimation) {
            doorAnimator.Play("DoorClose", 0, 0.0f);
            isDoorOpen = false;
            StartCoroutine(PauseBetweenAnimation());                
        }
    }
}
