using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class AgentController : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float rotationSpeed = 90f;
    public float jumpHeight = 2f;

    public EscapeRoomAgent agent;

    [SerializeField]
    private GameObject cube;
    [SerializeField]
    private GameObject direction;

    private int move, rotate, jump;

    private bool isGrounded;

    private Rigidbody body;

    private float timeForJump = 0f;
    private float timeDelay = 0.3f;

    private float height;

    private bool isUpsideDown = false;
    private float timeUpsideDown = 0f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isStuck = false;
    private bool isTouchingObstacle = false;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        isGrounded = true;

        height = gameObject.GetComponent<BoxCollider>().size.y;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    public void SetInputs(int move, int rotate, int jump)
    {
        
        this.move = move;
        this.rotate = rotate;
        this.jump = jump;
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {

        

        //Debug.DrawRay(transform.position, -cube.transform.forward * (((height / 2)*transform.localScale.x)+ 0.03f), Color.white, 2f);
        if (Physics.Raycast(transform.position, -cube.transform.up, height/2, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Physics.Raycast(transform.position, -cube.transform.forward, (height / 2) * transform.localScale.x, LayerMask.GetMask("Ground")) ||
            Physics.Raycast(transform.position, cube.transform.forward, (height / 2) * transform.localScale.x, LayerMask.GetMask("Ground")) ||
            Physics.Raycast(transform.position, -cube.transform.right, (height / 2) * transform.localScale.x, LayerMask.GetMask("Ground")) ||
            Physics.Raycast(transform.position, cube.transform.right, (height / 2) * transform.localScale.x, LayerMask.GetMask("Ground")) ||
            Physics.Raycast(transform.position, cube.transform.up, (height / 2) * transform.localScale.x, LayerMask.GetMask("Ground")))
        {
            // The player is upside down
            isUpsideDown = true;
        }
        else
        {
            isUpsideDown= false;
        }

        if (Physics.Raycast(transform.position, -cube.transform.forward, ((height / 2)*transform.localScale.x) + 0.03f , LayerMask.GetMask("Obstacle")) ||
            Physics.Raycast(transform.position, cube.transform.forward, ((height / 2) * transform.localScale.x) + 0.03f, LayerMask.GetMask("Obstacle")) ||
            Physics.Raycast(transform.position, -cube.transform.right, ((height / 2) * transform.localScale.x) + 0.03f, LayerMask.GetMask("Obstacle")) ||
            Physics.Raycast(transform.position, cube.transform.right, ((height / 2) * transform.localScale.x) + 0.03f, LayerMask.GetMask("Obstacle"))) 
        {
            //Debug.Log("Hitting wall");
            isTouchingObstacle = true;
        }
        else
        {
            isTouchingObstacle = false;
        }


        Vector3 dir = direction.transform.position - cube.transform.position;
        dir.Normalize();
        //Debug.Log(dir);

        Debug.Log("Inputs: " + move + " - " + rotate + " - " + jump);
        if (!isUpsideDown)
        {
            if (move == 1)
            {
                transform.position += dir * Time.deltaTime * moveSpeed;
            }
            else if (move == 2)
            {
                //transform.Translate(-dir * Time.deltaTime * moveSpeed);
                transform.position -= dir * Time.deltaTime * moveSpeed;
            }

            if (rotate == 1)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            else if (rotate == 2)
            {
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }

            if (jump == 1 && isGrounded && timeForJump <= 0)
            {
                timeForJump = timeDelay;
                //Debug.Log("Jumping");
                body.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                StartCoroutine(JumpFunc());
                //agent.AddRew(-0.5f);
            }
        }
        else
        {
            //agent.AddRew(-0.1f);
            timeUpsideDown += Time.deltaTime;
            if (timeUpsideDown >= 1.0f)
            {
                //agent.AddRew(-10f);
                timeUpsideDown = 0f;

                //agent.EndEpisode();
            }
        }
    }

    IEnumerator JumpFunc()
    {
        while (timeForJump > 0)
        {
            timeForJump -= Time.deltaTime;

            yield return null;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered");
        PressurePlate plate = other.GetComponentInParent<PressurePlate>();
        if (plate != null)
        {
            //Debug.Log("Pressure plate pressed: " + other.name);
            if (plate.ActivatePressurePlate())
                agent.AddRew(0.3f);
            return;
        }

        FallTrigger fallTrigger = other.GetComponentInParent<FallTrigger>();
        if (fallTrigger != null)
        {
            //Debug.Log("Found fall trigger");
            agent.AddRew(-0.2f);
            agent.Fall();
            return;
        }

        EndTrigger endTrigger = other.GetComponent<EndTrigger>();
        if (endTrigger != null)
        {
            //Debug.Log("Found end trigger");
            agent.AddRew(1f);
            agent.EndEpisode();
            return;
        }
    }

    public void ResetAgent()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        isUpsideDown = false;

        isStuck = false;
        isTouchingObstacle = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //Debug.Log("Colpito ostacolo");
            agent.AddRew(-0.04f);
        }
    }

    public void getAgentCondition(out bool isStuck, out bool isTouchingObstacle)
    {
        isStuck = this.isStuck;
        isTouchingObstacle = this.isTouchingObstacle;
    }
}
