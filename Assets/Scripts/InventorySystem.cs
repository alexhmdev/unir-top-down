using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory System")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Button[] inventorySlots;
    [Header("Game Manager")]
    [SerializeField] private GameManagerSO gameManager;
    private int inventorySize;
    private string[] inventoryItems = new string[0];

    public string[] InventoryItems { get => inventoryItems; set => inventoryItems = value; }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            int index = i;
            inventorySlots[i].onClick.AddListener(() => ItemClicked(index));
            
        }
        inventorySize = inventorySlots.Length;
        inventoryItems = new string[inventorySize];
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Inventory Opened");
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            gameManager.PlayerInteracting(inventoryPanel.activeSelf);
        }
    }

    void ItemClicked(int index)
    {
        Debug.Log("Item Clicked: " + index);
    }

    public void AddItem(ItemSO item)
    {

        StartCoroutine(ShowHint());
        if (item.isStackable)
        {
            // check if the item is already in the inventory
            if (inventoryItems.Contains(item.itemName))
            {
                // add the item to the stack
                for (int i = 0; i < inventoryItems.Length; i++)
                {
                    if (inventoryItems[i] == item.itemName)
                    {
                        inventorySlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = int.Parse(inventorySlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text) + 1 + "";
                        break;
                    }
                }
                return;
            } else
            {
                inventorySlots[inventorySize - 1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "1";
            }
        } else
        {
            // text will be null if the item is not stackable
            inventorySlots[inventorySize - 1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = null;
        }
        inventorySlots[inventorySize - 1].gameObject.SetActive(true);
        inventorySlots[inventorySize - 1].transform.GetChild(0).GetComponent<Image>().sprite = item.itemSprite;
        inventoryItems[inventorySize - 1] = item.itemName;
        inventorySize--;
        Debug.Log("Item Added: " + item.itemName);

    }

    IEnumerator ShowHint()
    {
        yield return new WaitForSeconds(0.1f);
        gameManager.ShowHintPanel(true);
        gameManager.SetHintText("Presiona \"I\" para abrir el inventario");
        yield return new WaitForSeconds(2);
        gameManager.ShowHintPanel(false);
    } 
}
