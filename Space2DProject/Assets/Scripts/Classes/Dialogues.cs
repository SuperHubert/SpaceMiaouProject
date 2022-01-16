using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogues
{
    public string characterName;
    public Sprite characterImage;
    
    [TextArea(3,10)]
    public List<string> sentences = new List<string>();

    public int audioSourceIndex = 0;


}
