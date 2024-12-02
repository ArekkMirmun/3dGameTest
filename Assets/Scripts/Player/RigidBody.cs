using System;
using UnityEngine;

public class RigidBody : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float targetMass;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        // We don't want to move objects without a rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We don't want to push objects below us
        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        targetMass = body.mass;
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.linearVelocity = pushDir * moveSpeed / targetMass;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetMass = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
