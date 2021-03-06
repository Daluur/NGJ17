﻿using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public AudioClip deathClip;

    private SoundHolder soundHolder;
    private AudioSource myAudioSource;

    private const int MAX_CONTACTS = 4;

    public float groundSpeed = 5f;
    public float airSpeed = 5f;
    public float groundJumpSpeed = 10f;
    public float wallJumpSpeed = 10f;
    public float groundAcceleration = 50f;
    public float airAcceleration = 30f;

    public float inputJumpEarlyBias = 0.1f;
    public float inputJumpLateBias = 0.1f;
    public float jumpMaxOvertime = 1f;
    public float groundAngle = 60;
    public float wallAngle = 100;

    private bool _dead;
    private bool _jumping;
    private bool _grounded;
    private float _moveDirection;
    private float _jumpInputTime = float.NegativeInfinity;
    private float _jumpOvertimeTime = float.NegativeInfinity;
    private float _lastGroundedTime = float.NegativeInfinity;
    private float _lastClimbingTime = float.NegativeInfinity;
    private Vector2 _contactNormal;

    public float MoveDirection
    {
        get { return _moveDirection; }
        set { _moveDirection = Mathf.Clamp(value, -1, 1); }
    }

    public bool Jumping
    {
        get { return _jumping; }
        set
        {
            if (!_jumping && value)
            {
                _jumpInputTime = Time.timeSinceLevelLoad;
            }
            else if (!value)
            {
                _jumpOvertimeTime = float.NegativeInfinity;
            }
            _jumping = value;
        }
    }

    public void Kill()
    {
        if (_dead)
        {
            return;
        }
        _dead = true;
		if(GameHandler.instance != null) {
			GameHandler.instance.PlayerGotKilled(this);
		}
		else {
			LobbySceneBackground.instance.GotKill();
		}
        
        GetComponentInChildren<ParticleCollision>().ActivateParticleSystem();
        Destroy(gameObject);
    }

    private void OnValidate()
    {
        groundSpeed = Mathf.Max(0, groundSpeed);
        airSpeed = Mathf.Max(0, airSpeed);
        groundAcceleration = Mathf.Max(0, groundAcceleration);
        airAcceleration = Mathf.Max(0, airAcceleration);
        groundJumpSpeed = Mathf.Max(0, groundJumpSpeed);
        wallJumpSpeed = Mathf.Max(0, wallJumpSpeed);
        inputJumpEarlyBias = Mathf.Max(0, inputJumpEarlyBias);
        inputJumpLateBias = Mathf.Max(0, inputJumpLateBias);
        jumpMaxOvertime = Mathf.Max(0, jumpMaxOvertime);
        groundAngle = Mathf.Max(0, groundAngle);
        wallAngle = Mathf.Max(0, wallAngle);
    }

    private void Start()
    {
        soundHolder = GetComponent<SoundHolder>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateCollision();
        UpdateJump();
        UpdateMove();
    }

    private void UpdateCollision()
    {
        _grounded = false;
        var time = Time.timeSinceLevelLoad;
        _contactNormal = Vector2.up;
        var collider = GetComponent<Collider2D>();
        var contacts = new ContactPoint2D[MAX_CONTACTS];
        var contactAmount = collider.GetContacts(contacts);
        var groundNormals = new List<Vector2>();
        var wallNormals = new List<Vector2>();
        if (contactAmount == 0)
        {
            return;
        }
        for (var i = 0; i < contactAmount; i++)
        {
            var contactPoint = contacts[i];
            var angle = Vector2.Angle(Vector2.up, contactPoint.normal);
            if (angle < groundAngle)
            {
                groundNormals.Add(contactPoint.normal);
                _grounded = true;
                _lastGroundedTime = time;
            }
            else if (angle < wallAngle)
            {
                wallNormals.Add(contactPoint.normal);
                _lastClimbingTime = time;
            }
            else
            {
                _jumpOvertimeTime = float.NegativeInfinity;
            }
        }
        if (groundNormals.Count > 0)
        {
            _contactNormal = AverageNormal(groundNormals.ToArray());
        }
        else if (wallNormals.Count > 0)
        {
            _contactNormal = AverageNormal(wallNormals.ToArray());
        }
    }

    private void UpdateMove()
    {
        var body2D = GetComponent<Rigidbody2D>();

        var momentum = body2D.velocity;
        float acceleration;
        float normalAngle;
        float move;
        if (_grounded || transform.parent != null)
        {
            acceleration = groundAcceleration;
            normalAngle = Vector2.Angle(Vector2.up, _contactNormal);
            move = MoveDirection * groundSpeed;
        }
        else
        {
            acceleration = airAcceleration;
            normalAngle = 0f;
            if (MoveDirection == 0)
            {
                move = momentum.x;
            }
            else
            {
                move = MoveDirection * Mathf.Max(Mathf.Sign(MoveDirection) * momentum.x, airSpeed);
            }
        }
        if (_contactNormal.x < 0)
        {
            normalAngle = -normalAngle;
        }

        var directedMomentum = Rotate(momentum, normalAngle);

        var directedMoveX = Mathf.MoveTowards(directedMomentum.x, move, acceleration * Time.fixedDeltaTime);

        var directedVelocity = new Vector2(directedMoveX, directedMomentum.y);
        body2D.velocity = Rotate(directedVelocity, -normalAngle);

        if (Mathf.Abs(directedMoveX) > 0.6f)
        {
            RunningSound();
        }
    }

    private void RunningSound()
    {
        if (!myAudioSource.isPlaying)
        { 
            myAudioSource.clip = soundHolder.running;
            myAudioSource.Play();
            myAudioSource.volume = 1;
        }
    }

    private void JumpingSound()
    {
        myAudioSource.clip = soundHolder.jumping;
        myAudioSource.Play();
        myAudioSource.volume = 1;
    }

    private void UpdateJump()
    {
        var time = Time.timeSinceLevelLoad;
        var isJumpFlag = _jumpInputTime + inputJumpEarlyBias > time;
        var isOvertimeFlag = _jumpOvertimeTime + jumpMaxOvertime > time;
        if (!isJumpFlag && !isOvertimeFlag)
        {
            return;
        }

        var body2D = GetComponent<Rigidbody2D>();
        var momentum = body2D.velocity;
        var jump = momentum;
        if (((_lastGroundedTime + inputJumpLateBias > Time.timeSinceLevelLoad || transform.parent != null) && isJumpFlag) ||
            isOvertimeFlag)
        {
            jump = new Vector2(momentum.x, groundJumpSpeed);
            ResetJump();
            if (isJumpFlag)
            {
                _jumpOvertimeTime = time;
                JumpingSound();
            }
        }
        else if (_lastClimbingTime + inputJumpLateBias > Time.timeSinceLevelLoad && isJumpFlag)
        {
            jump = new Vector2(Mathf.Sign(_contactNormal.x), 1).normalized * wallJumpSpeed;
            ResetJump();
            if (isJumpFlag)
            {
                _jumpOvertimeTime = time;
                JumpingSound();
            }
        }
        body2D.velocity = jump;
    }

    private void ResetJump()
    {
        _jumpInputTime = float.NegativeInfinity;
        _lastGroundedTime = float.NegativeInfinity;
        _lastClimbingTime = float.NegativeInfinity;
        _grounded = false;
    }

    private static Vector2 AverageNormal(Vector2[] vectors)
    {
        var x = 0f;
        var y = 0f;
        foreach (var vector in vectors)
        {
            x += vector.x;
            y += vector.y;
        }
        return new Vector2(x / vectors.Length, y / vectors.Length).normalized;
    }

    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}