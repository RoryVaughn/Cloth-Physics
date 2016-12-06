using  UnityEngine;

public class Triangle
{
    //References to the agents in the triangle
    public Agent P1;
    public Agent P2;
    public Agent P3;
    public float p; //density coefficient
    public float c; //drag coefficient
    public float a; //wind power with magnitude
    public float Aa; //normalized wind vector
    public Vector3 n; //cross Product
    public Vector3 Vsurface; 
    public Vector3 Vair1; 
    public Vector3 v;
    public void Air(float Dense, float Drag, Vector3 Vair)
    {
        p = Dense;
        c = Drag;
        Vair1 = Vair;
        Vsurface = (P1.Velocity + P2.Velocity + P3.Velocity) / 3;
        v = Vsurface - Vair;
        n = Vector3.Cross(P2.transform.position - P1.transform.position, P3.transform.position - P1.transform.position)
          / Vector3.Cross(P2.transform.position - P1.transform.position, P3.transform.position - P1.transform.position).magnitude;
        a = 0.5f * Vector3.Cross(P2.transform.position - P1.transform.position, P3.transform.position - P1.transform.position).magnitude;
        Aa = a * (Vector3.Dot(v, n) / v.magnitude);
        //calculates the strength and direction of the wind
        Vector3 Faero = -0.5f * p * (v.magnitude * v.magnitude) * c * Aa * n;
        //separates the wind power to all of the particles in the traingle
        P1.Force += Faero / 3;
        P2.Force += Faero / 3;
        P3.Force += Faero / 3;
    }
    public Triangle(Agent P1, Agent P2, Agent P3)
    {
        this.P1 = P1;
        this.P2 = P2;
        this.P3 = P3;
    }
}