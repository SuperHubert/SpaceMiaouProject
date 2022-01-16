using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MoveHubCamera : MonoBehaviour
{
    private Transform camTransform;
    private Camera cam;
    public Vector3 pos;
    private Vector3 offset = new Vector3(0, 0, -10f);
    public float cameraSize = 6f;

    private void Start()
    {
        cam = Camera.main;
        camTransform = cam.transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        camTransform.DOMove(pos + offset,1f);
        cam.DOOrthoSize(cameraSize, 1f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        camTransform.DOMove(offset, 1f);
        cam.DOOrthoSize(6f, 1f);
    }
}
