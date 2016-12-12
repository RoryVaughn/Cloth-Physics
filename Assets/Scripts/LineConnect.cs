using UnityEngine;
using System.Collections;

public class LineConnect : MonoBehaviour
{
    public Agent P1;
    public Agent P2;
    private LineRenderer _line;

    // Use this for initialization
    private void Awake()
    {
        _line = gameObject.GetComponent<LineRenderer>();
        _line.SetWidth(0.1f, 0.1f);
        _line.SetColors(Color.black, Color.red);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        _line.SetPosition(0, P1.Position);
        _line.SetPosition(1, P2.Position);
    }
}