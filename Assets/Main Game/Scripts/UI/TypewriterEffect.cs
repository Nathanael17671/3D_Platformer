using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject dialougeObject;
    [SerializeField] private GameObject fadeOut;

    [Header("Delay")]
    public float letterDelay = 0.1f;
    public float displayDelayAfterWriten = 5f;

    [Header("Text")]
    [TextArea(3, 10)] public List<String> dialougeText = new List<String>();

    [Header("Tutorial")]
    [SerializeField] private bool isTutorialLevel = false;
    
    private int currentDialougeCount;
    private string currentGoal;
    private string currentText = "";
    [HideInInspector] public bool isTyping = false;
    private bool hidden;
    [HideInInspector] public bool endAfterFinished = false;
    private bool startup = true;

    void Start()
    {
        dialougeObject.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isTyping == false)
        {
            StartTexting();
        }
    }

    public void StartTexting()
    {
        if(dialougeText.Count > currentDialougeCount)
        {
            hidden = false;
            StartCoroutine(TypeText());
        }
        else
        {
            if (hidden == false)
            {
                hidden = true;
                dialougeObject.gameObject.SetActive(false);
                Debug.Log("There is no more Dialouge");
                if (endAfterFinished == true)
                    StartCoroutine(EndGame());
                
            }
            
        }
        
    }

    IEnumerator EndGame()
    {
        Debug.Log("End Game");
        
        if (isTutorialLevel)
        {
            fadeOut.SetActive(true);
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Level_1");
        }
        else
            gameManager.TriggerVictory();
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        if (startup == true)
        {
            startup = false;
            yield return new WaitForSeconds(3f);
            
        }
            
        dialougeObject.gameObject.SetActive(true);
        currentGoal = dialougeText[currentDialougeCount];
        
        for (int i = 0; i < 1 + currentGoal.Length; i++)
        {
            if (gameManager.isPaused == true)
            {
                if(i > 0)
                    i--;
                yield return new WaitForSeconds(letterDelay);
            }
            currentText = currentGoal.Substring(0,i);
            textBox.GetComponent<TMP_Text>().text = currentText;
            yield return new WaitForSeconds(letterDelay);
        }
        
        yield return new WaitForSeconds(displayDelayAfterWriten);
        while (gameManager.isPaused == true)
        {
            yield return new WaitForSeconds(1);
        }
        currentDialougeCount++;
        isTyping = false;
        
    }
}