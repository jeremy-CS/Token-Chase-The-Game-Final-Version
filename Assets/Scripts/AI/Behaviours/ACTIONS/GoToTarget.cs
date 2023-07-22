//Code for collision avoidance made with help from URL: https://github.com/SebLague/Boids
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class GoToTarget : Node
{
    private Transform _transform;
    private Vector3 _position;
    private Vector3 _forward;
    private Vector3 _velocity;
    private Vector3 _acceleration; //To update each call

    public GoToTarget(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        _position = _transform.position;
        _forward = _transform.forward;

        if (Vector3.Distance(_transform.position, target.position) > 0.1f)
        {
            _acceleration = Vector3.zero;

            Vector3 offsetToTarget = (target.position - _position);
            _acceleration = SteerTowards(offsetToTarget) * Chaser_AI.TargetWeight;

            if (IsHeadingForCollision())
            {
                Vector3 collisionAvoidDir = ObstacleRays();
                Vector3 collisionAvoidForce = SteerTowards(collisionAvoidDir) * Chaser_AI.AvoidCollisionWeight;
                _acceleration += collisionAvoidForce;
            }
            

            //Update path with collision in mind
            _velocity += _acceleration * Time.deltaTime;
            float speed = _velocity.magnitude;
            Vector3 dir = _velocity / speed;
            speed = Mathf.Clamp(speed, Chaser_AI.PatrolSpeed, Chaser_AI.ChaseSpeed);
            _velocity = dir * speed;

            _transform.position += _velocity * Time.deltaTime;
            _transform.forward = dir;
            _position = _transform.position;
            _forward = _transform.forward;
        }
        else
        {
            //Head straight for the target
            _transform.position = Vector3.MoveTowards(_transform.position, target.position, Chaser_AI.ChaseSpeed * Time.deltaTime);
            _transform.LookAt(target.position);
        }

        state = NodeState.RUNNING;
        return state;
    }

    bool IsHeadingForCollision()
    {
        RaycastHit hit;
        if (Physics.SphereCast(_transform.position, Chaser_AI.BoundsRadius, _forward, out hit, Chaser_AI.CollisionAvoidDst, Chaser_AI.ObstacleMask))
        {
            return true;
        }
        else { }
        return false;
    }

    Vector3 ObstacleRays()
    {
        Vector3[] rayDirections = CollisionHelper.directions;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Vector3 dir = _transform.TransformDirection(rayDirections[i]);
            Ray ray = new Ray(_position, dir);
            if (!Physics.SphereCast(ray, Chaser_AI.BoundsRadius, Chaser_AI.CollisionAvoidDst, Chaser_AI.ObstacleMask))
            {
                return dir;
            }
        }
        return _forward;
    }

    Vector3 SteerTowards(Vector3 vector)
    {
        Vector3 v = vector.normalized * Chaser_AI.ChaseSpeed - _velocity;
        return Vector3.ClampMagnitude(v, Chaser_AI.MaxSteerForce);
    }
}
