using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    //Wander Algorithm
    public void WanderPath(AIAgent agent, Vector3 target)
    {
        if ((agent.transform.position - target).magnitude < 0.5f)
            agent.isStanding = true;
        else
            agent.isStanding = false;

        if (!agent.isStanding)
        {
            agent.isWalking = true;
            Vector3 desiredVelocity = target - agent.transform.position;
            desiredVelocity = desiredVelocity.normalized * agent.MaxVelocity;

            //Steering vector
            Vector3 steering = desiredVelocity - agent.velocity;
            steering = Vector3.ClampMagnitude(steering, agent.MaxForce);
            steering /= agent.Mass;

            //Applying steering force to the velocity
            agent.velocity = Vector3.ClampMagnitude(agent.velocity + steering, agent.MaxVelocity);

            //Applying new movement to character
            //agent.transform.position += agent.velocity * Time.deltaTime;

            //Clamping the y-axis to 1 (keep the characters on the plane)
            //agent.transform.position = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);
            //agent.transform.forward = agent.velocity.normalized;
        }
        else
            agent.isWalking = false;
    }
}