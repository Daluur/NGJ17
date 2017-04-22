using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    private const int MAX_CONTACTS = 4;

    public float moveSpeed = 1f;
    public float jumpSpeed = 10f;
    public float moveAcceleration = 1f;
    public float inputJumpEarlyBias = 0.1f;
    public float inputJumpLateBias = 0.1f;
    public float jumpOvertimes = 1f;
    public float groundAngle = 90;

    private bool _grounded;
    private bool _dead;
    private bool _jumping;
    private float _moveDirection;
    private float _jumpInputTime = float.NegativeInfinity;
    private Vector2 _groundNormal;

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
        moveSpeed = Mathf.Max(0, moveSpeed);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateGrounded();
        UpdateMove();
        UpdateJump();
    }

    private void UpdateMove()
    {
        var body2D = GetComponent<Rigidbody2D>();

        var momentum = body2D.velocity;
        var normal = _groundNormal;
        var normalAngle = Vector2.Angle(Vector2.up, normal);
        if (normal.x < 0)
        {
            normalAngle = -normalAngle;
        }

        var directedMomentum = Rotate(momentum, normalAngle);

        var move = MoveDirection * moveSpeed;
        var acceleration = moveAcceleration * Time.fixedTime;

        var directedMoveX = Mathf.MoveTowards(momentum.x, move, acceleration);

        var directedVelocity = new Vector2(directedMoveX, directedMomentum.y);
        body2D.velocity = Rotate(directedVelocity, -normalAngle);
    }

    private void UpdateJump()
    {
        if (_grounded &&
            _jumpInputTime + inputJumpEarlyBias >= Time.timeSinceLevelLoad)
        {
            var body2D = GetComponent<Rigidbody2D>();
            body2D.velocity = new Vector2(body2D.velocity.x, jumpSpeed);
        }
    }

    private void UpdateGrounded()
    {
        _grounded = false;
        _groundNormal = Vector2.up;
        var collider = GetComponent<BoxCollider2D>();
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
            Debug.Log(angle);
            if (angle < groundAngle)
            {
                _groundNormal = contactPoint.normal;
                _grounded = true;
            }
        }
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