using System.Collections.Generic;
using UnityEngine;

public class SecretDoor : MonoBehaviour, IInteractible
{
    public List<Dialogues> dialogues;

    public void OnInteraction()
    {
        DialogueManager.Instance.StartDialogue(LoadingLevelData.bossDead ? dialogues[1] : dialogues[0]);
    }
}
