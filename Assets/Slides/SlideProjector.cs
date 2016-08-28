using UnityEngine;

public class SlideProjector : MonoBehaviour
{
    public Slide currentSlide;

    [SerializeField]
    private Material screenMaterial;

    private Texture _origScreenTexture;

    public void Awake()
    {
        _origScreenTexture = screenMaterial.GetTexture("_ShadowTex");
    }

    public void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Left mouse clicked");
            NextSlide();
        }
    }

    public void NextSlide()
    {
        if (currentSlide != null)
        {
            screenMaterial.SetTexture("_ShadowTex", currentSlide.SlideTexture);
        }
    }

    public void OnDisable()
    {
        screenMaterial.SetTexture("_ShadowTex", _origScreenTexture);
    }
}
