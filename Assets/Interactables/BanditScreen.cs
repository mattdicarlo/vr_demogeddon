using UnityEngine;

public class BanditScreen : MonoBehaviour {
    public Material _baseImage;
    public Material _winImage;
    public Material _loseImage;

    // Use this for initialization
    void Start () {
        setToBase();
    }

    // Update is called once per frame
    void Update () {

    }

    public void setToBase() {
        GetComponent<Renderer>().material = _baseImage;
    }

    public void setToWin()
    {
        GetComponent<Renderer>().material = _winImage;
    }

    public void setToLose()
    {
        GetComponent<Renderer>().material = _loseImage;
    }
}
