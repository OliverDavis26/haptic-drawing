using UnityEngine;
using UnityEngine.Rendering;

public class Whiteboard : MonoBehaviour
{
    public RenderTexture renderTexture;
    private CommandBuffer commandBuffer;
    private Texture2D drawnPixels;

    void Start()
    {
        // Create the command buffer
        commandBuffer = new CommandBuffer();
        commandBuffer.name = "Render Whiteboard";

        // Set up the command buffer to render the whiteboard to the RenderTexture
        commandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, renderTexture);

        // Add the command buffer to the camera
        Camera.main.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
    }

    void Update()
    {
        // Allocate the Texture2D to hold the drawn pixels
        drawnPixels = new Texture2D(renderTexture.width, renderTexture.height);

        // Copy the drawn pixels from the RenderTexture to the Texture2D
        RenderTexture.active = renderTexture;
        drawnPixels.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        drawnPixels.Apply();
    }
}
