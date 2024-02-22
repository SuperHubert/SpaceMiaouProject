using System.Collections.Generic;
using UnityEngine;

public class MapIcons : MonoBehaviour
{
    public RectTransform jonesIcon;
    public RectTransform shopIcon;
    public RectTransform towerIcon;
    public RectTransform portalIcon;
    public RectTransform jonesIconMinimap;
    public RectTransform shopIconMinimap;
    public RectTransform towerIconMinimap;
    public RectTransform portalIconMinimap;
    private Transform jonesTrans;
    [SerializeField] private GameObject chestIconPrefab;
    [SerializeField] private Transform chestParent;
    [SerializeField] private Transform chestParentMinimap;
    public List<Transform> chestList;
    
    void Start()
    {
        jonesTrans = LevelManager.Instance.Player().transform;
    }

    public void UpdateJonesIconPos(float levelSize)
    {
        var position = (Vector2) jonesTrans.position * 330 / levelSize;
        jonesIcon.localPosition = position;
        jonesIconMinimap.localPosition = position;
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
            position = (Vector2) chest.position * 330 / levelSize;
            chest.GetComponent<Chest>().linkedIcon =
                Instantiate(chestIconPrefab, position, Quaternion.identity, chestParent);
            chest.GetComponent<Chest>().linkedIcon.transform.localPosition = position;
            chest.GetComponent<Chest>().linkedIconMinimap =
                Instantiate(chestIconPrefab, position, Quaternion.identity, chestParentMinimap);
            chest.GetComponent<Chest>().linkedIconMinimap.transform.localPosition = position;
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

        foreach (Transform icon in chestParentMinimap)
        {
            Destroy(icon.gameObject);
        }

    }

    public void MoveShopIcon(Transform shopTransform, int levelSize,bool active)
    {
        Vector2 position = (Vector2) shopTransform.position * 330 / levelSize;
        if(shopIcon == null) return;
        shopIcon.localPosition = position;
        shopIconMinimap.localPosition = position;
        shopIcon.gameObject.SetActive(active);
        
    }
    
    public void MoveTowerIcon(Transform towerTransform, int levelSize)
    {
        Vector2 position = (Vector2) towerTransform.position * 330 / levelSize;
        if(towerIcon == null) return;
        towerIcon.localPosition = position;
        towerIconMinimap.localPosition = position;
    }
    
    public void MovePortalIcon(Transform portalTransform, int levelSize)
    {
        Vector2 position = (Vector2) portalTransform.position * 330 / levelSize;
        if(portalIcon == null) return;
        portalIcon.localPosition = position;
        portalIconMinimap.localPosition = position;
    }
}
