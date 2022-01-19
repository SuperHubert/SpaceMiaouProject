using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    void Start()
    {
        ResetRes();
    }
    
    public void ResetRes()
    {
        Screen.SetResolution(1920,1080,FullScreenMode.MaximizedWindow);
    }
}
