using UnityEngine;

public class ActivateTeleporterHitbox : MonoBehaviour
{
    public GameObject colInside;
    public GameObject colOutside;

    private void OnTriggerEnter2D(Collider2D other)
    {
        colInside.SetActive(true);
        colOutside.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        colInside.SetActive(false);
        colOutside.SetActive(true);
    }
}
