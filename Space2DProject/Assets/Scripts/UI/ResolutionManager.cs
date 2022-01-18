using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResetRes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetRes()
    {
        Screen.SetResolution(1280,720,true);
    }
}
