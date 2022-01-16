using UnityEngine;

public class HubAudio : MonoBehaviour
{
    private AudioManager am;
    
    void Start()
    {
        am = AudioManager.Instance;
        am.StopAllSounds();
        am.Play(5);
    }
    
}
