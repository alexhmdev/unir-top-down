using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, Interactable
{
    [SerializeField] private GameManagerSO gameManager;
    [Header("Dialogue system")]
    [SerializeField, TextArea(1, 2)] private string[] prhases;
    [SerializeField] private float timeBetweenPrhases = 8;
    [SerializeField] private float timeBetweenCharacters = 0.3f;
    [SerializeField] private string npcName;
    [SerializeField] private GameObject dialogueFrame;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueInteractIcon;
    [SerializeField] private GameObject dialogueNPCSprite;
    [SerializeField] private Sprite NPCSprite;
    private bool isSpeaking = false;
    private bool canSkipPrhase = false;
    private int currentPrhase = 0;
    private int totalPrhases;
    // Start is called before the first frame update
    void Start()
    {
        totalPrhases = prhases.Length;
    }

    private void Update()
    {
        if(isSpeaking && canSkipPrhase)
        {
            // we listen for the player to press the interact button
            if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Debug.Log("Pressed E");
                if(currentPrhase < totalPrhases - 1)
                {
                    currentPrhase++;
                    dialogueText.text = $"{npcName}: ";
                    StartCoroutine(SpeakPrhase(prhases[currentPrhase]));
                } else
                {
                    isSpeaking = false;
                    dialogueFrame.SetActive(false);
                    currentPrhase = 0;
                    StartCoroutine(PlayerCanInteractAgain());
                }
            }
        }
    }

    public void Interact()
    {

        StartCoroutine(StartSpeaking());
    }

    public void ShowHint()
    {
        StartCoroutine(ShowHintPanel("Presiona \"E\" para hablar"));
    }

    IEnumerator ShowHintPanel(string hint)
    {
        gameManager.ShowHintPanel(true);
        gameManager.SetHintText(hint);
        yield return new WaitForSeconds(2);
        gameManager.ShowHintPanel(false);
    }

    IEnumerator StartSpeaking()
    {
        dialogueFrame.SetActive(true);
        dialogueNPCSprite.GetComponent<Image>().sprite = NPCSprite;
        isSpeaking = true;
        dialogueText.text = $"{npcName}: ";
        // Start speaking the first prhase
        yield return SpeakPrhase(prhases[currentPrhase]);

    }

    IEnumerator SpeakPrhase(string prhase)
    {
        canSkipPrhase = false;
        dialogueInteractIcon.SetActive(false);
        foreach (char letter in prhase)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
        dialogueInteractIcon.SetActive(true);
        canSkipPrhase = true;
    }

    IEnumerator PlayerCanInteractAgain()
    {
        yield return new WaitForSeconds(0.1f);
        gameManager.PlayerInteracting(false);
    }
}
