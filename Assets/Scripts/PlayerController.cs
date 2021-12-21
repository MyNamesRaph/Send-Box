using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.0F;
    public float jumpForce = 10.0F;
    public float lookSpeed = 1.0F;
    public float stepHeight = 0.51f;
    public float stepSmoothing = 0.1f;


    new private Rigidbody rigidbody;
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
    public float maxObjectPlacementDistance = 5.0F;
    public LayerMask placementMask;

    [Header("Items")]
    public PlaceableObject[] items;
    private PlaceableObject heldItem;
    private GameObject placedItem;
    private RaycastHit lookingAt;
    public Material placedItemMaterial;

    //item in the items array where -1 is none and 0+ are the items' index
    private sbyte selectedItem;

    private float distanceToGround;
    public float groundRaycastBias = 0.1F;

    new private CapsuleCollider collider;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        distanceToGround = collider.bounds.extents.y + 0.1F;
        StepRaycastLow.transform.localPosition = Vector3.zero;
        StepRaycastHigh.transform.localPosition = new Vector3(0.0f, stepHeight, 0.0F);

        heldItem = items[0];
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

        ShowPlacedItem();
    }

    /// <summary>
    /// Shows an hologram of the item you are holding on the ground
    /// </summary>
    private void ShowPlacedItem()
    {
        if (Physics.Raycast(head.transform.position, head.transform.forward, out lookingAt, maxObjectPlacementDistance, placementMask.value))
        {
            if (heldItem != null && placedItem == null)
            {
                //Create the hologram
                placedItem = Instantiate(heldItem.prefab);
                placedItem.layer = 6;
                placedItem.GetComponent<MeshRenderer>().material = placedItemMaterial;
                Destroy(placedItem.GetComponent<Collider>());
                Destroy(placedItem.GetComponent<Animator>());
                Destroy(placedItem.GetComponent<AudioSource>());
                Transform particles = placedItem.transform.Find("Particle System");
                if (particles != null)
                    Destroy(particles.gameObject);
            }
            //Update the position of the hologram
            placedItem.SetActive(true);
            placedItem.transform.position = new Vector3(Mathf.RoundToInt(lookingAt.point.x), Mathf.CeilToInt(lookingAt.point.y), Mathf.RoundToInt(lookingAt.point.z));
        }
        else
        {
            if (placedItem != null)
            {
                placedItem.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Place the item you are holding in the world
    /// </summary>
    private void OnPlace()
    {
        if (placedItem != null && placedItem.activeSelf)
        {
            GameObject go = Instantiate(heldItem.prefab);
            go.transform.position = placedItem.transform.position;
            go.layer = 7;
        }
    }

    /// <summary>
    /// Delete the item you are looking at in the world
    /// </summary>
    private void OnBreak()
    {
        if (lookingAt.collider != null)
        {
            if (lookingAt.collider.gameObject.layer == 7)
            {
                Destroy(lookingAt.collider.gameObject);
            }
        }

    }

    /// <summary>
    /// Manages item selection
    /// </summary>
    /// <param name="inputValue"></param>
    private void OnPickItem(InputValue inputValue)
    {
        sbyte value = (sbyte)inputValue.Get<float>();
        if (value != 0)
        {
            selectedItem = (sbyte)(value-1);
            heldItem = items[selectedItem];
            if (placedItem != null)
            {
                Destroy(placedItem);
            }
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
        if (IsGrounded())
            rigidbody.AddForce(0.0F, jumpForce, 0.0F,ForceMode.VelocityChange);
    }

    private bool IsGrounded()
    {
        Vector3 pos = transform.position;
        Vector3 posFront = new Vector3(transform.position.x + collider.bounds.extents.x, transform.position.y, transform.position.z);
        Vector3 posBack = new Vector3(transform.position.x - collider.bounds.extents.x, transform.position.y, transform.position.z);
        return Physics.Raycast(pos, Vector3.down, distanceToGround + groundRaycastBias) || Physics.Raycast(posFront, Vector3.down, distanceToGround + groundRaycastBias) || Physics.Raycast(posFront, Vector3.down, distanceToGround + groundRaycastBias);
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