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

    [SerializeField] // karakter hareket hýzý
    float _speed;
    [SerializeField]
    float _gravityForce = -9.81f;  // yer çekimi hýzý
    [SerializeField]
    float jumpHeight = 3;

    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    float groundDistance = 0.2f; // kürenin yarý çapý
    [SerializeField]
    LayerMask groundMask;
    bool isGrounded;

    Vector3 velocity; // yer çekimi için karakter hýzý depolanacak

    float x; // horizontal hýz
    float z; //vertical hýz

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
        velocity.y += _gravityForce * Time.deltaTime; // burda velocity deðerini hem altta hem üstte time.deltatime çarptýn çünkü serbest düþüþ fiziði = yer çekimi kuvveti çarpý zamanýn karesi 

        cController.Move(velocity * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // isGrounded bool u oluþturduðumuz checkSphere e göre (groundCheck pozisyonunda , distance yarýçapýnda, groundMask katmanýnda) true yada false diyecek
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;  // -2 dememizin sebebi 0 ken çok iyi çalýþmýyor hafif bir kuvvet daha iyi çalýþtý 
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * _gravityForce);  // bu olay basit fizik denklemi zplmaka için uygun gücü bize veriyor zýplamak isteiðimiz yðkseklik jump height gerkeen gücü þu denklem saðladý == zýplamak isteðimiz yüksekliðin kare kökü çarpý -2 çarpý yer çekimi kuvveti bize isteiðimiz yüksekliðe çýkaracak gücü yaratý sqrt metodu da karek kök alýyo
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
        //ileri yürü animasyon
        if (z > 0)
        {
            animator.SetBool("isWalk", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("ÝsRunningJump", true);
            }
            else
            {
                animator.SetBool("ÝsRunningJump", false);
            }
        }
        else if (z < 0)
        {
            animator.SetBool("isWalkBack", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("ÝsRunningJump", true);
            }
            else
            {
                animator.SetBool("ÝsRunningJump", false);
            }
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isWalkBack", false);
        }
        //
        //horizontal  yürü
        if (x > 0)
        {
            animator.SetBool("isWalkRight", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("ÝsRunningJump", true);
            }
            else
            {
                animator.SetBool("ÝsRunningJump", false);
            }
        }
        else if (x < 0)
        {
            animator.SetBool("isWalkLeft", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("ÝsRunningJump", true);
            }
            else
            {
                animator.SetBool("ÝsRunningJump", false);
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
