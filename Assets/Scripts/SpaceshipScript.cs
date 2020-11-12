using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipScript : MonoBehaviour
{
   
    [Header("Transforms")]
    public Transform spaceship;
    public Transform cameraPivot;
    public Camera mainCamera;
    public RectTransform crosshair;

    [Header("Particle Systems")]
    public ParticleSystem jetEngine;
    public ParticleSystem boostJetEngine;

    [Header("Flight Settings")]
    public float currentSpeed;
    public float regularSpeed;
    public float boostSpeed;
    public float rotationSpeed;
    public float cameraSmoothing;


    private Rigidbody spaceshipRigidbody;
    private Quaternion lookRotation;
    private float rotationX = 0;
    private float rotationZ = 0;
    private float mouseXSmooth = 0;
    private float mouseYSmooth = 0;
    private Vector3 baseSpaceshipRotation;

    void Start()
    {
        spaceshipRigidbody = GetComponentInChildren<Rigidbody>();
        spaceshipRigidbody.useGravity = false;

        lookRotation = transform.rotation;
        baseSpaceshipRotation = spaceship.localEulerAngles;
        rotationZ = baseSpaceshipRotation.z;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        BoostJetEngine();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            regularSpeed = 20;
        }
    }

    // Fixed update since the flight controls are physics based
    void FixedUpdate()
    {
        // When space is pressed, the ship accelerates up to the boosted speed
        // Once space is let go off, the ship slows down back to the regular speed
        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed = Mathf.Lerp(currentSpeed, boostSpeed, Time.deltaTime * 2);            
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, regularSpeed, Time.deltaTime * 10);
        }

        // Set the move direction to the horizontal axis
        Vector3 moveDirection = new Vector3(0.0f, 0.0f, currentSpeed);
        moveDirection = transform.TransformDirection(moveDirection);

        // Set the velocity of the rigidbody to the current speed
        spaceshipRigidbody.velocity = moveDirection;

        // Lerp the camera position between the pivot and the main camera to achieve the floaty space effect
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPivot.position, Time.deltaTime * cameraSmoothing);

        // Lerp the camera rotation between the pivot and the main camera to achieve more of the floaty space effect
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, cameraPivot.rotation, Time.deltaTime * cameraSmoothing);

        // Updates the ship's rotation based on key and mouse inputs
        float tempRotationX = 0;
        float tempRotationZ = 0;

        if (Input.GetKey(KeyCode.W))
        {
            tempRotationX = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            tempRotationX = -1;
        }else
        {
            tempRotationX = 0;
        }

        if (Input.GetKey(KeyCode.A))
        {
            tempRotationZ = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            tempRotationZ = -1;
        }
        else
        {
            tempRotationZ = 0;
        }

        mouseXSmooth = Mathf.Lerp(mouseXSmooth, Input.GetAxis("Mouse X") * rotationSpeed, Time.deltaTime * cameraSmoothing);
        mouseYSmooth = Mathf.Lerp(mouseYSmooth, Input.GetAxis("Mouse Y") * rotationSpeed, Time.deltaTime * cameraSmoothing);

        Quaternion localRotation = Quaternion.Euler(tempRotationX * rotationSpeed - mouseYSmooth, mouseXSmooth,  tempRotationZ * rotationSpeed);
        lookRotation = lookRotation * localRotation;
        transform.rotation = lookRotation;

        rotationX -= mouseYSmooth;
        rotationX = Mathf.Clamp(rotationX, -boostSpeed, boostSpeed);

        rotationZ -= mouseXSmooth;
        rotationZ = Mathf.Clamp(rotationZ, -boostSpeed, boostSpeed);

        spaceship.transform.localEulerAngles = new Vector3(rotationX, baseSpaceshipRotation.y, rotationZ);

        rotationX = Mathf.Lerp(rotationX, baseSpaceshipRotation.x, Time.deltaTime * cameraSmoothing);
        rotationZ = Mathf.Lerp(rotationZ, baseSpaceshipRotation.z, Time.deltaTime * cameraSmoothing);

        // Update the crosshair position
        if (crosshair)
        {
            crosshair.position = mainCamera.WorldToScreenPoint(transform.position + transform.forward * 100);
        }
    }

    // Plays and stops the relevant particle systems for the jet engine
    void BoostJetEngine()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            boostJetEngine.Play();
            jetEngine.Stop();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jetEngine.Play();
            boostJetEngine.Stop();
        }
    }

}
