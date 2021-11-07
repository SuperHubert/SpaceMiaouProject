using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickableUpgrade : MonoBehaviour, IInteractible
{
    [SerializeField] private UnityEvent upgrade = new UnityEvent();
    
    public void OnInteraction()
    {
        upgrade.Invoke();
        Destroy(gameObject);
    }
}