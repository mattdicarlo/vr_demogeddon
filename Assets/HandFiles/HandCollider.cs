using UnityEngine;

public class HandCollider : MonoBehaviour {

    private Throw _controller;

    public Throw ConnectedHand
    {
        set { _controller = value; }
    }

    public int vibeInterval = 2;
    private int _frameCount;

    public void OnCollisionEnter(Collision collision)
    {
        _frameCount = 0;
    }

    public void OnCollisionStay(Collision collision)
    {
        _frameCount++;
        if (_controller != null)
        {
            if (_frameCount % vibeInterval == 0)
            {
                _controller.ForceFeedback(collision.relativeVelocity.magnitude);
            }
        }
    }
}
