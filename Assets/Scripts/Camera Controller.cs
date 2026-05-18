using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float scrollSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 50f;

    void FixedUpdate()
    {
        // Movement with WASD / Arrow keys
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
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
