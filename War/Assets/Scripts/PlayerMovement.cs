using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    CharacterController cController;
    Animator animator;

    PhotonView pW;
    [SerializeField] Camera _cameraControl;

    [SerializeField] // karakter hareket h�z�
    float _speed;
    [SerializeField]
    float _gravityForce = -9.81f;  // yer �ekimi h�z�
    [SerializeField]
    float jumpHeight = 3;

    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    float groundDistance = 0.2f; // k�renin yar� �ap�
    [SerializeField]
    LayerMask groundMask;
    bool isGrounded;

    Vector3 velocity; // yer �ekimi i�in karakter h�z� depolanacak

    float x; // horizontal h�z
    float z; //vertical h�z

    // mouse
    [Range(50, 500)]
    public float mouseSensitivity;

    public Transform playerBody;
    public Transform cam;

    float xRotation = 0f;

    private void Start()
    {
        cController = GetComponent<CharacterController>();
        pW = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(pW.IsMine)
        {         
            Movement();
            SetAnimaton();
            Gravity();
            Jump();
            MouseLookCamera();
        }
        else
        {
            Destroy(_cameraControl);
        }
    }

    void Movement()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        cController.Move(move * _speed * Time.deltaTime);

        if (!isGrounded)
        {
            _speed = 2.5f;
        }
        else
        {
            _speed = 3f;
        }
    }

    void Gravity()
    {
        velocity.y += _gravityForce * Time.deltaTime; // burda velocity de�erini hem altta hem �stte time.deltatime �arpt�n ��nk� serbest d���� fizi�i = yer �ekimi kuvveti �arp� zaman�n karesi 

        cController.Move(velocity * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // isGrounded bool u olu�turdu�umuz checkSphere e g�re (groundCheck pozisyonunda , distance yar��ap�nda, groundMask katman�nda) true yada false diyecek
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;  // -2 dememizin sebebi 0 ken �ok iyi �al��m�yor hafif bir kuvvet daha iyi �al��t� 
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravityForce);  // bu olay basit fizik denklemi zplmaka i�in uygun g�c� bize veriyor z�plamak istei�imiz y�kseklik jump height gerkeen g�c� �u denklem sa�lad� == z�plamak iste�imiz y�ksekli�in kare k�k� �arp� -2 �arp� yer �ekimi kuvveti bize istei�imiz y�ksekli�e ��karacak g�c� yarat� sqrt metodu da karek k�k al�yo
            animator.SetBool("isJump", true);
        }
        else
        {
            animator.SetBool("isJump", false);
        }
    }

    void MouseLookCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30, 30);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void SetAnimaton()
    {
        //sabit durma
        if (z == 0 && x == 0)
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isWalkBack", false);
            animator.SetBool("isWalkRight", false);
            animator.SetBool("isWalkLeft", false);         
        }
        //ileri y�r� animasyon
        if (z > 0)
        {
            animator.SetBool("isWalk", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("�sRunningJump", true);
            }
            else
            {
                animator.SetBool("�sRunningJump", false);
            }
        }
        else if (z < 0)
        {
            animator.SetBool("isWalkBack", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("�sRunningJump", true);
            }
            else
            {
                animator.SetBool("�sRunningJump", false);
            }
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isWalkBack", false);
        }
        //
        //horizontal  y�r�
        if (x > 0)
        {
            animator.SetBool("isWalkRight", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("�sRunningJump", true);
            }
            else
            {
                animator.SetBool("�sRunningJump", false);
            }
        }
        else if (x < 0)
        {
            animator.SetBool("isWalkLeft", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("�sRunningJump", true);
            }
            else
            {
                animator.SetBool("�sRunningJump", false);
            }
        }
        else
        {
            animator.SetBool("isWalkRight", false);
            animator.SetBool("isWalkLeft", false);
        }
        //
    }
}
