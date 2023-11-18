using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Transactions;


public class BasicAgent : Agent
{

    [SerializeField]
    private GameObject goal;


    [SerializeField]
    private float moveSpeed = 1f;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-4.5f, 4.5f), 0, Random.Range(-4.5f, 4.5f));
        goal.transform.localPosition = new Vector3(Random.Range(-4.5f, 4.5f), 0, Random.Range(-4.5f, 4.5f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(goal.transform.localPosition);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];
        AddReward(-0.01f);

        
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        continuousAction[0] = Input.GetAxisRaw("Horizontal");
        continuousAction[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            AddReward(10f);
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-10f);
            EndEpisode();
        }
    }
}
