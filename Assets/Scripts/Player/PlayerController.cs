using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    private const int MAX_CONTACTS = 4;

    public float groundSpeed = 5f;
    public float airSpeed = 3f;
    public float groundJumpSpeed = 10f;
    public float wallJumpSpeed = 6f;
    public float groundAcceleration = 5f;
    public float airAcceleration = 1f;
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
        GameHandler.instance.PlayerGotKilled();
        var ps = GetComponentInChildren<ParticleSystem>();
        ps.transform.parent = null;
        ps.Play();
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
        if (contactAmount == 0)
        {
            return;
        }
        for (var i = 0; i < contactAmount; i++)
        {
            var contactPoint = contacts[i];
            var angle = Vector2.Angle(Vector2.up, contactPoint.normal);
            if (angle < wallAngle)
            {
                _contactNormal = contactPoint.normal;
                _lastClimbingTime = time;
                if (angle < groundAngle)
                {
                    _grounded = true;
                    _lastGroundedTime = time;
                }
            }
        }
    }

    private void UpdateMove()
    {
        var body2D = GetComponent<Rigidbody2D>();

        var momentum = body2D.velocity;
        float normalAngle;
        float move;
        float acceleration;
        if (_grounded)
        {
            normalAngle = Vector2.Angle(Vector2.up, _contactNormal);
            move = MoveDirection * groundSpeed;
            acceleration = groundAcceleration;
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

        
        var directedMoveX = Mathf.MoveTowards(momentum.x, move, acceleration * Time.fixedDeltaTime);

        var directedVelocity = new Vector2(directedMoveX, directedMomentum.y);
        body2D.velocity = Rotate(directedVelocity, -normalAngle);
    }

    private void UpdateJump()
    {
        var time = Time.timeSinceLevelLoad;
        if (_jumpInputTime + inputJumpEarlyBias < time)
        {
            return;
        }

        var body2D = GetComponent<Rigidbody2D>();
        var momentum = body2D.velocity;
        var jump = Vector2.zero;
        if (_lastGroundedTime + inputJumpLateBias > Time.timeSinceLevelLoad)
        {
            jump = Vector2.up * groundJumpSpeed;
            ResetJump();
        }
        else if (_lastClimbingTime + inputJumpLateBias > Time.timeSinceLevelLoad)
        {
            jump = new Vector2(Mathf.Sign(_contactNormal.x), 1).normalized * wallJumpSpeed;
            ResetJump();
        }
        body2D.velocity += jump;
    }

    private void ResetJump()
    {
        _jumpInputTime = float.NegativeInfinity;
        _lastGroundedTime = float.NegativeInfinity;
        _lastClimbingTime = float.NegativeInfinity;
        _grounded = false;
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