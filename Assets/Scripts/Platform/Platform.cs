using UnityEngine;

public class Platform : MonoBehaviour
{

    public Rigidbody platformRb;
    public Transform[] platformWaypoints;
    public int speed;
    public int currentWaypointIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentWaypointIndex = 0;
        platformRb.transform.position = platformWaypoints[currentWaypointIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
        UpdateWaypointIndex();
    }

    private void MovePlatform()
    {
        platformRb.MovePosition(Vector3.MoveTowards(transform.position, platformWaypoints[currentWaypointIndex].position, speed * Time.deltaTime));
    }
    
    private void UpdateWaypointIndex()
    {
        if (Vector3.Distance(transform.position, platformWaypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex = GetNextWaypointIndex();
        }
    }
    private int GetNextWaypointIndex()
    {
        if (currentWaypointIndex == platformWaypoints.Length - 1)
        {
            return 0;
        }
        else
        {
            return currentWaypointIndex + 1;
        }
    }
    
    
}
