using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();

    #region Singleton

    public static CombatManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    void Start()
    {
        enemyList.Clear();
    }

    public void Add(GameObject obj)
    {
        if(!enemyList.Contains(obj)) enemyList.Add(obj);
    }
    
    public void Remove(GameObject obj)
    {
        if(enemyList.Contains(obj)) enemyList.Remove(obj);
    }

    public void Clear()
    {
        enemyList.Clear();
    }

    public bool IsEmpty()
    {
        return enemyList.Count == 0;
    }
}
