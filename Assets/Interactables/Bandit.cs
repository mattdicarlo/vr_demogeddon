using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour {

    private Lever _lever;
    private BanditScreen _screen;
    private bool _isShowingResults;

	// Use this for initialization
	void Start () {
        _isShowingResults = false;
        _lever = GetComponentInChildren<Lever>();
        _screen = GetComponentInChildren<BanditScreen>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (_lever.Value >= 1 && !_isShowingResults) {
            StartCoroutine(ResultsDisplay());
        }
	}

    IEnumerator ResultsDisplay() {
        if (_isShowingResults) {
            yield break;
        }

        _isShowingResults = true;
        if (Random.Range(0, 100) > 90) { 
            _screen.setToWin();
        }
        else {
            _screen.setToLose();
        }

        yield return new WaitForSeconds(5);
        _screen.setToBase();
        _isShowingResults = false;
    }
}
