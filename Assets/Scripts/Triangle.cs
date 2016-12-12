using UnityEngine;

public class Triangle
{
    //References to the agents in the triangle
    public Agent P1;
    public Agent P2;
    public Agent P3;
    public float P; //density coefficient
    public float C; //drag coefficient
    public float A; //wind power with magnitude
    public float Aa; //normalized wind vector
    public Vector3 N; //cross Product
    public Vector3 Vsurface;
    public Vector3 Vair1;
    public Vector3 V;

    public void Air(float dense, float drag, Vector3 vair)
    {
        P = dense;
        C = drag;
        Vair1 = vair;
        Vsurface = (P1.Velocity + P2.Velocity + P3.Velocity)/3;
        V = Vsurface - vair;
        N = Vector3.Cross(P2.Position - P1.Position, P3.Position - P1.Position)
            /Vector3.Cross(P2.Position - P1.Position, P3.Position - P1.Position).magnitude;
        A = 0.5f*Vector3.Cross(P2.Position - P1.Position, P3.Position - P1.Position).magnitude;
        Aa = A*(Vector3.Dot(V, N)/V.magnitude);
        //calculates the strength and direction of the wind
        var faero = -0.5f*P*(V.magnitude*V.magnitude)*C*Aa*N;
        //separates the wind power to all of the particles in the traingle
        P1.Force += faero/3;
        P2.Force += faero/3;
        P3.Force += faero/3;
    }

    public Triangle(Agent p1, Agent p2, Agent p3)
    {
        P1 = p1;
        P2 = p2;
        P3 = p3;
    }
}