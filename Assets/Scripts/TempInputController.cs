using UnityEngine;

public class TempInputController : MonoBehaviour
{
    public PlayerController player;

    // Update is called once per frame
    private void Update()
    {
        var direction = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            direction -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += 1f;
        }
        player.MoveDirection = direction;
    }
}