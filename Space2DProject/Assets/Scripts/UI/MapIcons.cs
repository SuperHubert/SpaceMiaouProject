using System.Collections.Generic;
using UnityEngine;

public class MapIcons : MonoBehaviour
{
    public RectTransform jonesIcon;
    public RectTransform shopIcon;
    public RectTransform towerIcon;
    public RectTransform portalIcon;
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
        var position = (Vector2) jonesTrans.position * 330 / levelSize + new Vector2(640, 360);
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
            position = (Vector2) chest.position * 330 / levelSize + new Vector2(640, 360);
            chest.GetComponent<Chest>().linkedIcon =
                Instantiate(chestIconPrefab, position, Quaternion.identity, chestParent);
        }

        chestList.Clear();
    }

    public void ClearChestsIcons()
    {
        if (chestParent.childCount == 0) return;
        foreach (Transform icon in chestParent)
        {
            Destroy(icon.gameObject);
        }
    }

    public void MoveShopIcon(Transform shopTransform, int levelSize,bool active)
    {
        Vector2 position = (Vector2) shopTransform.position * 330 / levelSize + new Vector2(640, 360);
        if(shopIcon == null) return;
        shopIcon.position = position;
        shopIcon.gameObject.SetActive(active);
    }
    
    public void MoveTowerIcon(Transform towerTransform, int levelSize)
    {
        Vector2 position = (Vector2) towerTransform.position * 330 / levelSize + new Vector2(640, 360);
        if(towerIcon == null) return;
        towerIcon.position = position;
    }
    
    public void MovePortalIcon(Transform portalTransform, int levelSize)
    {
        Vector2 position = (Vector2) portalTransform.position * 330 / levelSize + new Vector2(640, 360);
        if(portalIcon == null) return;
        portalIcon.position = position;
    }
}
