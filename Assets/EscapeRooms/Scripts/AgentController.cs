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

    private bool isStuck = false;
    private float timeStuck = 0f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

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
        agent.AddReward(-0.01f);
        Vector3 dir = direction.transform.position - cube.transform.position;
        dir.Normalize();
        //Debug.Log(dir);
        if (!isStuck) 
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

            if(jump == 1 && isGrounded && timeForJump <= 0)
            {
                timeForJump = timeDelay;
                //Debug.Log("Jumping");
                body.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                StartCoroutine(JumpFunc());
                agent.AddReward(-0.5f);
            }
        }
        else
        {
            agent.AddReward(-0.1f);
            timeStuck += Time.deltaTime;
            if (timeStuck >= 1.0f)
            {
                agent.AddReward(-10f);
                timeStuck = 0f;
                agent.EndEpisode();
            }
        }
    }

    private void FixedUpdate()
    {
        
        //Debug.DrawRay(transform.position, -cube.transform.up * height/2, Color.white, 2f);
        if (Physics.Raycast(transform.position, -cube.transform.up, height/2, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Physics.Raycast(transform.position, -cube.transform.forward, height / 2, LayerMask.GetMask("Ground")) ||
            Physics.Raycast(transform.position, cube.transform.forward, height / 2, LayerMask.GetMask("Ground")) ||
            Physics.Raycast(transform.position, -cube.transform.right, height / 2, LayerMask.GetMask("Ground")) ||
            Physics.Raycast(transform.position, cube.transform.right, height / 2, LayerMask.GetMask("Ground")) ||
            Physics.Raycast(transform.position, cube.transform.up, height / 2, LayerMask.GetMask("Ground")))
        {
            // The player is upside down
            isStuck = true;
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
            plate.ActivatePressurePlate();
            agent.AddReward(2f);
            return;
        }

        FallTrigger fallTrigger = other.GetComponentInParent<FallTrigger>();
        if (fallTrigger != null)
        {
            //Debug.Log("Found fall trigger");
            agent.AddReward(-2f);
            agent.Fall();
            return;
        }

        EndTrigger endTrigger = other.GetComponent<EndTrigger>();
        if (endTrigger != null)
        {
            //Debug.Log("Found end trigger");
            agent.AddReward(10f);
            agent.EndEpisode();
            return;
        }
    }

    public void ResetAgent()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        isStuck = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //Debug.Log("Colpito ostacolo");
            agent.AddReward(-0.4f);
        }
    }
}
