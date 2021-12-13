using UnityEngine;

public class OnTriggerEXecute : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != 6) return;
        StartCoroutine(LevelManager.Instance.Player().GetComponent<Fall>().TeleportFollower());
    }
}
