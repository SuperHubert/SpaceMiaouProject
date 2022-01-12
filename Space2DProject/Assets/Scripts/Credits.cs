using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private RectTransform credits;
    [SerializeField] private GameObject thanks;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (LoadingLevelData.Instance.creditsGoToMenu)
        {
            credits.position = Vector3.up * 360 + Vector3.right * 640;
        }
        else
        {
            credits.position = Vector3.up * -360 + Vector3.right * 640;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        ReturnToHub();
        MoveCredits();
    }

    private void ReturnToHub()
    {
        if (Input.anyKeyDown)
        {
            if (thanks.activeSelf || LoadingLevelData.Instance.creditsGoToMenu)
            {
                SceneManager.LoadScene(LoadingLevelData.Instance.creditsGoToMenu ? 2 : 3);
            }
            
        }
    }

    private void MoveCredits()
    {
        if(thanks.activeSelf || LoadingLevelData.Instance.creditsGoToMenu) return;
        if (credits.position.y < 990)
        {
            credits.position += Vector3.up * speed * Time.deltaTime;
        }
        else if(!thanks.activeSelf)
        {
            thanks.SetActive(true);
        }
    }
}
