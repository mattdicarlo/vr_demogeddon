using UnityEngine;
using System.Collections;

public class KeypressListener : MonoBehaviour
{
    public KeyCode triggerKey;

    public GameObject[] gameObjectsToActivate;
    public Animator[] animatorsToStart;

    public void Awake()
    {
        foreach (GameObject go in gameObjectsToActivate)
        {
            go.SetActive(false);
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(triggerKey))
        {
            foreach (GameObject go in gameObjectsToActivate)
            {
                go.SetActive(true);
            }

            foreach (Animator animator in animatorsToStart)
            {
                animator.SetBool("Start", true);
            }
        }
    }
}
