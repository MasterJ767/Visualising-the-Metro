using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float speed = 24f;
    public Transform parent;
    public Transform train;

    public Point[] points;
    
    private Mode cameraMode;
    private float XRotation = 0;
    private Vector3 movement;
    private Rigidbody rb;

    private int currentIndex;

    private void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        cameraMode = Mode.FREE;
    }

    private void FixedUpdate()
    {
        FreeMoveCamera();
    }

    private void Update()
    {
        if (Input.GetButtonDown("ModeUp"))
        {
            ModeUp();
        }
        else if (Input.GetButtonDown("ModeDown"))
        {
            ModeDown();
        }
        
        if (cameraMode == Mode.FREE)
        {
            GetFreeMoveInputs();
            ChangeAltitude();
        }
        else if (cameraMode == Mode.STATIC)
        {
            ChangePoint();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (Point point in points)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(point.position, 2f);
        }
    }

    private int mod(int x, int m) {
        return (x%m + m)%m;
    }

    private void ModeUp()
    {
        cameraMode = (Mode)mod((int)cameraMode + 1, (int)Enum.GetValues(typeof(Mode)).Cast<Mode>().Last() + 1);
        transform.parent.parent = parent;
        transform.rotation = Quaternion.identity;
        transform.parent.rotation = quaternion.identity;

        if (cameraMode == Mode.STATIC)
        {
            transform.parent.position = points[currentIndex].position;
            transform.localRotation = Quaternion.Euler(points[currentIndex].rotation);
        }
        else if (cameraMode == Mode.FOLLOW)
        {
            transform.parent.parent = train;
            transform.parent.localPosition = new Vector3(-2, 5, -4);
            transform.localRotation = Quaternion.Euler(15, 15, 0);
        }
    }

    private void ModeDown()
    {
        cameraMode = (Mode)mod((int)cameraMode - 1, (int)Enum.GetValues(typeof(Mode)).Cast<Mode>().Last() + 1);
        transform.parent.parent = parent;
        transform.rotation = Quaternion.identity;
        transform.parent.rotation = quaternion.identity;
        
        if (cameraMode == Mode.STATIC)
        {
            transform.parent.position = points[currentIndex].position;
            transform.localRotation = Quaternion.Euler(points[currentIndex].rotation);
        }
        else if (cameraMode == Mode.FOLLOW)
        {
            transform.parent.parent = train;
            transform.parent.localPosition = new Vector3(-2, 5, -4);
            transform.localRotation = Quaternion.Euler(15, 15, 0);
        }
    }

    private void FreeMoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
        
        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -50f, 90f);

        Vector3 YRotation = Vector3.up * mouseX;
            
        transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
        transform.parent.Rotate(YRotation);

        if (cameraMode == Mode.FREE)
        {
            rb.MovePosition(rb.position + (movement * speed * 2f * Time.fixedDeltaTime));
        }
    }

    private void GetFreeMoveInputs()
    {
        movement = (Input.GetAxisRaw("Vertical") * transform.forward) + (Input.GetAxisRaw("Horizontal") * transform.right);
        movement = movement.normalized;
    }

    private void ChangeAltitude()
    {
        if (Input.GetButton("Jump"))
        {
            rb.MovePosition(rb.position + (Vector3.up * speed * 2f * Time.fixedDeltaTime));
        }
        else if (Input.GetButton("Descend"))
        {
            rb.MovePosition(rb.position + (-Vector3.up * speed * 2f * Time.fixedDeltaTime));
        }
    }

    private void ChangePoint()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            currentIndex = mod(currentIndex + 1, points.Length);
            transform.parent.position = points[currentIndex].position;
            transform.localRotation = Quaternion.Euler(points[currentIndex].rotation);
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            currentIndex = mod(currentIndex - 1, points.Length);
            transform.parent.position = points[currentIndex].position;
            transform.localRotation = Quaternion.Euler(points[currentIndex].rotation);
        }
    }
}

public enum Mode
{
    FREE,
    STATIC,
    FOLLOW
}

[Serializable]
public struct Point
{
    public Vector3 position;
    public Vector3 rotation;
}