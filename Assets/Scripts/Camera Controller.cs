using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float scrollSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 50f;
    public float rotationSpeed = 40f;
    public float maxRotation = 90f;
    public float minRotation = 10f;

    void FixedUpdate()
    {
        // Movement with WASD / Arrow keys
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float pan = Input.GetAxisRaw("Pan");

        float newRotationX = transform.eulerAngles.x + pan * rotationSpeed * Time.deltaTime;
        newRotationX = Mathf.Clamp(newRotationX, minRotation, maxRotation);

        transform.eulerAngles = new Vector3(newRotationX, transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 move = new Vector3(horizontal, 0, vertical).normalized;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
       

        // Scroll wheel zoom (moves camera along its forward direction)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            Vector3 forward = transform.forward;
            float currentDist = Vector3.Dot(transform.position, forward);
            float targetDist = Mathf.Clamp(currentDist + scroll * scrollSpeed, -15f, -5f);
            transform.position = forward * targetDist + (transform.position - forward * currentDist);
        }
    }
    
}
