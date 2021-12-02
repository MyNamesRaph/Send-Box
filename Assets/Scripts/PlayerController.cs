/*using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.0F;
    public float jumpForce = 10.0F;
    public float lookSpeed = 1.0F;
    public float stepHeight = 0.51f;
    public float stepSmoothing = 0.1f;


    private Rigidbody rigidbody;
    private float movementX;
    private float movementY;
    private float lookX;
    private float lookY;
    public float maxRotation = 50.0F;
    //Negative rotation value
    public float minRotation = -50.0F;
    
    [Header("Body Parts")]
    public Camera cam;
    public GameObject head;

    [Header("Raycast Settings")]
    public float raycastLenghtLow = 0.51F;
    public float raycastLenghtHigh = 0.52F;
    public GameObject StepRaycastLow;
    public GameObject StepRaycastHigh;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
        StepRaycastHigh.transform.localPosition = new Vector3(0.0f, stepHeight, 0.0F);
    }

    void FixedUpdate()
    {
        StepClimb();
        Vector3 movement = transform.forward * movementY + transform.right * movementX;
        rigidbody.AddForce(movement * speed, ForceMode.VelocityChange);
    }

    private void Update()
    {
        rigidbody.transform.Rotate(0.0F, lookX, 0.0F);
        
        head.transform.Rotate(lookY, 0.0F, 0.0F);
        Vector3 headRot = head.transform.rotation.eulerAngles;
        if (360.0F + minRotation > headRot.x && headRot.x > 180)
        {
            head.transform.rotation = Quaternion.Euler(360.0F+minRotation, headRot.y, 0.0F);
        }
        else if (180 >= headRot.x && headRot.x > maxRotation)
        {
            head.transform.rotation = Quaternion.Euler(maxRotation, headRot.y, 0.0F);
        }

    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnLook(InputValue rotationValue)
    {
        Vector2 rotationVector = rotationValue.Get<Vector2>();
        lookX = rotationVector.x * lookSpeed;
        lookY = -rotationVector.y * lookSpeed;
    }

    private void OnJump()
    {
        rigidbody.AddForce(0.0F, jumpForce, 0.0F,ForceMode.VelocityChange);
    }


    /// <summary>
    /// Source: https://www.youtube.com/watch?v=DrFk5Q_IwG0
    /// </summary>
    private void StepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(StepRaycastLow.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, raycastLenghtLow))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(StepRaycastHigh.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, raycastLenghtHigh))
            {
                rigidbody.position -= new Vector3(0.0F, -stepSmoothing, 0.0F);
            }
        }
    }
}
*/