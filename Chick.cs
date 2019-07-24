using System;
using UnityEngine;
using UnityEngine.Assertions;

public class Chick : MonoBehaviour
{
	//Needs to be shown in Inspector
    [SerializeField] private float speed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float letWalk;
    [SerializeField] private Transform targetObj;

    //Does Not Need to be shown
    private bool hide;
    private bool following;
	private bool isMoving;
    private bool content;
	SpriteRenderer mySpriteRenderer;
	Animator anim;
    public float stopDistract = 0f;
    public float timerToChange = 3f;

	void Awake()
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

		NullChecking();
    }

	void Update()
	{
		Follow();
		Flip();
		HideChick();
		SetAnimBools();
       
    }

    private void OnParticleCollision()
    {
        print("I'm Scared");
    }

    void NullChecking()
	{
		//Check to make sure values have been set in Inspector
		Assert.AreNotEqual(0, speed);
		Assert.AreNotEqual(0, stoppingDistance);
		Assert.IsNotNull(targetObj);
	}

    public void SetTarget(Transform newTarget)
    { 
        targetObj = newTarget;
        timerToChange -= Time.deltaTime;
    }

    void Follow()
    {
        var henPos = new Vector2(targetObj.position.x, transform.position.y);
        following = GameManager.instance.followtheHen;

		if (Vector2.Distance(transform.position, targetObj.position) > stoppingDistance && following && content)
		{
			transform.position = Vector2.MoveTowards(transform.position, henPos, speed * Time.deltaTime);
			isMoving = true;
		}
        if (Vector2.Distance(transform.position, targetObj.position) > letWalk && following && content)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }


    void Flip()
	{
		if (targetObj.position.x > transform.position.x)
		{
			mySpriteRenderer.flipX = false;
		}

		else if (targetObj.position.x < transform.position.x)
		{
			mySpriteRenderer.flipX = true;
		}
	}

	void HideChick()
	{
		hide = GameManager.instance.hideTheChick;

		print("Is moving: " + isMoving);
		print("Hiding: " + hide);

		if (!isMoving && hide)
		{
			anim.SetBool("Hiding", true);
            content = true;
			//mySpriteRenderer.enabled = false;
		}

		else if (isMoving || !hide)
		{
			anim.SetBool("Hiding", false);
		}
	}

   
    void SetAnimBools()
	{
		anim.SetBool("isMoving", isMoving);
	}
}
