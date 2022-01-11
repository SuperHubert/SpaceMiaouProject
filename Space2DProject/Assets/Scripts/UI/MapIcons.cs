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

    public void AddChest(GameObject chest)
    {
        chestList.Add(chest.transform);
    }

    public void UpdateChests(float levelSize)
    {
        Vector2 position = Vector2.zero;
        foreach (var chest in chestList)
        {
            position = (Vector2)chest.position * 330 / levelSize + new Vector2(640, 360);
            chest.GetComponent<Chest>().linkedIcon = Instantiate(chestIconPrefab, position, Quaternion.identity, chestParent);
        }
        chestList.Clear();
    }

    public void ClearChestsIcons()
    {
        if(chestParent.childCount == 0) return;
        foreach (Transform icon in chestParent)
        {
            Destroy(icon.gameObject);
        }
    }
    
    
}
