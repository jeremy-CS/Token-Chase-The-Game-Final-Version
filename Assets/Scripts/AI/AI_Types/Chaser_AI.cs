using System.Collections.Generic;
using BehaviourTree;

public class Chaser_AI : Tree
{
    public UnityEngine.Transform[] waypoints;
    public UnityEngine.Material freezeMat;
    public UnityEngine.Material normalMat;

    //Static variables for the nodes to reference
    public static float PatrolSpeed = 4f;
    public static float ChaseSpeed = 10f;
    public static float fovRange = 12f;
    public static float CaptureRange = 0.5f;

    //Wall avoidance variables
    public static int ObstacleMask = 1 << 7;
    public static float BoundsRadius = 0.5f;
    public static float AvoidCollisionWeight = 15;
    public static float TargetWeight = 1;
    public static float CollisionAvoidDst = 5f;
    public static float MaxSteerForce = 25f;

    //TokenAlert
    public bool tokenTaken = false;

    //Freeze Bomb variable
    public bool IsFrozen = false;
    

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node> 
        {
            new Sequence(new List<Node> // WAIT if hit by freeze bomb
            {
                new CheckIfFrozen(transform),
                new WaitToUnfreeze(transform, freezeMat, normalMat),
            }),
            new Sequence(new List<Node>  //A_01 - CAPTURE if target is in range
             {
                 new CheckForCaptureRange(transform),
                 new Capture(transform),
             }),
            new Selector(new List<Node>
            {
                new Sequence(new List<Node> //A_02 - If target is in FOV, PURSUE
                {
                    new CheckForEnemy(transform),
                    new GoToTarget(transform),
                }),
                new Sequence(new List<Node> //B_01 - If target is out of sight, go to last known location
                {
                    new CheckIfOutOfSight(transform),
                    new GoToLastLocation(transform),
                }),
            }),
            new Patrol(transform, waypoints), //C - If no targets are in range, PATROL
        });

        return root;
    }

    public void Freeze()
    {
        IsFrozen = true;
    }
}
