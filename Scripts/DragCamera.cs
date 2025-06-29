using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCamera : MonoBehaviour
{
    public float fMinY;
    public float fMaxY;
    private Vector3 lastMousePosition;
    private bool isDragging = false;

    public float dragSpeed;

    public void Start()
    {
        transform.position = new Vector3(transform.position.x, fMaxY, transform.position.z);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Vector3 delta = Input.mousePosition - lastMousePosition;

            float _moveY = delta.y * dragSpeed;

            transform.position += new Vector3(0, _moveY, 0);
            lastMousePosition = Input.mousePosition;

            if (transform.position.y < fMinY)
                transform.position = new Vector3(transform.position.x, fMinY, transform.position.z);
            else if (transform.position.y > fMaxY)
                transform.position = new Vector3(transform.position.x, fMaxY, transform.position.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
