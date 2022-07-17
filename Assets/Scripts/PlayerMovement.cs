using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;


    public float moveSpeed;
    const float ROLLSPEED = 120f;
    float rollSpeed;
    Rigidbody2D _rb;

    public Transform gunHolder;
    public SpriteRenderer gunSprite;


    Vector2 playerInput;
    Vector2 moveVelocity;
    Vector2 mouseInput;
    Vector2 mouseDirection;

    Vector2 slideDir;
    //assuming player is using some capsule collider

    public bool isDead;

    float NextTimeToRoll;
    [HideInInspector]
    public bool rollReady;
    bool bellDing = true;

    public Animator legsAnim;
    public Animator bodyAnim;
    enum State
    {
        Normal,
        DodgeRollSliding,
    }
    State state;

    [SerializeField]
    LayerMask DodgeRollLayer;     //for when the player is dodgerolling, avoid enemies


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        //if ! game paused
        if(!isDead)
        {
            switch(state)
            {
                case State.Normal:
                    PlayerMovementInput();
                    SetGunToFaceMouse();
                    HandleDodgeRoll();

                    break;
                case State.DodgeRollSliding:
                    DoDodgeRoll();
                    break;
            }
        }
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + moveVelocity * Time.fixedDeltaTime);
       // _rb.velocity = moveVelocity;
    }

    void HandleDodgeRoll()
    {
        if(Time.time >= NextTimeToRoll)
        {
            rollReady = true;
            
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(PlayerInvulnerable());
                GunManager.instance.RollGun();
                DoDodgeRoll();
                state = State.DodgeRollSliding;
                slideDir = (Camera.main.ScreenToWorldPoint(mouseInput) - transform.position).normalized;
                rollSpeed = ROLLSPEED;
                AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[0],1f, transform.position);
                bellDing = false;
                bodyAnim.SetTrigger("Roll");
            }
        }
        if(rollReady &&!bellDing)
        {
            //maybe do some anim here 
            AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[1], 1f, transform.position);
            bellDing = true;
        }
    }

    void DoDodgeRoll()
    {
        moveVelocity = slideDir * rollSpeed;
        rollSpeed -= rollSpeed *15f* Time.deltaTime;
        if(rollSpeed <= 1)
        {
            rollReady = false;
            
            state = State.Normal;
            NextTimeToRoll = Time.time + 2f;
        }
    }

    IEnumerator PlayerInvulnerable()
    {
        gameObject.layer = 7;       //7:dodge roll layer
        yield return new WaitForSeconds(0.5f);
        gameObject.layer = 3;       //3: player layer
    }
    void PlayerMovementInput()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = playerInput * moveSpeed;
        if (moveVelocity.magnitude > 0.051f) legsAnim.SetTrigger("Walking"); 
    }
    void SetGunToFaceMouse()
    {
        mouseInput = Input.mousePosition;
        Vector3 mousePosInWord = Camera.main.ScreenToWorldPoint(mouseInput);
        mousePosInWord.z = 0;
        mouseDirection = mousePosInWord - transform.position;
        gunHolder.transform.right = mouseDirection;

        //Hacky sprite flip here
        float angle = gunHolder.transform.rotation.eulerAngles.z;
        gunSprite.flipY = (angle > 90f && angle < 270f);        //angle goes from 0 to 360. 
    }
    
}
