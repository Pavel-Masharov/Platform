public class UIInput : IInputProvider
{
    private float _moveInput;
    private bool _jumpRequested;

    public float GetMoveInput() => _moveInput;
    public bool GetJumpPressed() => _jumpRequested;
    public void ClearJump() => _jumpRequested = false;

    public void SetMoveInput(float value) => _moveInput = value;
    public void RequestJump() => _jumpRequested = true;
}