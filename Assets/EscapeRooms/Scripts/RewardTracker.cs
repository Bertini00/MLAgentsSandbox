using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardTracker : MonoBehaviour
{

    public bool isPrinting = false;

    private float reward;

    private void Start()
    {
        reward = 0;
    }

    public void Reset()
    {
        reward = 0;
    }

    public void AddReward(float amount)
    {
        reward += amount;
        if (amount > 0)
        {
            LogAddedReward(amount);
        }
    }

    public void LogReward()
    {
        if (isPrinting)
        {
            Debug.Log("Reward of current Episode: " + reward);
        }
    }

    public void LogAddedReward(float amount)
    {
        if (isPrinting)
        {
            Debug.Log("Added Reward: " + amount);
        }
    }
}
