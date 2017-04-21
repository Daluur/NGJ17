using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float moveAcceleration;

    public float MoveDirection
    {
        get { return _moveDirection; }
        set { _moveDirection = Mathf.Clamp(value, -1, 1); }
    }

    private float _moveDirection;

    private void OnValidate()
    {
        moveSpeed = Mathf.Max(0, moveSpeed);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var body2D = GetComponent<Rigidbody2D>();

        // Move
        var move = body2D.velocity.x;
        if (Mathf.Sign(move) != Mathf.Sign(MoveDirection))
        {
            move = 0f;
        }
        var acc = moveAcceleration * Time.fixedTime;

        move = Mathf.MoveTowards(move, MoveDirection * moveSpeed, acc);

        body2D.velocity = new Vector2(move, body2D.velocity.y);
    }
}