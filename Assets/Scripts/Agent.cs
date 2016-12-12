using System;
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

    public void Boundaries()
    {
        //top boundaries
        if (Position.y > 1.7f)
        {
            Position = new Vector3(Position.x, 1.7f, Position.z);
            Force += new Vector3(0, -0.7f * (Velocity.y * Mass), 0);
        }
        //bot boundaries
        if (Position.y < -10.8f)
        {
            Position = new Vector3(Position.x, -10.8f, Position.z);
            Force += new Vector3(0, -0.7f * (Velocity.y * Mass), 0);
        }
        //back wall boundaries
        if (Position.z > 5.0f)
        {
            Position = new Vector3(Position.x, Position.y, 5.0f);
            Force += new Vector3(0, 0, -0.7f * (Velocity.z * Mass));
        }
        //your facewall boundaries
        if (Position.z < -4.0f)
        {
            Position = new Vector3(Position.x, Position.y, -4.0f);
            Force += new Vector3(0, 0, -0.7f * (Velocity.z * Mass));
        }
        //left boundaries
        if (Position.x < -3.0f)
        {
            Position = new Vector3(-3.0f, Position.y, Position.z);
            Force += new Vector3(-0.7f * (Velocity.x * Mass), 0, 0);
        }
        //right boundaries
        if (!(Position.x > 13.0f)) return;
        Position = new Vector3(13.0f, Position.y, Position.z);
        Force += new Vector3(-0.7f * (Velocity.x * Mass), 0, 0);
    }

    public Vector3 CalcuateForce()
    {
        Boundaries();
        if (Anchor) return Position;
        Acceleration = 1/Mass*Force;
        Velocity += Acceleration*Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, 5.0f);
        Position += Velocity*Time.deltaTime;
        return Position;
    }

    // Use this for initialization
}