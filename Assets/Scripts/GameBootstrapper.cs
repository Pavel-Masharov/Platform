using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Collider2D _platform;
    [SerializeField] private InputController _inputController;

    private void Start()
    {
        _inputController.InitializeInputSystem();
        _character.Initialize(_inputController.compositeInput, _platform);
    }

}
