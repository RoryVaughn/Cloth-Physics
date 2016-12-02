using UnityEngine;
using System.Collections;

public class LineConnect : MonoBehaviour {

    public GameObject P1;
    public GameObject P2;
    private LineRenderer Line;
    public GameObject Spawn;
    // Use this for initialization
    void Awake () {
        Line = gameObject.GetComponent<LineRenderer>();

                P1 = transform.Find("P1").gameObject;

                P2 = transform.Find("P2").gameObject;

     
        Line.SetPosition(0, P1.transform.position);
        Line.SetPosition(1, P2.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        Line.SetPosition(0, P1.transform.position);
        Line.SetPosition(1, P2.transform.position);
    }
}
