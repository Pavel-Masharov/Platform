using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Button _jumpButton;

    private CompositeInput _compositeInput;
    private UIInput _uiInput;

    public CompositeInput compositeInput => _compositeInput;

    public void InitializeInputSystem()
    {
        SetupInput();
        SetupUIButtons();
    }

    private void SetupInput()
    {
        _compositeInput = new CompositeInput();
        _compositeInput.AddProvider(new KeyboardInput());
        _uiInput = new UIInput();
        _compositeInput.AddProvider(_uiInput);
    }

    private void SetupUIButtons()
    {
        if (_leftButton != null)
        {
            var leftTrigger = _leftButton.gameObject.AddComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => _uiInput.SetMoveInput(-1f));
            var pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => _uiInput.SetMoveInput(0f));
            leftTrigger.triggers.Add(pointerDown);
            leftTrigger.triggers.Add(pointerUp);
        }

        if (_rightButton != null)
        {
            var rightTrigger = _rightButton.gameObject.AddComponent<EventTrigger>();
            var pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((data) => _uiInput.SetMoveInput(1f));
            var pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((data) => _uiInput.SetMoveInput(0f));
            rightTrigger.triggers.Add(pointerDown);
            rightTrigger.triggers.Add(pointerUp);
        }

        if (_jumpButton != null)       
            _jumpButton.onClick.AddListener(() => _uiInput.RequestJump());
        
    }
}
