using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;


    public float moveSpeed;
    const float ROLLSPEED = 100f;
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

    float maxHp = 20;
    public float currHP;
    public bool isDead;
    public Image playerHPBar;
    public Animator playerAnim;

    enum State
    {
        Normal,
        DodgeRollSliding,
    }
    State state;

    [SerializeField]
    LayerMask RollingLayer;     //for when the player is dodgerolling, avoid enemies


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        currHP = maxHp;
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
        UpdateHP();
    }
    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + moveVelocity * Time.fixedDeltaTime);
       // _rb.velocity = moveVelocity;
    }
    void UpdateHP()
    {
        
       // playerHPBar.fillAmount = currHP / maxHp;
    }

    void HandleDodgeRoll()
    {
        if(Input.GetMouseButtonDown(1))
        {
            DoDodgeRoll();
            state = State.DodgeRollSliding;
            slideDir = (Camera.main.ScreenToWorldPoint(mouseInput) - transform.position).normalized;
            rollSpeed = ROLLSPEED;
        }
    }

    void DoDodgeRoll()
    {
        //transform.position += (Vector3)slideDir * rollSpeed * Time.deltaTime;
        moveVelocity = slideDir * rollSpeed;
        rollSpeed -= rollSpeed *15f* Time.deltaTime;
        if(rollSpeed <= 5)
        {
            state = State.Normal;
        }

    }


    void PlayerMovementInput()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = playerInput * moveSpeed;
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
    public void TakeDamage(float damage)
    {
        currHP -= damage;
        playerAnim.SetTrigger("hurt");
        //AudioManager.instance.PlaySoundAtLocation(AudioManager.instance.MiscSounds[6], 0.3f, transform.position);
        if (currHP <= 0)
        {
            isDead = true;
        }
    }
    public void AddHP(float health)
    {
        currHP += health;
        if (currHP > maxHp) currHP = maxHp;
    }
}