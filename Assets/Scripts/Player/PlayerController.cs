using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float moveAcceleration;
    [HideInInspector]
    public AudioClip deathClip;

    private SoundHolder soundHolder;
    private AudioSource myAudioSource;

	bool dead = false;

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

    private void Start()
    {
        soundHolder = GetComponent<SoundHolder>();
        myAudioSource = GetComponent<AudioSource>();
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

        if (Mathf.Abs(move) > 0.6f)
            RunningSound();

        move = Mathf.MoveTowards(move, MoveDirection * moveSpeed, acc);
        
        body2D.velocity = new Vector2(move, body2D.velocity.y);
        
    }

    private void RunningSound() {
        if (!myAudioSource.isPlaying) { 
        myAudioSource.clip = soundHolder.running;
        myAudioSource.Play();
        myAudioSource.volume = 1;
        }

    }
    private void JumpingSound()
    {

    }

    public void Kill() {
		if (dead) {
			return;
		}

       /* if (deathClip == null) {
            deathClip = soundHolder.death;
        }

        GameHandler.instance.MuteCurrentPlayerMusic();
        myAudioSource.clip = deathClip;
        myAudioSource.Play();
        */
        dead = true;
		GameHandler.instance.PlayerGotKilled();
        var ps = GetComponentInChildren<ParticleSystem>();
        ps.transform.parent = null;
        ps.Play();
		Destroy(gameObject);
	}
}