using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanGoBehind : MonoBehaviour
{
    public Transform player;
    public float offset = 0;
    private SpriteRenderer renderer;
    private int baseLayer;

    private void Start()
    {
        if (player == null) player = LevelManager.Instance.Player().transform;
        renderer = gameObject.GetComponent<SpriteRenderer>();
        baseLayer = renderer.sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        renderer.sortingOrder = player.position.y < transform.position.y + offset ? baseLayer : 7;
    }
}
