using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class MovingEntity : MonoBehaviour
{       
    protected bool canMove = true;    
    protected bool stayInBounds = true;    
    protected Animator anim;
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected float height;
    protected float width;
    protected float horizontalMov;
    protected float verticalMov;
    protected bool isMovingDiagonal;
    protected bool bumping;
    protected bool sliding;
    
    // for player following 
    protected bool isFollowing;
    protected MovingEntity followTarget;
    public float followingDistance = 1.25f;
    protected double angle;
    protected Vector2 followerPos;
    protected Vector2 targetPos;
    
    // moving speeds
    public float moveSpeed;
    public float runningSpeed; 
    protected bool isRunning;
    protected float currentMoveSpeed;
    protected float catchupMoveSpeed;
    protected float catchupRunSpeed;
    public float bumpingMoveSpeed;
    public float bumpingRunSpeed;
    
        
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
        catchupMoveSpeed = moveSpeed * 1.415f;
        catchupRunSpeed = runningSpeed * 1.415f;
        
        bumpingMoveSpeed = moveSpeed * 0.607f;
        bumpingRunSpeed = runningSpeed * 0.607f;

    }

    // Update is called once per frame
    protected void Update()
    {                    
        // handle non-following movement
        if (!isFollowing)
        {
            rigidBody.velocity = canMove ? new Vector2(horizontalMov, verticalMov) * currentMoveSpeed : rigidBody.velocity = new Vector2(0, 0);
        }
        else
        {
            rigidBody.velocity = new Vector2(0, 0);
        }
        
        // control handle running as well as diaganol movement for non following player
        if (!isFollowing && canMove)
        {
            currentMoveSpeed = isRunning ? runningSpeed : moveSpeed;
        }
        
        //  animate non-following movement
        if (!isFollowing)
        {
            anim.SetFloat("moveX", rigidBody.velocity.x);
            anim.SetFloat("moveY", rigidBody.velocity.y);
        }
       
        if (canMove && !isFollowing)
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
        if ((transform.position.y <= GameManager.instance.bottomLeftLimit.y + height * 0.875f) ||
            (transform.position.y >= GameManager.instance.topRightLimit.y - height * 0.875f) || 
            (transform.position.x <= GameManager.instance.bottomLeftLimit.x + width * 0.875f) ||
            (transform.position.x >= GameManager.instance.topRightLimit.x - height * 0.875f))
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
        if (canMove && isFollowing)
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
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, GameManager.instance.bottomLeftLimit.x + (width / 2), GameManager.instance.topRightLimit.x - (width / 2)),
            Mathf.Clamp(transform.position.y, GameManager.instance.bottomLeftLimit.y + (height / 2), GameManager.instance.topRightLimit.y - (height / 2)));
    }
    
    
    // getters and setters
    public void setCanMove(bool setting) => canMove = setting;
    public void setStayInBounds(bool setting) => stayInBounds = setting;

    public void FollowTarget(MovingEntity tgt)
    {
        followTarget = tgt;
        isFollowing = true;
    }
    
    public void StopFollowing()
    {
        isFollowing = false;
        followTarget = null;
    }

    public float getFollowDistance(MovingEntity entity) => followingDistance;
    public MovingEntity getFollowTarget() => followTarget;
    public void setFollowDistance(float distance) => followingDistance = distance;
    public float getCurrentMoveSpeed() => currentMoveSpeed;
    public bool getIsMovingDiagonal() => isMovingDiagonal;
    public bool getIsFollowing(MovingEntity entity) => entity.isFollowing;
    public bool getIsFollowing(MovingEntity entity, MovingEntity target) => (entity.isFollowing && entity.followTarget == target);
}
