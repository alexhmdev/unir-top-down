using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float inputH = 0;
    private float inputV = 0;
    private Vector3 direction;
    private Vector3 lastInput;
    private Vector3 interactionPosition;
    private bool isMoving = false;
    private Collider2D interactable;
    [SerializeField] private float speed = 5;
    [SerializeField] private float interactRadius = 1;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject gameOverPanel;
    private float actualSpeed;
    private HealthSystem healthSystem;
    private InventorySystem inventorySystem;
    private AudioManager audioManager;
    private bool isInteracting = false;
    public bool IsInteracting
    {
        get { return isInteracting; }
        set { isInteracting = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        healthSystem = gameObject.GetComponent<HealthSystem>();
        inventorySystem = gameObject.GetComponent<InventorySystem>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        actualSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        ReadInputs();
        // flip the sprite
        if (inputH != 0)
        {
            spriteRenderer.flipX = inputH == -1;
        }
       
        // Move the player only if it is in a grid position
        if(!isMoving && (inputH != 0 || inputV != 0))
        {
            anim.SetBool("isMoving", true);
            anim.SetFloat("inputH", inputH);
            anim.SetFloat("inputV", inputV);
            lastInput = new Vector3(inputH, inputV, 0);
            direction = transform.position + new Vector3(inputH, inputV, 0);
            interactionPosition = direction;
            interactable = CheckInteractable();
            if(interactable == null)
            {
            StartCoroutine(Move());
            }
        }  else if(inputH == 0 && inputV == 0)
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void ReadInputs()
    {
        if(isInteracting) return;
        if (inputV == 0)
        {
            inputH = Input.GetAxisRaw("Horizontal");

        }
        if (inputH == 0)
        {
            inputV = Input.GetAxisRaw("Vertical");
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        // Run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = actualSpeed * 1.5f;
            // increase animation speed
            anim.speed = 1.5f;
        }
        else
        {
            speed = actualSpeed;
            anim.speed = 1;
        }
    }

    private void Attack()
    {
        Debug.Log("Attack");

        if (inventorySystem.InventoryItems.Contains("Sword"))
        {
            anim.SetTrigger("Attack");
            Invoke("AttackOnAnim", 0.1f);
        }
    }

    private void AttackOnAnim()
    {
        audioManager.PlaySFX(audioManager.playerAttack);
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 1, interactableLayer);
        foreach (Collider2D other in hit)
        {
            Debug.Log(other.tag);
            if(other.CompareTag("Enemy"))
            { 
            other.GetComponent<HealthSystem>().TakeDamage(10);
            }
        }
    }

    private void Interact()
    {
        interactable = CheckInteractable();
        if(interactable == null) return;
        if (interactable.GetComponent<Interactable>() != null)
            isInteracting = true;
            interactable.GetComponent<Interactable>().Interact();
    }

    public void OnTakeDamage()
    {
        // update health bar
        float health = healthSystem.GetHealth();
        //healthbar.value = health * 0.01f;

        if (healthSystem.GetHealth() <= 0)
        {
            // Game Over
            //GameOver();

        }
    }

    IEnumerator Move()
    {
        isMoving = true;
        while (transform.position != direction)
        {
            transform.position = Vector3.MoveTowards(transform.position, direction, speed * Time.deltaTime);
            yield return null;
        }
        // Update interactionPosition to the new position;
        interactionPosition = transform.position + lastInput;
        isMoving = false;
    }

    private Collider2D CheckInteractable()
    {

        Collider2D interactable = Physics2D.OverlapCircle(interactionPosition, interactRadius, interactableLayer);
        if(interactable != null)
        {
            if(interactable.GetComponent<Interactable>() != null)
            {
                interactable.GetComponent<Interactable>().ShowHint();
            }
        }
        return interactable;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPosition, interactRadius);
        Gizmos.DrawWireSphere(interactionPosition - 0.5f * lastInput, 0.4f);
    }
}

