using UnityEngine;
using UnityEngine.Events;

public class InteractibleDialogue : MonoBehaviour, IInteractible
{
    [SerializeField] private UnityEvent dialogue = new UnityEvent();

    public void OnInteraction()
    {
        dialogue.Invoke();
    }
}
