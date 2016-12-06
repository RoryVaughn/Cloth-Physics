using UnityEngine;
using System.Collections;

public class LineConnect : MonoBehaviour {

    public Agent P1;
    public Agent P2;
    private LineRenderer Line;

    // Use this for initialization
    void Awake () {
        Line = gameObject.GetComponent<LineRenderer>();
        Line.SetWidth(0.1f,0.1f);
        Line.SetColors(Color.black, Color.red);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        Line.SetPosition(0, P1.Position);
        Line.SetPosition(1, P2.Position);
    }
}
