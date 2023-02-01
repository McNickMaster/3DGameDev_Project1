using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movespeed = 1.3f, gravForce;
    public float grabRange = 15f, grabSize = 1.5f;
    public float dropForce = 50, throwForce = 500;
    public float crouchHeight = 0.4f, crouchSpeedMod = 0.75f;

    private float moveMod = 1;
    private float yMod = 1;

    [Header("Camera Settings")]
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    [Header("Modules")]
    public Transform body, playerCam;
    public CharacterController charController;
    public Rigidbody rb;
    public GameObject playerHand;

    [SerializeField]
    private bool canPickUp = false;
    private bool crouched = false, justCrouched = false;
    [SerializeField]
    private GameObject objectToPickUp, objectInHand;
    



    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Look();
        CheckCrouch();
        //CheckPickup();



        
    }

    void CheckPickup()
    {
        Vector3 origin, dir;
        origin = playerCam.transform.position;
        dir = playerCam.transform.forward * grabRange;

        //Debug.DrawRay(origin, dir, Color.magenta);
        RaycastHit hit;
        Physics.Raycast(origin, dir, out hit, grabRange);

        if(hit.transform != null && hit.transform.gameObject.layer == 10)
        {
            canPickUp = true;
            objectToPickUp = hit.transform.gameObject;
        } else {
            canPickUp = false;
            objectToPickUp = null;
        }

    }
    private void PickUp()
    {
        canPickUp = false;
        objectInHand = objectToPickUp;
        objectInHand.layer = 0;
        objectInHand.GetComponentInChildren<BoxCollider>().enabled = false;
        objectInHand.GetComponent<Rigidbody>().isKinematic = true;
        objectInHand.transform.parent = playerHand.transform;
        //lerp this
        objectInHand.transform.localPosition = Vector3.zero;
        objectToPickUp = null;

    
    }


    private void DropObject()
    {
        
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.transform.parent = null;
        objectInHand.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * dropForce * 10000 * Time.deltaTime);
        objectInHand.layer = 10;
        objectInHand.GetComponentInChildren<BoxCollider>().enabled = true;
        objectInHand = null;
    }

    private void ThrowObject()
    {
        objectInHand.GetComponent<Rigidbody>().isKinematic = false;
        objectInHand.GetComponentInChildren<BoxCollider>().enabled = true;
        objectInHand.transform.parent = null;

        Vector3 throwDir = playerCam.transform.forward + (0.5f) * playerCam.transform.up * (0.4f);
        objectInHand.GetComponent<Rigidbody>().AddForce(throwDir * throwForce * 10000 * Time.deltaTime);
        objectInHand.layer = 10;
        objectInHand = null;
    }

    private void Jump()
    {
        
    }

    private void Move()
    {
        Vector3 velocity;

        velocity = GetInput_Translation() * movespeed * Time.fixedDeltaTime;

        velocity = (velocity.x * body.transform.right) + (velocity.z * body.transform.forward);

        //charController.Move(velocity.x * body.transform.right + velocity.z * body.transform.forward + (gravForce * Vector3.down));
        rb.velocity = (moveMod * (velocity.x * body.transform.right + velocity.z * body.transform.forward) + (gravForce * yMod  * Time.fixedDeltaTime * Vector3.down));
        if(crouched)
        {
            rb.velocity *= crouchSpeedMod;
        }
    }

    private void CheckCrouch()
    {
        justCrouched = false;
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(crouched)
            {
                crouched = false;
            } else if(!crouched)
            {
                crouched = true;
                justCrouched = true;
            }
        }

        
        transform.localScale = new Vector3(1, crouched ? crouchHeight:1
        , 1);

        if(justCrouched)
        {
            transform.Translate(Vector3.down * crouchHeight * 2);
        }
    }

    float desiredX, desiredZ;
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

    
        //target_camera_tilt = camera_tilt_amount * -Input.GetAxisRaw("Horizontal");
        //desiredZ = Mathf.Lerp(desiredZ,target_camera_tilt,0.1f);
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, desiredZ);
        body.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }


    private Vector3 GetInput_Translation()
    {
        Vector3 input_dir = Vector3.zero;

        input_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3.ClampMagnitude(input_dir, 1);

        return input_dir;
    }


    void OnTriggerEnter(Collider other)
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        
    }

    
}
