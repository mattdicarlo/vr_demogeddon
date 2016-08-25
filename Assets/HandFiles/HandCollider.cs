using UnityEngine;

public class HandCollider : MonoBehaviour {

    private Throw _controller;

    public Throw ConnectedHand
    {
        set { _controller = value; }
    }

    public void OnCollisionEnter(Collision collision)
    {
    }

    public void OnCollisionStay(Collision collision)
    {
        if (_controller != null)
        {
            _controller.ForceFeedback(collision.relativeVelocity.magnitude);
        }
    }
}
