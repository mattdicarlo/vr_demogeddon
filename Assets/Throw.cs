using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class Throw : MonoBehaviour
{
    public const ushort MAX_PULSE_LENGTH = 3999;

    [SerializeField]
    public IGrabbable selected;

    public Transform attachPoint;

    SteamVR_TrackedObject trackedObj;
    Joint joint;
    Rigidbody throwRigidbody;

    public HandCollider handCollider;
    public GameObject handModel;

    Animator handAnimator;

    private bool heldItemShouldUseGravity = true;

    public ushort vibrationBaseTime = 1000;

    // Editor Testing Flags
    public bool fake_trigger;
    private bool trigger_down;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        throwRigidbody = handCollider.gameObject.GetComponent<Rigidbody>();
        handAnimator = handModel.GetComponent<Animator>();
        handCollider.ConnectedHand = this;

        fake_trigger = false;
        trigger_down = false;
    }

    void FixedUpdate()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);

        // Find and highlight a selected object
        selected = SelectNearbyObject();

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) || (!trigger_down && fake_trigger))
        {
            trigger_down = true;
            fake_trigger = false;

            handAnimator.SetBool("ShouldGrip", true);
            if (joint == null && selected != null)
            {
                var heldItem = selected;
                if (heldItem.ConnectedHand != null)
                {
                    heldItem.ConnectedHand.DropObject(heldItem, 0);
                }
                heldItem.ConnectedHand = this;

                if (heldItem.MoveToGrabberWhenGrabbed)
                {
                    var collider = heldItem.Transform.GetComponent<Collider>();
                    if (collider && collider.GetType() == typeof(SphereCollider))
                    {
                        var sphere = collider as SphereCollider;
                        var direction = -attachPoint.up;
                        // this is gross
                        var translation = direction.normalized * (sphere.radius * heldItem.Transform.localScale.x);
                        heldItem.Transform.position = attachPoint.position + translation;
                    }
                    //if (collider && collider.GetType() == typeof(BoxCollider))
                    //{
                    //    var box = collider as BoxCollider;
                    //    var direction = -attachPoint.up;
                    //    var maxbound = Mathf.Max(new float[] { box.bounds.max.x, box.bounds.max.y, box.bounds.max.z });
                    //    var translation = direction.normalized * maxbound;
                    //    heldItem.Transform.position = attachPoint.position + translation;
                    //}
                    else
                    {
                        heldItem.Transform.position = attachPoint.position;
                    }
                }
                joint = heldItem.CreateGrabJoint();
                joint.connectedBody = throwRigidbody;
                joint.enableCollision = false;

                heldItemShouldUseGravity = heldItem.Rigidbody.useGravity;
                heldItem.Rigidbody.useGravity = false;
                joint.breakForce = heldItem.BreakForce;

                ForceFeedback(50);
                Debug.Log(name + " FixedUpdate heldItemShouldUseGravity=" + heldItemShouldUseGravity);
            }
        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) || (trigger_down && fake_trigger))
        {
            trigger_down = false;
            fake_trigger = false;

            handAnimator.SetBool("ShouldGrip", false);
            if (joint != null)
            {
                StartCoroutine(ThrowObject(device, heldItemShouldUseGravity));
            }
        }
    }

    private IEnumerator ThrowObject(SteamVR_Controller.Device device, bool shouldUseGravity)
    {
        Debug.Log(name + " ThrowObject shouldUseGravity=" + shouldUseGravity);
        var go = joint.gameObject;

        var rigidbody = go.GetComponent<Rigidbody>();
        rigidbody.useGravity = shouldUseGravity;
        Object.Destroy(joint);
        joint = null;

        yield return null;

        var grabbable = go.GetComponent<IGrabbable>();
        grabbable.ConnectedHand = null;

        // We should probably apply the offset between trackedObj.transform.position
        // and device.transform.pos to insert into the physics sim at the correct
        // location, however, we would then want to predict ahead the visual representation
        // by the same amount we are predicting our render poses.

        var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
        if (origin != null)
        {
            rigidbody.velocity = origin.TransformVector(device.velocity);
            rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
        }
        else
        {
            rigidbody.velocity = device.velocity;
            rigidbody.angularVelocity = device.angularVelocity;
        }

        rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;


    }

    public void DropObject(IGrabbable objToDrop, float breakForce)
    {
        Debug.Log(name + " DropObject heldItemShouldUseGravity=" + heldItemShouldUseGravity);
        if (objToDrop != null)
        {
            objToDrop.ConnectedHand = null;
            if (joint)
            {
                Destroy(joint);
                joint = null;
            }
            objToDrop.Rigidbody.useGravity = heldItemShouldUseGravity;
        }
    }

    private IGrabbable SelectNearbyObject()
    {
        Collider[] nearHandObjects = Physics.OverlapSphere(transform.position + (0.05f * transform.forward) + (-0.05f * transform.up), 0.15f);
        foreach (Collider col in nearHandObjects)
        {
            //Debugging the 'Catch" collider range. Also a cool "painting" of cubes
            //GameObject hit = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //DestroyImmediate(hit.GetComponent<Collider>());
            //hit.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            //hit.transform.position = col.transform.position;
            //Destroy(hit, 0.5f);

            if (col.GetComponent<IGrabbable>() != null && col.gameObject.tag != "nonpickup")
            {
                return col.GetComponent<IGrabbable>();
            }
        }
        return null;
    }

    private bool _isForceFeedbackCoroutineRunning = false;
    private ushort _nextFeedbackValue = 0;

    public void ForceFeedback(float forceStrength)
    {
        ushort pulseLength = (ushort)(vibrationBaseTime * forceStrength);

        if (_isForceFeedbackCoroutineRunning)
        {
            _nextFeedbackValue = (ushort)Mathf.Max(pulseLength, _nextFeedbackValue);
        }
        else
        {
            _isForceFeedbackCoroutineRunning = true;
            _nextFeedbackValue = pulseLength;
            StartCoroutine(VibrateCoroutine());
        }
    }

    private IEnumerator VibrateCoroutine()
    {
        while (_nextFeedbackValue > 0)
        {
            ushort pulseLength = (ushort)Mathf.Min(_nextFeedbackValue, MAX_PULSE_LENGTH);
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(pulseLength);
            _nextFeedbackValue -= pulseLength;
            yield return null;
        }
        _isForceFeedbackCoroutineRunning = false;
    }
}
