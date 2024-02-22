using UnityEngine;

public class HubCameraStart : MonoBehaviour
{
    private Camera cam;
    private Transform camTransform;
    private bool hasExited = false;
    
    private void Start()
    {
        Time.timeScale = 1;
        cam = Camera.main;
        camTransform = cam.transform;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("bonk");
        if(hasExited) return;
        hasExited = true;
        var components = gameObject.GetComponentsInChildren<MoveHubCamera>();
        foreach (MoveHubCamera component in components)
        {
            component.enabled = true;
        }
    }
}
