using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReflection : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private SpriteRenderer targetRender;
    private SpriteRenderer ownRenderer;

    
    void Start()
    {
        targetRender = target.GetComponent<SpriteRenderer>();
        ownRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        ownRenderer.enabled = targetRender.enabled;
        ownRenderer.sprite = targetRender.sprite;
    }

    public void Disable(bool state=false)
    {
        gameObject.SetActive(state);
    }
}
