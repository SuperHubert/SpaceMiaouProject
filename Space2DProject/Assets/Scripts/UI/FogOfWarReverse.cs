using UnityEngine;
using UnityEngine.UI;

public class FogOfWarReverse : MonoBehaviour
{
    // Component for the mask
    public RawImage rawImage;
    public RawImage rawImageMinimap;
    // Place to draw our revealing smudges
    public RenderTexture targetRender;
    public RectTransform targetImage;

    public InputManager inputManager;
    public int levelSize;
    public int revealSize;

    Texture2D texture;

    private Camera main;
    private Vector2 localPoint;

    public int circleRadius = 5;
    private int circleRadiusAfter;

    private MapIcons mapIcons;
    
    void Start()
    {
        mapIcons = gameObject.GetComponent<MapIcons>();
        ClearFog();
    }

    public void UpdateMapFog(Vector2 point)
    {
        if (levelSize == 0) return;
        localPoint = point * 330 / levelSize + Vector2.one * 330;
        RenderTexture.active = targetRender;


        // Draw circle to reveal content
        int xPos = Mathf.RoundToInt((localPoint.x / targetImage.sizeDelta.x) * (float)targetRender.width);
        int yPos = Mathf.RoundToInt((localPoint.y / targetImage.sizeDelta.y) * (float)targetRender.height);

        revealSize = (int)((25f/(float)(levelSize - 10))*80f);
        circleRadiusAfter = circleRadius * revealSize;
            
        for (int i = 0; i < circleRadiusAfter*2; i++)
        {
            for (int j = 0; j < circleRadiusAfter*2; j++)
            {
                int targetX = i - circleRadiusAfter;
                int targetY = j - circleRadiusAfter;

                float distance = Mathf.Sqrt((targetX * targetX) + (targetY * targetY));
                if (distance < circleRadiusAfter)
                {
                    texture.SetPixel(xPos+targetX, yPos+targetY, Color.clear);
                }
            }    
        }
            
        // Don't forget to apply the result
        texture.Apply();

        RenderTexture.active = null;
        
        mapIcons.UpdateJonesIconPos(levelSize);
    }

    public void ClearFog()
    {
        texture = new Texture2D (targetRender.width, targetRender.height);
        var clearColors = new Color[targetRender.width * targetRender.height];
        
        // clear initial image
        for (int i = 0; i < clearColors.Length; i++)
        {
            clearColors[i] = Color.black;
        }
        texture.SetPixels(0,0,targetRender.width, targetRender.height, clearColors);
        texture.Apply();
        
        rawImage.texture = texture;
        rawImage.texture = texture;
    }
}
