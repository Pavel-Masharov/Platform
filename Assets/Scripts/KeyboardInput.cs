using UnityEngine;

public class KeyboardInput : IInputProvider
{
    public float GetMoveInput() => Input.GetAxis("Horizontal");
    public bool GetJumpPressed() => Input.GetButtonDown("Jump");
    public void ClearJump() { }
}