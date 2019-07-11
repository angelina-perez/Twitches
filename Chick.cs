using UnityEngine;
using UnityEngine.Assertions;

public class Chick : MonoBehaviour
{
	//Needs to be shown in Inspector
	[SerializeField] private float speed;
	[SerializeField] private float stoppingDistance;
	[SerializeField] private Transform hen;

	//Does Not Need to be shown
	private bool hide;
    public bool following;
	private bool isMoving;
	SpriteRenderer mySpriteRenderer;

	void Awake()
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();

		NullChecking();
        //following = GameManager.instance.followtheHen;
    }

	void Update()
	{
		Follow();
		Flip();
		HideChick();	
	}

	void NullChecking()
	{
		//Check to make sure values have been set in Inspector
		Assert.AreNotEqual(0, speed);
		Assert.AreNotEqual(0, stoppingDistance);
		Assert.IsNotNull(hen);
	}

    void Follow()
    {
        var henPos = new Vector2(hen.position.x, transform.position.y);
        following = GameManager.instance.followtheHen;

        if (Vector2.Distance(transform.position, hen.position) > stoppingDistance && following)
        { 
        transform.position = Vector2.MoveTowards(transform.position, henPos, speed * Time.deltaTime);
			isMoving = true;
		}

		else
		{
			isMoving = false;
		}

	}


    void Flip()
	{
		if (hen.position.x > transform.position.x)
		{
			mySpriteRenderer.flipX = true;
		}
		else if (hen.position.x < transform.position.x)
		{
			mySpriteRenderer.flipX = false;
		}
	}

	void HideChick()
	{
		hide = GameManager.instance.hideTheChick;

		if (!isMoving && hide)
		{
			mySpriteRenderer.enabled = false;
		}
		else if (isMoving || !hide)
		{
			mySpriteRenderer.enabled = true;
		}
	}
}
