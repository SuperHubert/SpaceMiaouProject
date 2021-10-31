using System.Collections;
using UnityEngine;

public interface IEnemy
{
    void OnTriggerZoneEnter();
    void Activate();
    void DeActivate();
}
