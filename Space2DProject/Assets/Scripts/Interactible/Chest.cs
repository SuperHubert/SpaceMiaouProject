using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour, IInteractible
{
    private float[] table = {100f,0f};
    private int floor;
    private Animator anim;
    private Collider2D col;
    public GameObject linkedIcon;
    private AudioManager am;

    private void Start()
    {
        floor = LevelManager.Instance.FloorNumber();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        am = AudioManager.Instance;

    }
    IEnumerator PlayOpenAnim()
    {
        anim.SetTrigger("Open");
        
        float drop = Random.Range(0, 100);
        
        var transform1 = transform;
        if (drop < LootManager.Instance.ConvertLevelToProbability(floor))
        {
            var bonk = LootManager.Instance.GetCoins(floor > 2 ? Random.Range(1, floor + 1) : Random.Range(1, 3),
                transform1.position,transform1);
            yield return new WaitForSeconds(0.75f);
            foreach (var obj in bonk)
            {
                obj.GetComponent<Collider2D>().enabled = true;
            }
        }
        else
        {
            LootManager.Instance.GetUpgrade(floor,transform1.position,transform1);
        }
        
        yield return new WaitForSeconds(0.95f);
    }
    public void OnInteraction()
    {
        col.enabled = false;
        linkedIcon.SetActive(false);
        StartCoroutine(PlayOpenAnim());
        am.Play(27, true);
    }

    public void UpdateChest(MapIcons mapIcons)
    {
        float fortuneUpgrade = Upgrades.Instance.GetFortuneLevel();
        float probabilityToStay = ((floor + (4f + 5 * fortuneUpgrade)) / (floor + (5f + 5 * fortuneUpgrade))) * (0.5f+(fortuneUpgrade/(fortuneUpgrade + 2f))*0.5f);
        
        if (Random.Range(0f, 1f) > probabilityToStay)
        {
            Destroy(gameObject);
        }
        else
        {
            mapIcons.AddChest(gameObject);
        }
        
        var biome = LevelManager.Instance.GetBiome();
        
        switch (biome)
        {
            case 0:
                anim.SetLayerWeight (2, 0f);
                anim.SetLayerWeight (1, 1f);
                break;
            case 1:
                anim.SetLayerWeight (2, 1f);
                anim.SetLayerWeight (1, 0f);
                break;
            case 2:
                anim.SetLayerWeight (2, 1f);
                anim.SetLayerWeight (1, 0f);
                break;
            default:
                anim.SetLayerWeight (2, 0f);
                anim.SetLayerWeight (1, 1f);
                break;
        }
    }
}
