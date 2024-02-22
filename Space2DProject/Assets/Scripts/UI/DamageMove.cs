using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageMove : MonoBehaviour
{
    public Transform linkedEnemy;
    public int damage;
    [SerializeField] private int lifetime;
    private TextMeshProUGUI text;
    private Vector3 position;
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    
    void Update()
    {
        if(linkedEnemy == null) return;

        lifetime--;
        if(lifetime < 0) Destroy(gameObject);
        
        position = linkedEnemy.position;

        position.z = 0f;
        position.y += 0;

        position = cam.WorldToScreenPoint(position);

        transform.position = position;

        text.text = damage.ToString();
    }
}
