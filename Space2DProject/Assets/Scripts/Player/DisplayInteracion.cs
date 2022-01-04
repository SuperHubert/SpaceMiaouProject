using UnityEngine;

public class DisplayInteracion : MonoBehaviour
{
    [SerializeField] private GameObject pressButton;
    private GameObject objectToInteractWith;
    private bool canInteract = false;

    [HideInInspector] public bool interact;

    private void Update()
    {
        if (!interact || !canInteract) return;
        
        objectToInteractWith.GetComponent<IInteractible>().OnInteraction();
        pressButton.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(DialogueManager.Instance.dialogueCanvas.activeSelf) return;
        if (other.GetComponent<IInteractible>() == null) return;
        
        pressButton.SetActive(canInteract = true);
        objectToInteractWith = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IInteractible>() != null)
        {
            pressButton.SetActive(canInteract = false);
        }
    }
}
