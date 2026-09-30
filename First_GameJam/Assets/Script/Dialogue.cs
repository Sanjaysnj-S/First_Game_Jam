using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    [Header("UI's")]
    public GameObject healthUI;
    public GameObject gameTimer;
    public GameObject dialogueScreen;

    
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    private bool isTyping;

    void Start()
    {
        dialogueScreen.gameObject.SetActive(true);
        // Freeze the entire game
        Time.timeScale = 0f;
        healthUI.gameObject.SetActive(false);
        gameTimer.gameObject.SetActive(false);

        textComponent.text = string.Empty;

        StartDialogue();
    }

    void Update()
    {
        // New Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // If line is still typing, complete it immediately
            if (isTyping)
            {
                StopAllCoroutines();

                textComponent.text = lines[index];
                isTyping = false;
            }
            // If line is already complete, go to next line
            else
            {
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;

            // Works even when Time.timeScale = 0
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            // Close dialogue
            gameObject.SetActive(false);
            dialogueScreen.gameObject.SetActive(false);
            healthUI.gameObject.SetActive(true);
            gameTimer.gameObject.SetActive(true);


            // Resume game
            Time.timeScale = 1f;
        }
    }
}