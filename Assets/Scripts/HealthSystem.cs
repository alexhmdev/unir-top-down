using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthSystem : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private float health;
    private AudioManager audioManager;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(DamageRedAnim());
        health -= damage;
        if(CompareTag("Enemy"))
        {
            audioManager.PlaySFX(audioManager.enemyHit);
        }
        if (health <= 0 && !CompareTag("Player"))
        {
            if(anim != null) {
                anim.SetTrigger("die");
                // lock transform position
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            }
                Invoke(nameof(Die), 0.4f);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    private void Die()
    {
        if(CompareTag("Enemy"))
        {
            gameObject.GetComponent<Slime>().DropItem();
            Destroy(gameObject);
        }
        else if(CompareTag("Player"))
        {
            audioManager.StopBackground();
            audioManager.PlaySFX(audioManager.gameOver);
            //GameObject.FindObjectOfType<GameManagerSO>().GameOver();
        }
    }

    IEnumerator DamageRedAnim()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }


}
