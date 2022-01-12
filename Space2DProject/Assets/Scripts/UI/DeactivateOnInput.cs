using UnityEngine;

public class DeactivateOnInput : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            gameObject.SetActive(false);
        }
    }
}
