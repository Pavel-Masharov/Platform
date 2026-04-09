using System.Collections.Generic;
using UnityEngine;

public class CompositeInput: IInputProvider
{
    private readonly List<IInputProvider> _providers = new();

    public void AddProvider(IInputProvider provider) => _providers.Add(provider);

    public float GetMoveInput()
    {
        foreach (var p in _providers)
        {
            float val = p.GetMoveInput();
            if (Mathf.Abs(val) > 0.01f) return val;
        }
        return 0f;
    }

    public bool GetJumpPressed()
    {
        foreach (var p in _providers)
            if (p.GetJumpPressed()) return true;
        return false;
    }

    public void ClearJump()
    {
        foreach (var p in _providers) p.ClearJump();
    }
}
