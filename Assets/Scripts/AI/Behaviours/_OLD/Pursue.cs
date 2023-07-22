using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : MonoBehaviour
{
    //Pursue Algorithm
    public void PursuePath(AIAgent agent, Transform Target)
    {
        Vector3 desiredVelocity = Target.position - agent.transform.position;

        //Predicting the new position of the target
        //float prediction = desiredVelocity.magnitude / agent.MaxVelocity;
        desiredVelocity = desiredVelocity.normalized * agent.MaxVelocity; //* prediction;

        //Steering vector
        Vector3 steering = desiredVelocity - agent.velocity;
        steering = Vector3.ClampMagnitude(steering, agent.MaxForce);
        steering /= agent.Mass;

        //Applying steering force to velocity
        agent.velocity = Vector3.ClampMagnitude(agent.velocity + steering, agent.MaxVelocity);

        //Applying new movement to character
        //agent.transform.position += agent.velocity * Time.deltaTime;

        //Clamping the y-axis to 1 (keep the characters on the plane)
        //agent.transform.position = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
        //agent.transform.forward = agent.velocity.normalized;
    }
}
