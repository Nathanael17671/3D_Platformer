using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Referances")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject textBox;

    [Header("Delay")]
    public float letterDelay = 0.1f;
    public float displayDelayAfterWriten = 5f;

    [Header("Text")]
    [TextArea(3, 10)] public List<String> dialougeText = new List<String>();
    
    private int currentDialougeCount;
    private string currentGoal;
    private string currentText = "";
    [HideInInspector] public bool isTyping = false;

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
            StartCoroutine(TypeText());
        }
        else
        {
            this.gameObject.SetActive(false);
            Debug.Log("There is no more Dialouge");
        }
        
    }

    IEnumerator TypeText()
    {
        isTyping = true;
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