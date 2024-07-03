using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "GameManagerSO")]
public class GameManagerSO : ScriptableObject
{
    private Player player;
    private InventorySystem inventorySystem;
    private GameObject hintPanel;
    private TextMeshProUGUI hintText;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindObjectOfType<Player>();
        inventorySystem = GameObject.FindObjectOfType<InventorySystem>();
        hintPanel = GameObject.FindGameObjectWithTag("HintPanel");
        hintText = GameObject.FindGameObjectWithTag("HintText").GetComponent<TextMeshProUGUI>();
        hintPanel.SetActive(false);
    }


    public void PlayerInteracting(bool state)
   {
       player.IsInteracting = state;
   }

    public void AddItemToInventory(ItemSO item)
    {
        inventorySystem.AddItem (item);
    }

    public void ShowHintPanel(bool state)
    {
        hintPanel.SetActive(state);
    }

    public void SetHintText(string text)
    {
        hintText.text = text;
    }
}
