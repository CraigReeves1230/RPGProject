using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MovingEntity : MonoBehaviour
{       
    private bool stayInBounds = true;    
    private Animator anim;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private float height;
    private float width;
    protected float horizontalMov;
    protected float verticalMov;
    private bool isMovingDiagonal;
    private bool bumping;
    private bool sliding;
    
    // for player following 
    protected bool isFollowing;
    private MovingEntity followTarget;
    public float followingDistance = 1.25f;
    private double angle;
    private Vector2 followerPos;
    private Vector2 targetPos;
    
    // moving speeds
    public float moveSpeed;
    public float runningSpeed; 
    protected bool isRunning;
    private float currentMoveSpeed;
    private float catchupMoveSpeed;
    private float catchupRunSpeed;
    private float bumpingMoveSpeed;
    private float bumpingRunSpeed;
    private float diagMoveSpeed;
    private float diagRunSpeed;
    
        
    // Start is called before the first frame update
    protected void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        width = spriteRenderer.localBounds.size.x;
        height = spriteRenderer.localBounds.size.y;

        // default moving speeds
        if (isFollowing)
        {
            moveSpeed = followTarget.moveSpeed;
            runningSpeed = followTarget.runningSpeed;
        }    
        currentMoveSpeed = moveSpeed;
        catchupMoveSpeed = moveSpeed * 0.8f;
        catchupRunSpeed = runningSpeed * 0.8f;
        
        bumpingMoveSpeed = moveSpeed * 0.507f;
        bumpingRunSpeed = runningSpeed * 0.507f;

        diagMoveSpeed = moveSpeed * 0.8f;
        diagRunSpeed = runningSpeed * 0.8f;
    }

    // Update is called once per frame
    protected void Update()
    {                    
        // handle non-following movement
        if (!isFollowing)
        {
            rigidBody.velocity = new Vector2(horizontalMov, verticalMov) * currentMoveSpeed;
        }
        else
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
        
        // control handle running as well as diaganol movement for non following player
        if (!isFollowing)
        {
            currentMoveSpeed = isRunning ? runningSpeed : moveSpeed;
        }
        
        //  animate non-following movement
        if (!isFollowing)
        {
            anim.SetFloat("moveX", rigidBody.velocity.x);
            anim.SetFloat("moveY", rigidBody.velocity.y);
        }
       
        if (!isFollowing)
        {
            // have non-following entity face the right directions
            if (horizontalMov > 0 || horizontalMov < 0 || verticalMov < 0 || verticalMov > 0)
            {
                anim.SetFloat("lastMoveX", horizontalMov);
                anim.SetFloat("lastMoveY", verticalMov);
                
                // stay in bounds of map if called to
                if (stayInBounds)
                {
                    // keep entity in bounds of map
                    keepEntityInBounds();
                }
            }
        }
        
        //handle diagonal movements speeding player up
        if (!isFollowing)
        {
            isMovingDiagonal = (horizontalMov != 0 && verticalMov != 0);
        }
        
        // slow player down moving diagonally
        if (isMovingDiagonal)
        {
            currentMoveSpeed = isRunning ? diagRunSpeed : diagMoveSpeed;
        }
        
        
        // for following entity, get target, follower position, angle and more
        if (isFollowing)
        {
            targetPos = new Vector2(followTarget.transform.position.x, followTarget.transform.position.y);
            followerPos = new Vector2(transform.position.x, transform.position.y);        
            angle = Mathf.Abs(Mathf.Atan2(followerPos.y - targetPos.y, followerPos.x - targetPos.x) * 180f / Mathf.PI);
            isMovingDiagonal = followTarget.getIsMovingDiagonal();
            bumping = followTarget.bumping;
            sliding = followTarget.sliding;
        }
        
        // deal with followers sliding and jittering on map borders
        if ((transform.position.y <= GameManager.instance.getBottomLeftLimit().y + height * 0.875f) ||
            (transform.position.y >= GameManager.instance.getTopRightLimit().y - height * 0.875f) || 
            (transform.position.x <= GameManager.instance.getBottomLeftLimit().x + width * 0.875f) ||
            (transform.position.x >= GameManager.instance.getTopRightLimit().x - height * 0.875f))
        {
            if (!isFollowing)
            {
                if (isMovingDiagonal)
                {
                    sliding = true;
                }
                else
                {
                    sliding = false;
                }
            }
            else
            {
                sliding = followTarget.sliding;
            }
        }
        else
        {
            sliding = false;
        }

        // adjust sorting order dynamically on y axis so lower objects appear in front
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-(gameObject.transform.position.y * 10));
    }

    // deal with followers bumping around when colliding with solid walls
    private void OnCollisionStay2D(Collision2D other)
    {
        if (!isFollowing)
        {
            if (isMovingDiagonal)
            {
                bumping = true;
            }
            else
            {
                bumping = false;
            }
        }
        else
        {
            bumping = followTarget.bumping;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        bumping = false;
    }

    void FixedUpdate()
    { 
        // follower should be moving at same speed as target and vice-versa
        if (isFollowing)
        {
            isRunning = followTarget.isRunning;
            moveSpeed = followTarget.moveSpeed;
            runningSpeed = followTarget.runningSpeed;
            currentMoveSpeed = followTarget.getCurrentMoveSpeed();
            
            // default target is party leader
            if (followTarget == null)
            {
                followTarget = GameManager.instance.partyLead();
            }
        }
        
        // handle movement for a following entity
        if (isFollowing)
        {
            // handle moving and animating follower
            if (Vector2.Distance(transform.position, followTarget.transform.position) > followingDistance)
            {
                // this is for when the follower is moving diagonally
                if (followTarget.getIsMovingDiagonal())
                {
                    // normal diagnonal following as the leader moves faster when moving diagonally
                    if (!sliding && !bumping)
                    {
                        if (!isRunning)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, catchupMoveSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, catchupRunSpeed * Time.deltaTime);
                        }
                    } else if (bumping)    // when pressing up against a colliding wall, keep follower from jittering
                    {
                        if (!isRunning)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, bumpingMoveSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, bumpingRunSpeed * Time.deltaTime);
                        }
                    } else if (sliding && !bumping)    // when pressing up against edge of map, keep follower from jittering
                    {
                        if (!isRunning)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, moveSpeed * Time.deltaTime);
                        }
                        else
                        {
                            transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, runningSpeed * Time.deltaTime);
                        }
                    }
                }
                else
                {    // this is for non-diagonal, Up, Down, Left and Right movement
                    if (!isRunning)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, runningSpeed * Time.deltaTime);
                    }
                }
                
                
                // keep follower in bounds of map
                if (stayInBounds) keepEntityInBounds();
                
                handleFollowAnimation();
            }
            else
            {
                handleStopFollowAnimation();
            }
        }
    }
   
    
    void handleFollowAnimation()
    {   
        if (angle <= 45f)
        {
            anim.SetFloat("moveX", -1);
            anim.SetFloat("moveY", 0);
            anim.SetFloat("lastMoveX", -1);
            anim.SetFloat("lastMoveY", 0);
        } else if (angle >= 120f)
        {
            anim.SetFloat("moveX", 1);
            anim.SetFloat("moveY", 0);
            anim.SetFloat("lastMoveX", 1);
            anim.SetFloat("lastMoveY", 0);
        }
        else
        {
            if (targetPos.y >= followerPos.y)
            {
                anim.SetFloat("moveX", 0);
                anim.SetFloat("moveY", 1);
                anim.SetFloat("lastMoveX", 0);
                anim.SetFloat("lastMoveY", 1);
            }
            else
            {
                anim.SetFloat("moveX", 0);
                anim.SetFloat("moveY", -1);
                anim.SetFloat("lastMoveX", 0);
                anim.SetFloat("lastMoveY", -1);
            }
        }
    }
    
    
    void handleStopFollowAnimation()
    {
        if (followTarget.anim.GetFloat("moveX") == 0 && followTarget.anim.GetFloat("moveY") == 0)
        {
           anim.SetFloat("moveX", 0);
           anim.SetFloat("moveY", 0);   
        }
    }

    
    // keep entity within bounds of map
    void keepEntityInBounds()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, GameManager.instance.getBottomLeftLimit().x + (width / 2), GameManager.instance.getTopRightLimit().x - (width / 2)),
            Mathf.Clamp(transform.position.y, GameManager.instance.getBottomLeftLimit().y + (height / 2), GameManager.instance.getTopRightLimit().y - (height / 2)));
    }
    
    
    // getters and setters
    
    public void setStayInBounds(bool setting) => stayInBounds = setting;

    public void FollowTarget(MovingEntity tgt)
    {
        followTarget = tgt;
        isFollowing = true;
    }
    
    public void stopFollowing()
    {
        isFollowing = false;
        followTarget = null;
    }
    
    public void clearDirectionalBuffer()
    {
        horizontalMov = 0f;
        verticalMov = 0f;
    }

    public float getFollowDistance(MovingEntity entity) => followingDistance;
    public MovingEntity getFollowTarget() => followTarget;
    public void setFollowDistance(float distance) => followingDistance = distance;
    public float getCurrentMoveSpeed() => currentMoveSpeed;
    public bool getIsMovingDiagonal() => isMovingDiagonal;
    public bool getIsFollowing() => isFollowing;
    
    // returns true if the target is being followed
    public bool getIsFollowing(MovingEntity target) => (isFollowing && followTarget == target);

    public bool getIsRunning(MovingEntity entity) => isRunning;
    public void setIsRunning(bool setting) => isRunning = setting;
    public void setHorizontalMov(float val) => horizontalMov = val;
    public void setVerticalMov(float val) => verticalMov = val;
    public float getHorizontalMov(MovingEntity entity) => horizontalMov;
    public float getVerticalMov(MovingEntity entity) => verticalMov;
}
