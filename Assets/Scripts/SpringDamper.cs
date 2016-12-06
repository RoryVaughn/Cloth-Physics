using UnityEngine;

public class SpringDamper
{
    //reference to the two particles in the spring damper
    public Agent a;
    public Agent b;
    private float Spring;
    private float Damper;
    Vector3 e1; //displacement between the particles
    public float l; //magnitude of the displacement
    Vector3 e; //normalized displacement vecotr
    float dir1; //particle A's Velocity
    float dir2; //particle B's Velocity
    Vector3 Force;
    float Spr1; //Spring coefficient
    float Damp1; //damping coefficient
    float Rest1; //rest length coefficient
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
        //adds the force to each of the partles in the spring damper
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