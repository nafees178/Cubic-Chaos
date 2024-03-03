using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Movement : MonoBehaviourPun
{
    [Header ("Basic Movements")]
    [Space]
    public float currentSpeed = 5.0f;
    public float normalSpeed = 5f;
    public float jumpForce = 8.0f;
    public bool isGrounded;

    [Header ("Gravity")]
    [Space]
    public float gravity = 15.0f;

    [Header ("Dash Settings")]
    [Space]
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.5f;
    [SerializeField]private bool isDashing = false;
    [SerializeField]private float dashTime = 0.0f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    [Header ("Freeze Settings")]
    [Space]
    public float freezeTime = 2f;
    public bool isFrozen = false;
    public bool justFrozen = false;


    [Header ("Dash Particles")]
    [Space]
    public ParticleSystem rightdash;    
    public ParticleSystem leftdash;

    [Header ("Player Camera Settings")]
    [Space]
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera zoomCam;
    public CinemachineVirtualCamera upsidedownzoomCam;
    public float dashFOV = 80;
    public float defaultFOV = 60;

    private float maxDist = 1f;
    private Vector3 offsetHeight = new Vector3(0,-0.5f,0);
    private Vector3 spherePos;
    private Vector3 spherePos2;
    private PlayerInput _input;


    [Header("Player UI Settings")]
    [Space]
    public GameObject freezeIndicator;


    void Awake()
    {
        
        controller = gameObject.GetComponent<CharacterController>();
        currentSpeed = normalSpeed;
    }

    private void Start()
    {
        _input = InputManager.inputActions;
        _input.Movement.Enable();
        _input.Movement.Jump.performed += Jump_performed;
        _input.Movement.Dash.performed += Dash_performed;
        _input.Movement.Freeze.performed += Freeze_performed;
        _input.Movement.Zoom.performed += Zoom_performed;
        _input.Movement.Zoom.canceled += Zoom_canceled;

    }

    private void Zoom_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (gravity > 0)
        {
            zoomCam.Priority = 5;
        }
        else
        {
            upsidedownzoomCam.Priority = 5;
        }
    }

    private void Zoom_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (gravity > 0)
        {
            zoomCam.Priority = 15;
        }
        else
        {
            upsidedownzoomCam.Priority = 15;
        }
    }

    private void Freeze_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!isFrozen && !justFrozen)
        {
            isFrozen = true;
        }
        else if (isFrozen)
        {
            isFrozen = false;
            StartCoroutine(frozeResetor());
        }
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!isFrozen)
        {
            // Check for dashing
            if (!isDashing)
            {
                if (photonView.IsMine)
                {
                    if (controller.velocity.x > 0)
                    {
                        photonView.RPC("PlayRightDashParticle", RpcTarget.All);
                    }
                    else
                    {
                        photonView.RPC("StopRightDashParticle", RpcTarget.All);
                    }
                    if (controller.velocity.x < 0)
                    {
                        photonView.RPC("PlayLeftDashParticle", RpcTarget.All);
                    }
                    else
                    {
                        photonView.RPC("StopLeftDashParticle", RpcTarget.All);
                    }
                }

                isDashing = true;
                dashTime = 0.0f;
                // Dash in the direction of current movement
                currentSpeed = dashSpeed;
            }
        }

    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!isFrozen)
        {
            if (isGrounded)
            {
                if (!isDashing)
                {
                    moveDirection.y = jumpForce;
                }
            }

        }
    }

    void Update()
    {
        if (isFrozen)
        {
            freezeIndicator.SetActive(true);
        }
        else
        {
            freezeIndicator.SetActive(false);
        }

        if(transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y,0);
        }
        movement();
        checkIfGrounded();   
    }
    IEnumerator frozeResetor()
    {
        justFrozen = true;
        yield return new WaitForSeconds(freezeTime);
        justFrozen = false;
    }
    void checkIfGrounded()
    {
        RaycastHit hit; //get a hit variable to store the hit information
        RaycastHit hit2; //get a hit variable to store the hit information

        spherePos = (transform.position - offsetHeight);
        spherePos2 = (transform.position + new Vector3(0,0.5f,0));

        if (Physics.SphereCast(spherePos, .5f, Vector3.down, out hit, maxDist) || Physics.SphereCast(spherePos2,.5f,Vector3.up,out hit2,maxDist))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void movement()
    {
        if (!isFrozen)
        {
            float horizontalInput = _input.Movement.Move.ReadValue<float>();
            moveDirection = new Vector3(horizontalInput * currentSpeed, moveDirection.y, 0);

            // Handle dashing
            if (isDashing)
            {
                dashTime += Time.deltaTime;
                if (dashTime < dashDuration)
                {
                    if (playerCam.m_Lens.FieldOfView < dashFOV && Mathf.Abs(controller.velocity.x) > currentSpeed)
                    {
                        playerCam.m_Lens.FieldOfView += 0.5f;
                    }
                    // Continue dashing
                }
                else
                {
                    isDashing = false;
                    currentSpeed = normalSpeed;
                    // Reset the vertical velocity to prevent upward motion
                    moveDirection.y = 0f;
                }
            }
            if (!isDashing) 
            {
                if (playerCam.m_Lens.FieldOfView > defaultFOV)
                {
                    playerCam.m_Lens.FieldOfView -= 0.5f;
                }
            }

            // Apply gravity
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the character
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
    [PunRPC]
    void PlayRightDashParticle()
    {
        rightdash.Play();
    }

    [PunRPC]
    void StopRightDashParticle()
    {
        rightdash.Stop();
    }

    [PunRPC]
    void PlayLeftDashParticle()
    {
        leftdash.Play();
    }

    [PunRPC]
    void StopLeftDashParticle()
    {
        leftdash.Stop();
    }

    public void changePriority()
    {
        if (playerCam.Priority == 10)
        {
            playerCam.Priority = 7;
        }else if(playerCam.Priority == 7)
        {
            playerCam.Priority = 10;
        }
    }
}
