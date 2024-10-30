using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollower : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float DistanceWhenFollow = 2.0f; // Distance at which the NPC will follow the player
    public float followSpeed = 3.0f; // Speed at which the NPC will follow the player

    private bool isFollowing = false;

    void Update()
    {
        // Check for input to start/stop following
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerInRange())
        {
            isFollowing = !isFollowing; // Toggle follow state
        }

        // If the NPC is following the player, move towards them
        if (isFollowing)
        {
            FollowPlayer();
        }
    }

    private bool IsPlayerInRange()
    {
        // Check the distance between the NPC and the player
        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= DistanceWhenFollow;
    }

    private void FollowPlayer()
    {
        // Move the NPC towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * followSpeed * Time.deltaTime;

        // Optionally, make the NPC look at the player
        //Quaternion lookRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
