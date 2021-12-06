using System.Collections;
using UnityEngine;

public class EvilTreeBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject rootPrefab;

    private void Update()
    {
        if (currentState != State.Awake) return;

        if (actionCd > 0)
        {
            actionCd--;
        }
    }

    protected override void Action()
    {
        base.Action();
        
        var targetPos = player.position;

        var spawnedObj = ObjectPooler.Instance.SpawnFromPool("Root", targetPos, Quaternion.identity);
        spawnedObj.GetComponent<Root>().Spawn();
    }
}