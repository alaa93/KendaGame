using UnityEngine;

public class ResponsiveSprite : MonoBehaviour
{
    public Vector2 originalResolution = new Vector2(2340f, 1080); // Example original resolution
    public Vector2 targetResolution = new Vector2(1920, 1080f); // Example target resolution

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        MakeSpriteResponsive();
    }

    private void MakeSpriteResponsive()
    {
        // Calculate the scale factor for both width and height
        float scaleX = Mathf.Clamp(targetResolution.x / originalResolution.x, 0.1f, 1f);
        float scaleY = Mathf.Clamp(targetResolution.y / originalResolution.y, 0.1f, 1f);

        // Apply the scale to the mesh renderer's local scale
        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
