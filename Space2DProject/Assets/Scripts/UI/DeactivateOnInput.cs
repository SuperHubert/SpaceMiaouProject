using UnityEngine;

public class DeactivateOnInput : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    void Update()
    {
        if (!Input.anyKeyDown) return;
        gameObject.SetActive(false);
        if(pauseUI != null )pauseUI.SetActive(true);
    }
}
