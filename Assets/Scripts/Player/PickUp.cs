using System;
using System.Collections;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public Transform mochila;
    private GameObject pickedUpObject;
    private bool onCooldown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickedUpObject = null;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Pickable") && pickedUpObject == null && !onCooldown)
        {
            pickedUpObject = hit.gameObject;
            pickedUpObject.transform.SetParent(mochila);
            pickedUpObject.transform.localPosition = Vector3.zero;
            pickedUpObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    
    public void ReleaseObject()
    {
        if (pickedUpObject != null)
        {
            onCooldown = true;
            pickedUpObject.transform.SetParent(null);
            pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;
            pickedUpObject = null;
        }

        StartCoroutine(CoolDown());
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(2f);
        onCooldown = false;
    }
}
