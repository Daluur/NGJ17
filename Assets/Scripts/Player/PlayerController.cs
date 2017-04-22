using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float jumpSpeed = 10f;
    public float moveAcceleration = 1f;
    public float inputJumpEarlyBias = 0.1f;
    public float inputJumpLateBias = 0.1f;
    public float jumpOvertimes = 1f;
    public float contactBias = 0.1f;

    private bool _dead;
    private bool _jumping;
    private float _moveDirection;
    private float _jumpInputTime = float.NegativeInfinity;

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
            if (IsGrounded())
            {
                if (!_jumping && value)
                {
                    _jumpInputTime = Time.timeSinceLevelLoad;
                }
                _jumping = value;
            }
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
			GameHandler.instance.PlayerGotKilled();
		}
		else {
			LobbySceneBackground.instance.GotKill();
		}
        
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
        UpdateMove();
        UpdateJump();
    }

    private void UpdateMove()
    {
        var body2D = GetComponent<Rigidbody2D>();

        var momentum = body2D.velocity;
        var normal = GetGroundNormal();
        var normalAngle = Vector2.Angle(Vector2.up, normal);

        var directedMomentum = Rotate(momentum, normalAngle);

        var move = MoveDirection * moveSpeed;
        var acceleration = moveAcceleration * Time.fixedTime;

        var directedMoveX = Mathf.MoveTowards(momentum.x, move, acceleration);

        var directedVelocity = new Vector2(directedMoveX, directedMomentum.y);
        body2D.velocity = Rotate(directedVelocity, -normalAngle);
    }


    private void UpdateJump()
    {
        if (IsGrounded() &&
            _jumpInputTime + inputJumpEarlyBias >= Time.timeSinceLevelLoad)
        {
            var body2D = GetComponent<Rigidbody2D>();
            body2D.velocity = new Vector2(body2D.velocity.x, jumpSpeed);
        }
    }

    private bool IsGrounded ()
    {
        var collider = GetComponent<BoxCollider2D>();
        var hit = Physics2D.Raycast(transform.position, Vector2.down);
        var closest = collider.bounds.ClosestPoint(hit.point);
        if (Vector2.Distance(closest, hit.point) < contactBias)
        {
            return true;
        }
        return false;
    }

    // Returns the upward vector for the player, used to determine move direction.
    private Vector2 GetGroundNormal()
    {
        var collider = GetComponent<BoxCollider2D>();
        var hit = Physics2D.Raycast(transform.position, Vector2.down);
        var closest = collider.bounds.ClosestPoint(hit.point);
        if (Vector2.Distance(closest, hit.point) < contactBias)
        {
            return hit.normal;
        }
        return Vector2.up;
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