using UnityEngine;
using UnityEngine.InputSystem;

public class TrackedObjectController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float roomHalfSize = 5.2f;

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                input.y += 1;
            if (Keyboard.current.sKey.isPressed)
                input.y -= 1;
            if (Keyboard.current.aKey.isPressed)
                input.x -= 1;
            if (Keyboard.current.dKey.isPressed)
                input.x += 1;

            if (Keyboard.current.upArrowKey.isPressed)
                input.y += 1;
            if (Keyboard.current.downArrowKey.isPressed)
                input.y -= 1;
            if (Keyboard.current.leftArrowKey.isPressed)
                input.x -= 1;
            if (Keyboard.current.rightArrowKey.isPressed)
                input.x += 1;
        }

        Vector3 movement = new Vector3(input.x, 0f, input.y);
        transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);

        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -roomHalfSize, roomHalfSize);
        position.z = Mathf.Clamp(position.z, -roomHalfSize, roomHalfSize);
        transform.position = position;
    }
}
