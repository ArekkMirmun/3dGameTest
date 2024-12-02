using System;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField] Vector3 offset;
    private Transform _target;
    [Range(0, 1)] public float lerpValue;
    [SerializeField] private float sensibility;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        offset = Quaternion.AngleAxis(Input.GetAxis(("Mouse X")) * sensibility, Vector3.up) * offset;
        transform.position = Vector3.Lerp(transform.position, _target.position + offset, lerpValue);
        transform.LookAt(_target.position);
    }
}
