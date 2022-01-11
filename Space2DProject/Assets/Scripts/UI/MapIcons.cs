using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIcons : MonoBehaviour
{
    public RectTransform jonesIcon;
    private Transform jonesTrans;
    [SerializeField] private GameObject chestIconPrefab;
    [SerializeField] private Transform chestParent;
    public List<Transform> chestList;

    // Start is called before the first frame update
    void Start()
    {
        jonesTrans = LevelManager.Instance.Player().transform;
    }
    
    public void UpdateJonesIconPos(float levelSize)
    {
        var position = (Vector2)jonesTrans.position * 330 / levelSize + new Vector2(640, 360);
        jonesIcon.position = position;
    }

    public void AddChest()
    {
        
    }

    public void UpdateChests()
    {
        
    }

    public void ClearChests()
    {
        
    }
    
    
}
