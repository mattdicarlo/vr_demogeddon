using UnityEngine;
using UnityEngine.UI;

public class Slide : Tossable
{
    public Texture SlideTexture
    {
        get { return slideImage.texture; }
    }
    public RawImage slideImage;
}
