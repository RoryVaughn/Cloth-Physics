using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ClothSystem : MonoBehaviour
{
    public List<Agent> Agents;
    public List<SpringDamper> SpringDampers;
    public List<Triangle> Triangles;
    public GameObject Joint;
    public float Mass = 0.1f;
    public int Dims = 10; //dimensions of nocdes in the cloth
    private int _amount;
    [Range(0, 2)] public float Grav = 1; //gravity slider
    [Range(0.5f, 2)] public float Rest = 1; //rest length slider
    [Range(1, 10)] public float Spr = 1; //springyness slider
    [Range(0.1f, 1)] public float Damp = 1; //damping slider
    [Range(0, 1)] public float Dense = 1; //Particle density slider
    [Range(0, 2)] public float Drag = 1; //particle drag slider 
    [Range(-2.5f, 2.5f)] public float Air1; //power of Air in the z direction.
    [Range(-2.5f, 2.5f)] public float Air2; //power of air in hte x direction.
    public bool Break;
    public bool Intruct ;
    public GameObject Text;
    public GameObject Line;

    //The following are UI ellements that can be used to edit infomation in the simulation from thge user.
    public void UiSetGravity(UnityEngine.UI.Slider slider)
    {
        //changes the gravity coefficent in the simulation.
        Grav = slider.value;
    }

    public void UiIntructions(UnityEngine.UI.Button button)
    {
        //opens the instructions to be visible to the user.
        Intruct = button;
        Text.SetActive(!Text.activeSelf);
    }

    public void UiReset(UnityEngine.UI.Button button)
    {
        //button that resets the scene
        SceneManager.LoadScene(0);
    }

    public void UiSetRest(UnityEngine.UI.Slider slider)
    {
        //changes the rest length of the spring dampers in the simulation.
        Rest = slider.value;
    }

    public void UiSetSpr(UnityEngine.UI.Slider slider)
    {
        //changes the spring coefficent in the simulation.
        Spr = slider.value;
    }

    public void UiSetDamp(UnityEngine.UI.Slider slider)
    {
        //changes the damping factor coefficent in the simulation.
        Damp = slider.value;
    }

    public void UiSetDense(UnityEngine.UI.Slider slider)
    {
        //changes the density coefficent in the simulation.
        Dense = slider.value;
    }

    public void UiSetAir1(UnityEngine.UI.Slider slider)
    {
        //changes the power of the air in the z direction in the simulation.
        Air1 = slider.value;
    }

    public void UiSetAir2(UnityEngine.UI.Slider slider)
    {
        //changes the power of the air in the x direction in the simulation.
        Air2 = slider.value;
    }

    private void CreateSpring(Agent a, Agent b, int index) //creates a spring that consists of two particles
        //and a line between the two particles to represent the bond.
    {
        var sd = new SpringDamper(a, b);
        SpringDampers.Add(sd);


        var go = Instantiate(Line);
        go.name = "Line Index" + (index);
        sd.Line = go;
        go.GetComponent<LineConnect>().P1 = a;
        go.GetComponent<LineConnect>().P2 = b;
    }

    // Use this for initialization
    private void Awake()
    {
        Agents = new List<Agent>();
        SpringDampers = new List<SpringDamper>();
        Triangles = new List<Triangle>();

        if (Joint == null)
            return;
        for (var x = 0; x < Dims; x++)
        {
            for (var y = 0; y < Dims; y++)
            {
                var a = new Agent(new Vector3(y, -x, 0), _amount);

                var go = Instantiate(Joint, a.Position, Quaternion.identity) as GameObject;

                if (go.GetComponent<MonoAgent>() == null)
                    go.AddComponent<MonoAgent>();


                go.GetComponent<MonoAgent>().Particle = a;
                Agents.Add(a);

 
                _amount++;
                go.name = "Joint index" + (Agents.Count - 1);
            }
        }
    }

    private void Start()
    {
        Text.SetActive(false);
        for (var i = 0; i < _amount; i++)
        {
            if (Agents[i].Number%Dims != 0 && Agents[i].Number != 0)
            {
                CreateSpring(Agents[i], Agents[i - 1], i); //connect left
            }
            if (Agents[i].Number >= Dims)
            {
                CreateSpring(Agents[i], Agents[i - Dims], i); //Connect down
            }
            if (Agents[i].Number%Dims != Dims - 1 && Agents[i].Number < Dims*Dims - 1 - Dims)
            {
                CreateSpring(Agents[i], Agents[i + Dims + 1], i); //top right
            }
            if (Agents[i].Number%Dims != Dims - 1 && Agents[i].Number >= Dims)
            {
                CreateSpring(Agents[i], Agents[i - Dims + 1], i); //bot right
            }

            if (Agents[i].Number%Dims == Dims - 1 || Agents[i].Number >= Dims*Dims - 1 - Dims) continue;
            var newTrianglex = new Triangle(Agents[i], Agents[i + Dims + 1], Agents[i + 1]);
            Triangles.Add(newTrianglex);

            var newTriangley = new Triangle(Agents[i], Agents[i + Dims + 1], Agents[i + Dims]);
            Triangles.Add(newTriangley);
        }
    }

    public Vector3 Gravity(Agent x)
    {
        var gravity = new Vector3(0, -10, 0)*x.Mass;
        return gravity;
    }

    public void Limit(Agent x)
    {
        x.Velocity = Vector3.ClampMagnitude(x.Velocity, 3f);
    }

    // Update is called once per frame
    private void Update()
    {
        Triangle tinstance;
        for (var j = 0; j < _amount; j++)
        {
            Agents[j].Force = Grav*Gravity(Agents[j]);
        }
        for (var i = 0; i < SpringDampers.Count; i++)
        {
            var instance = SpringDampers[i];

            if (instance.L >= 10)
            {
                Break = true;

                SpringDampers.Remove(instance);
                instance.Line.SetActive(false);
                for (var k = 0; k < Triangles.Count; k++)
                {
                    tinstance = Triangles[k];
                    if (tinstance.P1.Velocity.magnitude >= 17 || tinstance.P2.Velocity.magnitude >= 17 ||
                        tinstance.P3.Velocity.magnitude >= 17)
                    {
                        Triangles.Remove(tinstance);
                    }
                }
            }
            instance.ComputeForce(Spr, Damp, Rest);
        }

        if (Math.Abs(Air1) > 0 || Math.Abs(Air2) > 0)
        {
            foreach (var t in Triangles)
            {
                tinstance = t;
                tinstance.Air(Dense, Drag, Air1*Vector3.forward + Air2*Vector3.left);
            }
        }
        Break = false;
    }
}