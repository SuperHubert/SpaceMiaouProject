using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public Vector2 teleportSpot;
    
    public void ResetTeleportSpot()
    {
        teleportSpot = player != null ? player.transform.position : player.transform.position;
    }
}
