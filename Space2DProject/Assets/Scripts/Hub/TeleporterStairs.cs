using UnityEngine;

public class TeleporterStairs : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("bonkEnter");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("bonkExit");
    }
}
