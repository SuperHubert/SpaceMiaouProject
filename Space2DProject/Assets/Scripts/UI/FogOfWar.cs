using UnityEngine;
using UnityEngine.UI;

public class FogOfWar : MonoBehaviour
{

    // Component for the mask
    public RawImage rawImage;
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
    
    void Start()
    {
        ClearFog();
    }

    public void UpdateMapFog(Vector2 point)
    {
        
        localPoint = point * 330 / levelSize + Vector2.one * 330;
        // set our current render texture for drawing
        RenderTexture.active = targetRender;

        // Draw circle to reveal content
        int xPos = Mathf.RoundToInt((localPoint.x / targetImage.sizeDelta.x) * (float)targetRender.width);
        int yPos = Mathf.RoundToInt((localPoint.y / targetImage.sizeDelta.y) * (float)targetRender.height);
            
        float circle = 2 * Mathf.PI * 5;

        //revealSize = ((levelSize - 10)/25) * 70;
        revealSize = (int)((25f/(float)(levelSize - 10))*70f);
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
                    texture.SetPixel(xPos+targetX, yPos+targetY, Color.black);
                }
            }    
        }
            
        // Don't forget to apply the result
        texture.Apply();

        RenderTexture.active = null;
    }

    public void ClearFog()
    {
        texture = new Texture2D (targetRender.width, targetRender.height);
        var clearColors = new Color[targetRender.width * targetRender.height];
        
        // clear initial image
        for (int i = 0; i < clearColors.Length; i++)
        {
            clearColors[i] = Color.clear;
        }
        texture.SetPixels(0,0,targetRender.width, targetRender.height, clearColors);
        texture.Apply();
        
        rawImage.texture = texture;
    }
}
