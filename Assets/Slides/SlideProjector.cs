using UnityEngine;

public class SlideProjector : MonoBehaviour
{
    private Slide currentSlide;
    private Slide _selected;

    [SerializeField]
    private Lever _nextSlideLever;

    [SerializeField]
    private Transform _slideSlot;

    private Rigidbody _rigidbody;

    [SerializeField]
    private Material screenMaterial;

    private Texture _origScreenTexture;

    public void Awake()
    {
        _origScreenTexture = screenMaterial.GetTexture("_ShadowTex");
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Left mouse clicked");
            NextSlide();
        }
    }

    public void FixedUpdate()
    {
        _selected = SelectNearbySlide();

        if (currentSlide == null && _selected != null)
        {
            currentSlide = _selected;
            currentSlide.Transform.position = _slideSlot.position;
            var joint = currentSlide.CreateGrabJoint();
            joint.connectedBody = _rigidbody;
            joint.enableCollision = false;
            joint.breakForce = 1000.0f;
            currentSlide.Rigidbody.useGravity = false;
            currentSlide.slideProjector = this;
        }

        if (_nextSlideLever.Value > 0.8f)
        {
            NextSlide();
        }
    }

    public void NextSlide()
    {
        if (currentSlide != null)
        {
            screenMaterial.SetTexture("_ShadowTex", currentSlide.SlideTexture);
        }
        else
        {
            screenMaterial.SetTexture("_ShadowTex", _origScreenTexture);
        }
    }

    public void OnDisable()
    {
        screenMaterial.SetTexture("_ShadowTex", _origScreenTexture);
    }

    private Slide SelectNearbySlide()
    {
        // Collider[] nearHandObjects = Physics.OverlapSphere(transform.position + (0.05f * transform.forward) + (-0.05f * transform.up), 0.15f);
        Collider[] nearProjectorObjects = Physics.OverlapSphere(_slideSlot.position, 0.15f);
        foreach (Collider col in nearProjectorObjects)
        {
            if (col.GetComponent<Slide>() != null)
            {
                return col.GetComponent<Slide>();
            }
        }
        return null;
    }

    public void NotifyJointBroken()
    {
        currentSlide.Rigidbody.useGravity = true;
        currentSlide.slideProjector = null;
    }
}
