using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCutout : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    [SerializeField] private LayerMask wallMask;

    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }
    
    void Update()
    {
        Vector2 cutoutPos = camera.WorldToViewportPoint(targetTransform.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = targetTransform.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            foreach (Material m in materials)
            {
                m.SetVector("_CutoutPos",cutoutPos);
                m.SetFloat("_CutoutSize", 0.1f);
                m.SetFloat("_FalloffSize", 0.05f);
            }
        }
    }
}
