public interface IInputProvider
{
    float GetMoveInput();
    bool GetJumpPressed();
    void ClearJump();
}