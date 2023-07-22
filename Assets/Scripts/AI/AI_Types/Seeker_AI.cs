using System.Collections.Generic;
using BehaviourTree;

public class Seeker_AI : Tree
{
    //Static variables for the nodes to reference
    public static int ChaserLayerMask = 1 << 8;
    public static float MaxSpeed = 20f;
    public static float MinSpeed = 10f;
    public static float fovRange = 8f;
    public static float AttackRange = 2f;
    public static float DangerRange = 5f;

    //Fleeing Variables
    public static int ObstacleMask = 1 << 7;
    public static float BoundsRadius = 0.5f;
    public static float AvoidCollisionWeight = 15;
    public static float TargetWeight = 1;
    public static float CollisionAvoidDst = 5f;
    public static float MaxSteerForce = 35f;

    //Token variables
    public static int tokenCounter = 0;

    //Freeze Bomb variable
    public int FreezeBombAmmo = 2;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
              new Sequence(new List<Node> //A - if chaser is too close, USE FREEZE BOMB
              {
                  new CheckForDanger(transform, FreezeBombAmmo),
                  new UseFreezeBomb(transform),
              }),
              new Sequence(new List<Node> //B - if chaser is close, FLEE
              {
                  new CheckForChasers(transform),
                  new FleeFromChaser(transform),
              }),
             new Sequence(new List<Node> //B - if chaser is close to your path, recalculate a new one
             {
                 new CheckForPathUpdate(transform),
                 new TacticalPathFinding(transform),
             }),
            new Sequence(new List<Node> //C - GO GET TOKEN
            {
                new CheckForToken(transform),
                new GoToToken(transform),
            })
        });

        return root;
    }
}
