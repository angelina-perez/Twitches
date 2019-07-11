using UnityEngine;
using UnityEngine.Assertions;

public class Hen : MonoBehaviour
{
	//Needs to be shown in Inspector
    [SerializeField] private string moveAxis;
    [SerializeField] private string hideButton;
	[SerializeField] private string jumpButton;
	[SerializeField] private string peckButton;
    [SerializeField] private string chirpAxis;
    [SerializeField] private float speed = 50f;
	[SerializeField] private float jumpPower = 50f;
	[SerializeField] Transform groundCheck;

	//Does not need to be shown
	private bool grounded;
	private bool pecking;
	private bool flipped;
    Rigidbody2D rb;
	Animator anim;
    Transform _transform;
    SpriteRenderer mySpriteRenderer;
    
    void Awake()
    {
        _transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		NullChecking();
    }

	void NullChecking()
	{
		Assert.IsNotNull(moveAxis);
		Assert.IsNotNull(hideButton);
		Assert.IsNotNull(jumpButton);
		Assert.IsNotNull(peckButton);
		Assert.IsNotNull(groundCheck);
		Assert.AreNotEqual(0, speed);
		Assert.AreNotEqual(0, jumpPower);
	}

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        HideChick();
        Jump();
		Peck();
        Whistle();
    }

    void Move()
    {
		if (!GameManager.instance.hideTheChick)
		{
			float translate = Input.GetAxis(moveAxis) * speed * Time.deltaTime;

			_transform.Translate(translate, 0, 0);

			float move = Input.GetAxisRaw(moveAxis);

			if (move < 0)
			{
				mySpriteRenderer.flipX = false;
				flipped = false;
			}
			else if (move > 0)
			{
				mySpriteRenderer.flipX = true;
				flipped = true;
			}
		}
    }

    void Jump ()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Floor"));

		if (Input.GetButtonDown(jumpButton) && grounded && !GameManager.instance.hideTheChick)
        {
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
    }

    void Whistle()
    {

        if (Input.GetButtonDown(chirpAxis))
        {
            PlaySounds.SFXInstance().PlaySound(0);

            GameManager.instance.followtheHen = false;
        }
        else if(Input.GetButtonUp(chirpAxis))
        {
            print("Im true!");
            GameManager.instance.followtheHen = true;
        }
    }

void Peck()
	{
		if (Input.GetButtonDown(peckButton) && !pecking)
		{
			pecking = true;
			

			if (flipped)
			{
				anim.SetBool("PeckingRight", true);
			}
			else
			{
				anim.SetBool("PeckingLeft", true);
			}
		}

		if (Input.GetButtonUp(peckButton))
		{
			pecking = false;
			anim.SetBool("PeckingLeft", false);
			anim.SetBool("PeckingRight", false);
		}
	}

    void HideChick()
	{
        if (Input.GetButtonDown(hideButton) || Input.GetAxis(hideButton) > 0) 
		{
			GameManager.instance.hideTheChick = true;
        }
        else if (Input.GetButtonUp(hideButton) || Input.GetAxis(hideButton) <= 0)
        {
			GameManager.instance.hideTheChick = false;
        }
	}
}
