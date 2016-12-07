using UnityEngine;

public class Agent
{
    public Vector3 Acceleration = Vector3.zero;
    public bool Anchor; //disables reational movement from other nodes
    public Vector3 Force = Vector3.zero;
    public float Mass = 0.1f;
    public int Number;
    public Vector3 Position;
    public Vector3 Velocity = Vector3.zero;

    public Agent()
    {
        Position = Vector3.zero;
    }

    public Agent(Vector3 position)
    {
        Position = position;
    }

    public Agent(Vector3 position, int id)
    {
        Position = position;
        Number = id;
    }

    public Vector3 CalcuateForce()
    {
        Acceleration = 1/Mass*Force;
        Velocity += Acceleration*Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, 5.0f);
        Position += Velocity*Time.deltaTime;
        return Position;
    }

    // Use this for initialization
}