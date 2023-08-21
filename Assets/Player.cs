using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public Animator animator;

    public AudioSource hitSound;

    public bool isDead ;

    public Camera deathCamera;

    public Camera mainCamera;

    public int animationDuration = 3;

    private bool inBetweenAnimation = false;

    public GameManager gameManager;

    public GameObject envanterUI;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        hitSound.volume = 0.5f;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth == 0 && isDead == false && !inBetweenAnimation)
        {
            animator.SetTrigger("die");

            isDead = true;
            mainCamera.enabled = false;
            deathCamera.enabled = true;
            envanterUI.SetActive(false);
            StartCoroutine(PauseBetweenAnimation());

            
        }
    }
    private IEnumerator PauseBetweenAnimation()
    {
        inBetweenAnimation = true;
        yield return new WaitForSeconds(animationDuration);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameManager.gameOver();
    }

    public void TakeDamage(int damage)
    {
        currentHealth-= damage;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("damage");
        hitSound.Play();
        
    }

    
}
