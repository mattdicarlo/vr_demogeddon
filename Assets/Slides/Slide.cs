using UnityEngine;
using UnityEngine.UI;

public class Slide : Tossable
{
    public Texture SlideTexture
    {
        get
        {
            if (projectedTexture == null)
            {
                return slideImage.texture;
            }
            return projectedTexture;
        }
    }
    public RawImage slideImage;

    public Texture projectedTexture;

    public SlideProjector slideProjector;

    public override void OnJointBreak(float breakForce)
    {
        if (slideProjector)
        {
            slideProjector.NotifyJointBroken();
        }
    }
}
