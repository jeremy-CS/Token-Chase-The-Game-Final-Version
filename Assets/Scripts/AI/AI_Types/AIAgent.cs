using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    //Role of AI (Chaser/Evader)
    public bool isChaser;
    public bool isOriginalChaser;

    //Token counter (for original chaser only)
    public int tokenCounter = 0;

    //Animator variables
    private Animator animator;
    public bool isRunning = false;
    public bool isWalking = false;
    public bool isStanding = true;

    //Speed Variables
    public Vector3 velocity = Vector3.zero;
    public float Mass = 10f;
    public float MaxVelocity = 4f;
    public float MaxForce = 15f;
    private float rotationSpeed = 60f;

    //PathFinding variables (Original Chaser only?)
    public List<Waypoint> PathToGoal;
    public Waypoint goalNode;
    private int PathIndex = 0;
    private PathFinder pathFinder;
    private MapGenerator MapGen;
    public bool isGoingForToken = false;

    //Wandering variables
    private Vector3 wanderTarget;
    private bool isWandering = false;
    private float timer = 3f;
    private int XBorder = 3;
    private int ZBorder = 3;

    //Target variables
    public bool targetInRange = false;
    public Transform target;
    private bool isChasing = false;
    private bool isFleeing = false;

    //Algorithms references
    private Pursue pursue;
    private Flee flee;
    private Wander wander;

    //Freeze mechanic variables
    private bool isFrozen = false;
    private float frozenTimer = 0f;
    private float frozenLimit = 45f;
    public Transform frozenTarget;
    private bool isUnfreezing = false;
    public bool isSeeking = false;
    [SerializeField] Material chaserMat;
    [SerializeField] Material evaderMat;
    [SerializeField] Material frozenMat;

    //Wall Avoidance Variables
    private Ray frontRay;
    private Ray leftRay;
    private Ray rightRay;
    private float rayDist = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //Update evader/chaser counters
        if (isChaser)
            UIChanges.chaserCounter++;
        else
            UIChanges.evaderCounter++;

        //Get the required references
        animator = gameObject.GetComponent<Animator>();
        MapGen = GameObject.FindGameObjectWithTag("Map").GetComponent<MapGenerator>();
        pathFinder = gameObject.GetComponent<PathFinder>();
        wander = gameObject.GetComponent<Wander>();
        pursue = gameObject.GetComponent<Pursue>();

        if (!isOriginalChaser)
            flee = gameObject.GetComponent<Flee>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update Animator for AI agents
        UpdateAnimator();

        //--- DIFFERENT BEHAVIOURS DEPENDING ON TARGET AND FOV ---//
        //If a new token spawns, get original chaser to find it with A*
        if ((isOriginalChaser && goalNode != null && !isChasing) || isGoingForToken)
        {
            if (!isGoingForToken && goalNode != null)
            {
                isGoingForToken = true;
                isStanding = false;
                PathIndex = 0;
                //pathFinder.AStar(FindClosestNode(this.transform), goalNode);
            }

            PathFinding();
        }

        //If no one is within your fov, wander
        if ((!targetInRange && !isGoingForToken && !isFrozen && !isChasing && !isFleeing && !isUnfreezing) || isWandering)
        {
            if (!isWandering)
            {
                isWandering = true;
                isStanding = false;
                isRunning = false;
                StartWandering();
            }
            
            wander.WanderPath(this, wanderTarget);
        }

        //If an evader detects a chaser, flee
        if (targetInRange && !isChaser && !isFrozen || isFleeing)
        {

            if (isWandering)
            {
                isWandering = false;
                isStanding = false;
                StopWandering();
            }

            if (!isFleeing)
            {
                isFleeing = true;
                isRunning = true;
                isWalking = false;
                isStanding = false;
            }

            flee.FleePath(this, target);
        }

        //If a chaser detects an evader, pursue
        if ((targetInRange && isChaser && !isGoingForToken) || isChasing)
        {

            if (isWandering)
            {
                isWandering = false;
                StopWandering();
            }

            if (!isChasing)
            {
                isRunning = true;
                isWalking = false;
                isStanding = false;
                isChasing = true;
            }
            pursue.PursuePath(this, target);
        }

        //Update frozen timer for evaders that are tagged
        if (isFrozen)
        {
            //Stay frozen in place
            if (isWandering)
                isWandering = false;
            if (isFleeing)
                isFleeing = false;
            if (isUnfreezing)
                isUnfreezing = false;
            if (isSeeking)
                isSeeking = false;

            isWalking = false;
            isRunning = false;
            isStanding = true;

            frozenTimer += Time.deltaTime;

            if (frozenTimer > frozenLimit)
            {
                //Change the evader into a chaser after the timer is up
                this.isChaser = true;
                this.isFrozen = false;
                this.frozenTimer = 0f;
                this.MaxVelocity = 7f;

                //Change the color of the model
                this.GetComponentInChildren<SkinnedMeshRenderer>().material = chaserMat;
                this.gameObject.tag = "Chaser";

                //Update evader/chaser counters
                UIChanges.evaderCounter--;
                UIChanges.chaserCounter++;
            }
        }

        /*if ((frozenTarget != null && !isChaser && !isFrozen) || isUnfreezing)
        {
            //Go unfreeze the evader if still frozen
            if (!isUnfreezing)
            {
                isUnfreezing = true;
                pathFinder.AStar(FindClosestNode(this.transform), FindClosestNode(frozenTarget));
            }

            PathFinding();
        }*/

        if (isSeeking)
        {
            Seek();
        }

        
        if ((isChasing || isFleeing || isWandering || isSeeking) && !isFrozen && !isGoingForToken)
        {
            //Makes last minute checks for correct movements
            AvoidCollision();
            MoveAI();
        }
    }

    //Collision Avoidance Algorithm
    public void AvoidCollision()
    {
        frontRay = new Ray(transform.position + new Vector3(0f, 0.5f, 0f), transform.forward);
        leftRay = new Ray(transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.AngleAxis(-30f, transform.up) * transform.forward);
        rightRay = new Ray(transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.AngleAxis(30f, transform.up) * transform.forward);

        //Check if wall is ahead
        if (Physics.Raycast(frontRay, out RaycastHit hitF, rayDist))
        {
            //Only apply collision avoidance for same AI roles/wall/obstacles
            if (!((hitF.collider.gameObject.tag == "Evader" && !this.isChaser) || (hitF.collider.gameObject.tag == "Chaser" && this.isChaser)))
            {
                //Check if there is a wall to the left and right of you
                if (Physics.Raycast(leftRay, out RaycastHit hitL, rayDist) && Physics.Raycast(rightRay, out RaycastHit hitR, rayDist))
                {
                    //Wall on all sides, move backwards
                    velocity = Vector3.ClampMagnitude(velocity + hitF.normal + hitL.normal + hitR.normal, MaxVelocity);
                }
                else if (Physics.Raycast(leftRay, out RaycastHit hitLe, rayDist))
                {
                    //Wall on the left, move towards the right
                    velocity = Vector3.ClampMagnitude(velocity + hitF.normal + hitLe.normal, MaxVelocity);
                }
                else if (Physics.Raycast(rightRay, out RaycastHit hitRi, rayDist))
                {
                    //Wall on the right, move towards the left
                    velocity = Vector3.ClampMagnitude(velocity + hitF.normal + hitRi.normal, MaxVelocity);
                }
            }
        }
        else
        {
            //Check if there is no wall to the left and right of you
            if (!(Physics.Raycast(leftRay, out RaycastHit hitL, rayDist) && Physics.Raycast(rightRay, out RaycastHit hitR, rayDist)))
            {
                if (Physics.Raycast(leftRay, out RaycastHit hitLe, rayDist))
                {
                    if (!((hitLe.collider.gameObject.tag == "Evader" && !this.isChaser) || (hitLe.collider.gameObject.tag == "Chaser" && this.isChaser)))
                        //Wall on the left, move towards the right
                        velocity = Vector3.ClampMagnitude(velocity + hitLe.normal, MaxVelocity);
                }
                else if (Physics.Raycast(rightRay, out RaycastHit hitRi, rayDist))
                {
                    if (!((hitRi.collider.gameObject.tag == "Evader" && !this.isChaser) || (hitRi.collider.gameObject.tag == "Chaser" && this.isChaser)))
                        //Wall on the right, move towards the left
                        velocity = Vector3.ClampMagnitude(velocity + hitRi.normal, MaxVelocity);
                }
            }
        }
    }

    //Sum movements of AIs
    public void MoveAI()
    {
        if (!isGoingForToken || !isFrozen)
        {
            velocity.y = 0f;

            //Applying new movement to character
            this.transform.position += this.velocity * Time.deltaTime;

            //Clamping the y-axis to 1 (keep the characters on the plane)
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            this.transform.forward = this.velocity.normalized;
        }
    }

    //Find closest node
    public Waypoint FindClosestNode(Transform agent)
    {
        float distance = (MapGen._Map.graphNodes[0].Position - agent.position).magnitude;
        Waypoint startNode = MapGen._Map.graphNodes[0];

        foreach (Waypoint node in MapGen._Map.graphNodes)
        {
            if ((node.Position - agent.position).magnitude < distance)
            {
                startNode = node;
                distance = (node.Position - agent.position).magnitude;
            }
                
        }

        return startNode;
    }

    //PathFinding to get a token
    public void PathFinding()
    {
        if (PathToGoal == null && isGoingForToken)
        {
            PathToGoal = MapGen.path;
        }

        //Move to the goal if the list is not empty
        if (PathToGoal != null && PathToGoal.Count != 0)
        {
            //Move towards the next node in the path
            transform.position = Vector3.MoveTowards(transform.position, PathToGoal[PathIndex].Position, MaxVelocity * Time.deltaTime);
            isWalking = true;

            //Change orientation of the AI
            Vector3 direction = (PathToGoal[PathIndex].Position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            //Change target when we arrive at the node
            if (transform.position == PathToGoal[PathIndex].Position)
            {
                PathIndex++;

                //If we arrived at the goal, clear the path
                if (PathIndex > PathToGoal.Count - 1)
                {
                    PathIndex = 0;
                    PathToGoal = null;
                    isGoingForToken = false;

                    //If looking for a frozen evader, seek it when we arrive here
                    if (!isChaser && frozenTarget != null && isUnfreezing)
                    {
                        isSeeking = true;
                    }
                }
            }
        }
        else
        {
            //Bring back values to default (no pathfinding)
            isGoingForToken = false;
            goalNode = null;
            PathIndex = 0;
        }
    }

    public void Seek()
    {
        pursue.PursuePath(this, frozenTarget);
    }

    //Start wandering
    public void StartWandering()
    {
        InvokeRepeating("NextDirection", 0f, timer);
    }

    //Stop wandering
    public void StopWandering()
    {
        CancelInvoke("NextDirection");
    }

    //Get new wandering direction
    private void NextDirection()
    {
        wanderTarget = new Vector3(Random.Range(-XBorder, XBorder) + this.transform.position.x, 0, Random.Range(-ZBorder, ZBorder) + this.transform.position.z);
    }

    //Updating animator
    public void UpdateAnimator()
    {
        animator.SetBool("running", isRunning);
        animator.SetBool("walking", isWalking);
    }

    //Detect who touches what and make the appropriate changes
    private void OnCollisionEnter(Collision collision)
    {
        //Collect the token only if the AI is the original chaser
        if (collision.gameObject.tag == "Token" && this.isOriginalChaser)
        {
            tokenCounter++;
            Destroy(collision.gameObject);
            isGoingForToken = false;
            goalNode = null;
            PathToGoal = null;
        }

        //Unfreeze an evader if you touch them
        if (collision.gameObject.tag == "Evader" && !this.isChaser && this.isFrozen)
        {
            this.isFrozen = false;
            frozenTimer = 0f;
            isSeeking = false;
            this.GetComponentInChildren<SkinnedMeshRenderer>().material = evaderMat;

            //Remove freezing target
            GameObject[] evaders = GameObject.FindGameObjectsWithTag("Evader");
            foreach (GameObject evader in evaders)
            {
                evader.GetComponent<AIAgent>().frozenTarget = null;
            }

            //Flocking?
        }

        //Freeze an evader if a chaser collides with it
        if (collision.gameObject.tag == "Chaser" && !this.isChaser)
        {
            this.isFrozen = true;
            this.GetComponentInChildren<SkinnedMeshRenderer>().material = frozenMat;

            //Let the other evaders know you are frozen
            GameObject[] evaders = GameObject.FindGameObjectsWithTag("Evader");
            foreach (GameObject evader in evaders)
            {
                if (this.gameObject != evader)
                {
                    evader.GetComponent<AIAgent>().frozenTarget = this.transform;
                    evader.GetComponent<AIAgent>().isSeeking = true;
                }
            }

            isStanding = true;
            isWalking = false;
            isRunning = false;

            targetInRange = false;
            isFleeing = false;
            target = null;
        }

        //Stop pursuing if you hit the target
        if (collision.gameObject.tag == "Evader" && this.isChaser)
        {
            targetInRange = false;
            isChasing = false;
            target = null;
        }
    }

    //Change steering behaviour depending on who enters your fov
    private void OnTriggerEnter(Collider other)
    {
        //Change from wander to flee algorithm if a chaser enters an evaders fov
        if (other.gameObject.tag == "Chaser" && !this.isChaser && isWandering)
        {
            //Identify new target to runaway from
            targetInRange = true;
            target = other.gameObject.transform;
        }

        //Change from wander to pursue algorithm if an evader enter a chaser's fov
        if (other.gameObject.tag == "Evader" && this.isChaser && !isGoingForToken && !other.gameObject.GetComponent<AIAgent>().isFrozen)
        {
            //Identify new target and pursue them
            targetInRange = true;
            target = other.gameObject.transform;
        }

        if (other.gameObject.tag == "Player" && this.isChaser)
        {
            //Identify new target and pursue them
            targetInRange = true;
            target = other.gameObject.transform;

            if (isGoingForToken)
            {
                isGoingForToken = false;
                PathIndex = 0;
                PathToGoal = null;
            }
        }
    }

    //Go back to wandering when the target in no longer in range
    private void OnTriggerExit(Collider other)
    {
        //Stop fleeing
        if (other.gameObject.tag == "Chaser" && !this.isChaser && !isWandering)
        {
            targetInRange = false;
            isFleeing = false;
            target = null;
        }

        //Stop pursuing
        if ((other.gameObject.tag == "Evader" || other.gameObject.tag == "Player") && this.isChaser && !isGoingForToken)
        {
            targetInRange = false;
            isChasing = false;
            target = null;
        }
    }
}