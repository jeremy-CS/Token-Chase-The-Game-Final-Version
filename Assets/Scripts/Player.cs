using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Movement variables
    [SerializeField] float rotationSpeed = 65f;
    [SerializeField] float speedBoost = 10f;

    //Token variable
    public int tokenCounter = 0;

    //Freeze Bomb variables
    public int freezeAmmo = 5;
    public int _enemyLayerMask = 1 << 8;
    public float _attackRange = 5f;

    //Game reference
    private UIChanges game;

    //Movement variables
    private CharacterController characterController;
    [SerializeField] private Transform cameraTransform;
    
    //Animator variables
    private Animator animator;
    private bool isRunning = false;
    private bool isWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        game = GameObject.FindGameObjectWithTag("UI").GetComponent<UIChanges>();
    }

    // Update is called once per frame
    void Update()
    {
        //Update Animator
        UpdateAnimator();

        //convert user input into world movement
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        //assign movement to a vector3
        Vector3 movementDirection = new Vector3(horizontalMovement, 0, verticalMovement);
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        //If shift is pressed, run, otherwise walk
        if (movementDirection != Vector3.zero)
        {
            //apply rotation to player
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                isWalking = false;
            }
            else
            {
                isRunning = false;
                isWalking = true;
            }
        }
        else
        {
            isRunning = false;
            isWalking = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //FREEZE ENEMY
            Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRange, _enemyLayerMask);

            if (colliders.Length > 0 && freezeAmmo > 0)
            {
                colliders[0].GetComponent<Chaser_AI>().IsFrozen = true;
                freezeAmmo--;
            }
            
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition * speedBoost;
        characterController.Move(velocity);
    }

    //Updating animator
    public void UpdateAnimator()
    {
        animator.SetBool("Walking", isWalking);
        animator.SetBool("Running", isRunning);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Token")
        {
            tokenCounter++;
            Destroy(hit.gameObject);
        }
         
        if (hit.gameObject.tag == "Chaser")
        {
            game.EndGame(true);
        }
    }
}
