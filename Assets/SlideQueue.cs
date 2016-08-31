using UnityEngine;
using System.Collections;

public class SlideQueue : MonoBehaviour {

    public KeyCode triggerKey;

    public Slide[] slides;

    public SlideProjector slideProjector;

    private int nextIndex;

    public void Awake()
    {
        nextIndex = 0;
    }

    void Update ()
    {
        if (Input.GetKeyUp(triggerKey))
        {
            if (nextIndex < slides.Length)
            {
                var go = slides[nextIndex];
                slideProjector.SpawnSlide(go);
                nextIndex++;
            }
        }
    }
}
