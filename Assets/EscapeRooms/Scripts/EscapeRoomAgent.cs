using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using JetBrains.Annotations;

public class EscapeRoomAgent : Agent
{

    public AgentController controller;

    public RoomController roomController;

    [CanBeNull]
    private RewardTracker rewardTracker;

    private bool canRandomize = false;

    

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discActions = actionsOut.DiscreteActions;

        /*
         * First input, move -> 0: stand, 1: forward, 2: backward
         * Second input, rotate -> 0: still, 1: right, 2:left
         * Third input, jump -> 0: don't jump, 1: jump
        */

        int move = 0;
        int rotate = 0;
        int jump = 0;

        if (Input.GetKey(KeyCode.D))
        {
            rotate = 1;
        }
        else if (Input.GetKey(KeyCode.A)) 
        {
            rotate = 2;
        }

        if (Input.GetKey(KeyCode.W))
        {
            move = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            move = 2;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            jump = 1;
        }

        discActions[0] = move;
        discActions[1] = rotate;
        discActions[2] = jump;

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        ActionSegment<int> discActions = actions.DiscreteActions;

        int move = discActions[0];
        int rotate = discActions[1];
        int jump = discActions[2];

        controller.SetInputs(move, rotate, jump);
        AddRew(-0.001f);


        if (jump == 1)
        {
            AddRew(-0.02f);
        }

        bool isStuck, isTouchingObstacle;
        controller.getAgentCondition(out isStuck, out isTouchingObstacle);

        if (isStuck)
        {
            AddRew(-0.2f);
        }

        if (isTouchingObstacle)
        {
            AddRew(-0.035f);
        }
    }

    public override void OnEpisodeBegin()
    {
        
        roomController.ResetRoom();
        controller.ResetAgent();
        canRandomize = roomController.canRandomize;

        rewardTracker.LogReward();
        rewardTracker.Reset();

        if (canRandomize)
        {
            gameObject.transform.position += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        }
    }

    public void Fall()
    {
        //AddReward(-2f);
        EndEpisode();
    }

    public void AddRew(float reward)
    {
        AddReward(reward);
        if (rewardTracker)
        {
            rewardTracker.AddReward(reward);
        }
    }

    private void Start()
    {
        TryGetComponent<RewardTracker>(out rewardTracker);
    }
}
