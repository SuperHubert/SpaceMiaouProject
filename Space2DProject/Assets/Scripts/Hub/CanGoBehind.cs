using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGoBehind : MonoBehaviour
{
    public Transform player;
    public float offset = 0;
    public int layer = 7;
    private SpriteRenderer ownRenderer;
    private int baseLayer;

    private void Start()
    {
        if (player == null) player = LevelManager.Instance.Player().transform;
        ownRenderer = gameObject.GetComponent<SpriteRenderer>();
        baseLayer = ownRenderer.sortingOrder;
    }

    void Update()
    {
        ownRenderer.sortingOrder = player.position.y < transform.position.y + offset ? baseLayer : layer;
        //renderer.sortingOrder = player.position.y < transform.position.y + offset ? 7 : baseLayer;
    }
}
