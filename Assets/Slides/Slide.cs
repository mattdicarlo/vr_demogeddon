using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    public void AnimateMe()
    {
        StartCoroutine(SlideRise());
    }

    public IEnumerator SlideRise()
    {
        Rigidbody.isKinematic = true;
        gameObject.tag = "nonpickup";
        transform.position -= new Vector3(0, .09f, 0);
        float addedY = 0;
        while (addedY < 0.09f)
        {
            addedY += (0.09f / 120f);
            transform.position += new Vector3(0, (0.09f / 120f), 0);

            yield return new WaitForSeconds((1/60f));
        }
        Rigidbody.isKinematic = false;
        gameObject.tag = "Untagged";
    }
}
