using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Interactable
{
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private ItemSO item;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
         FloatingAnim();
    }

    public void Interact()
    {
        Debug.Log("Item Interact");
        gameManager.PlayerInteracting(false);
        gameManager.AddItemToInventory(item);
        gameManager.ShowHintPanel(false);
        audioManager.PlaySFX(audioManager.pickUpItem);
        Destroy(gameObject);
    }

    public void ShowHint()
    {
        StartCoroutine(ShowHintPanel(item.hint));
    }

    void FloatingAnim()
    {
        // make the item float up and down
        transform.position = new Vector2(transform.position.x, transform.position.y + Mathf.Sin(Time.time * 5f) * 0.001f);
    }

    IEnumerator ShowHintPanel(string hint)
    {
        gameManager.ShowHintPanel(true);
        gameManager.SetHintText(hint);
        yield return new WaitForSeconds(2);
        gameManager.ShowHintPanel(false);
    }

   
}
