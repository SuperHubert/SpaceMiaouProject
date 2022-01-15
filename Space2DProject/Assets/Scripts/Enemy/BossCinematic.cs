using UnityEngine;
using DG.Tweening;

public class BossCinematic : MonoBehaviour
{
    public Camera cam;
    private Transform camTransform;
    public GameObject tower;
    
    // Start is called before the first frame update
    void Start()
    {
        camTransform = cam.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        camTransform.DOMove(new Vector3(0, -5.76f, -10), 2.5f);
        cam.DOOrthoSize(12, 2.5f);
    }
}
