using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        animator.Play("Trigger");
    }
    
}
