using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float speed = 5f;
    public VariableJoystick variableJoystick; // Assign the joystick in Inspector

    void Update()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + 
                           Vector3.right * variableJoystick.Horizontal;
        
        transform.position += direction * (speed * Time.deltaTime);
    }
}