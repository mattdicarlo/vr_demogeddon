using UnityEngine;

public class MonSpawner : MonoBehaviour
{
    public GameObject MonPrefab;
    public KeyCode triggerKey;

    public void Start ()
    {

    }

    public void Update ()
    {
        if (Input.GetKeyUp(triggerKey))
        {
            SpawnMon();
        }
    }

    private void SpawnMon()
    {
        Vector3 rndPosWithin;
        rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
        Instantiate(MonPrefab, rndPosWithin, transform.rotation);
    }
}
