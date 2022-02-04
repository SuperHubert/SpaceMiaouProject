using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    private enum Trigger {WakeUp, Sleep, Respawn, Action}

    [SerializeField] private Trigger state;
    [SerializeField] private bool showTrigger;
    private EnemyBehaviour enemy;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        if(sprite != null) sprite.enabled = showTrigger;
        
        var parent = transform.parent;
        enemy = state == Trigger.Respawn ? parent.gameObject.GetComponent<EnemyBehaviour>() : parent.parent.parent.gameObject.GetComponent<EnemyBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (state)
        {
            case Trigger.WakeUp:
                enemy.WakeUp();
                break;
            case Trigger.Action:
                enemy.ExecuteAction();
                break;
            case Trigger.Sleep:
                break;
            case Trigger.Respawn:
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (state)
        {
            case Trigger.Sleep:
                enemy.Sleep();
                break;
            case Trigger.Respawn:
                enemy.Respawn();
                break;
            case Trigger.WakeUp:
                break;
            case Trigger.Action:
                break;
            default:
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (state != Trigger.Action) return;
        enemy.ExecuteAction();
    }

    public bool ToggleShowTrigger()
    {
        showTrigger = !showTrigger;
        sprite.enabled = showTrigger;
        return sprite.enabled;
    }
}
