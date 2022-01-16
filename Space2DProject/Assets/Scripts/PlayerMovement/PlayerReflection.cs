using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReflection : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private SpriteRenderer targerRender;
    private SpriteRenderer renderer;

    
    void Start()
    {
        targerRender = target.GetComponent<SpriteRenderer>();
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        renderer.enabled = targerRender.enabled;
        renderer.sprite = targerRender.sprite;
    }
}
