using UnityEngine;

public class SpringDamper
{
    public Agent a;
    public Agent b;
    private float Spring;
    private float Damper;
    Vector3 e1;
    public float l;
    Vector3 e;
    float dir1;
    float dir2;
    Vector3 Force;
    float Spr1;
    float Damp1;
    float Rest1;
    public GameObject line;
    public void ComputeForce(float Spr, float Damp, float Rest)
    {
        Spr1 = Spr;
        Damp1 = Damp;
        Rest1 = Rest;
        e1 = b.transform.position - a.transform.position;
        l = e1.magnitude;
        e = e1 / l;
        dir1 = Vector3.Dot(e, a.Velocity);
        dir2 = Vector3.Dot(e, b.Velocity);
        Spring = -Spr1 * (Rest1 - l);
        Damper = -Damp1 * (dir1 - dir2);
        Force = (Spring + Damper) * e;
        a.Force += Force;
        b.Force += -Force;



    }
    public SpringDamper()
    {
        this.a = null;
        this.b = null;
    }
    public SpringDamper(Agent a, Agent b)
    {
        this.a = a;
        this.b = b;
    }
}