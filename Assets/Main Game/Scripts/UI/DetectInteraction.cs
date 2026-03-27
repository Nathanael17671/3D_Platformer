using UnityEngine;

public class DetectInteraction : MonoBehaviour
{
    [Header("Dialouge")]
    [SerializeField] private TypewriterEffect typewriterEffect;
    [SerializeField][TextArea(3, 10)] private string dialouge = "Placeholder";
    

    [Header("Set Active")]
    [SerializeField] private GameObject nextInteractor;

    [Header("Interact")]
    [SerializeField] private GrabbableObject grabbableObject;
    [SerializeField] private bool isGrabbableObject = false;
    [SerializeField] private CraftingTable craftingTable;
    [SerializeField] private bool isInputIngredient1 = false;
    [SerializeField] private bool isInputIngredient2 = false;
    [SerializeField] private bool isStartMachine = false;
    [SerializeField] private bool isStopMachine = false;
    

    [Header("Other")]
    [SerializeField] private bool skip = false;
    [SerializeField] private bool endAfterThis = false;

    void Update()
    {
        Grabbible();
        InputIngredient1();
        InputIngredient2();
        StartMachine();
        StopMachine();
        Skip();
        if (endAfterThis == true)
            typewriterEffect.endAfterFinished = true;
    }
    void Grabbible()
    {
        if (!isGrabbableObject)
            return;
        if (grabbableObject == null)
            return;
        if (grabbableObject.wasGrabbed)
        {
            typewriterEffect.dialougeText.Add(dialouge);
            nextInteractor.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    void InputIngredient1()
    {
        if (!isInputIngredient1)
            return;
        if (craftingTable == null)
            return;
        if (grabbableObject == null)
        {
            typewriterEffect.dialougeText.Add(dialouge);
            nextInteractor.SetActive(true);
            this.gameObject.SetActive(false);
        }
        
    }
    void InputIngredient2()
    {
        if (!isInputIngredient2)
            return;
        if (craftingTable == null)
            return;
        if (craftingTable.storedIngredients.Count == craftingTable.slots.Count)
        {
            typewriterEffect.dialougeText.Add(dialouge);
            nextInteractor.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    void StartMachine()
    {
        if (!isStartMachine)
            return;
        if (craftingTable == null)
            return;
        if (craftingTable.crafting == true)
        {
            typewriterEffect.dialougeText.Add(dialouge);
            nextInteractor.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    void StopMachine()
    {
        if (!isStopMachine)
            return;
        if (craftingTable == null)
            return;
        if (craftingTable.finishFirstCraft == true)
        {
            typewriterEffect.dialougeText.Add(dialouge);
            nextInteractor.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
    void Skip()
    {
        if (!skip)
            return;
        
        typewriterEffect.dialougeText.Add(dialouge);
        nextInteractor.SetActive(true);
        this.gameObject.SetActive(false);
        
    }
}
