using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    [SerializeField] private int index = -1;

    [SerializeField] private float timeBetweenLetters = 0.005f;
    private Coroutine typingCoroutine;
    private Coroutine soundCoroutine;
    private bool isDoneTyping = true;
    private AudioManager am;
    
    [System.Serializable] public class CinematicItem
    {
        public Sprite sprite;
        public string description = "does something";
    }

    public List<CinematicItem> itemList = new List<CinematicItem>();
    
    void Start()
    {
        am = AudioManager.Instance;
        
        LoadingManager.Instance.UpdateLoading(2);
        if (LoadingLevelData.hasLaunchedGame)
        {
            GoToImage(3);
            AudioManager.Instance.Stop(5, true);
            
        }
        else
        {
            StartCoroutine(BeginCutscene());
        }
    }
    
    void Update()
    {
        if(!image.gameObject.activeSelf) return;
        if (Input.GetKeyDown(KeyCode.A))
        {
            PreviousImage();
            return;
        }
        if(!Input.anyKeyDown) return;
        NextImage();
    }

    public void PreviousImage()
    {
        index--;
        if (index < 0) index = 0;
        image.sprite = itemList[index].sprite;
        
        if(typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(itemList[index].description));
        if(soundCoroutine != null) StopCoroutine(soundCoroutine);
        soundCoroutine = StartCoroutine(PlayTypingSound());
    }

    private void GoToImage(int number)
    {
        image.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        index = number;
        image.sprite = itemList[number].sprite;
        text.text = itemList[number].description;
        
        if(typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(itemList[number].description));
        if(soundCoroutine != null) StopCoroutine(soundCoroutine);
        soundCoroutine = StartCoroutine(PlayTypingSound());
    }
    
    public void NextImage()
    {
        if (isDoneTyping)
        {
            index++;
            if (index >= 6)
            {
                SceneManager.LoadScene(7);
                return;
            }
        
            if (index > 2 && !LoadingLevelData.hasLaunchedGame)
            {
                LoadingLevelData.hasLaunchedGame = true;
                LoadingLevelData.skipCinematic = true;
                SceneManager.LoadScene(3);
                return;
            }
            if (index >= itemList.Count) index = itemList.Count - 1;
            image.sprite = itemList[index].sprite;

            if(typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeSentence(itemList[index].description));
            if(soundCoroutine != null) StopCoroutine(soundCoroutine);
            soundCoroutine = StartCoroutine(PlayTypingSound());
        }
        else
        {
            if(typingCoroutine != null) StopCoroutine(typingCoroutine);
            if(soundCoroutine != null) StopCoroutine(soundCoroutine);
            isDoneTyping = true;
            text.text = itemList[index].description;
        }
        
    }
    
    private IEnumerator TypeSentence(string sentence)
    {
        text.text = "";
        isDoneTyping = false;
        foreach (char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(timeBetweenLetters);;
        }
        isDoneTyping = true;
    }
    
    private IEnumerator PlayTypingSound()
    {
        do
        {
            am.Play(index != 5 ? 0 : 23);
            yield return new WaitForSeconds(0.2f);
        } while (!isDoneTyping);
        
    }

    private IEnumerator BeginCutscene()
    {
        yield return new WaitForSeconds(1.5f);
        GoToImage(0);
    }
}