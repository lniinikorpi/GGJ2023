using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private InputActions m_actions;

    private void Start() {
        m_actions = new InputActions();
        m_actions.Player.MouseMove.performed += HandleMouseMove;
        m_actions.Player.MouseClick.performed +=  HandleMouseClick;
        m_actions.Player.MouseSecondaryClick.performed += HandleMouseSecondaryClick;
        m_actions.Player.Pause.performed += HandleEscape;
        m_actions.Enable();
    }

    private void HandleMouseMove(InputAction.CallbackContext ctx) {
        GridManager.Instance.HandleMouseMove(ctx.ReadValue<Vector2>());
    }

    private void HandleMouseClick(InputAction.CallbackContext ctx) {
        GridManager.Instance.HandleMouseClick();
    }

    private void HandleMouseSecondaryClick(InputAction.CallbackContext ctx) {
        GridManager.Instance.HandleMouseSecondaryClick();
    }

    private void HandleEscape(InputAction.CallbackContext ctx) {
        if (GameManager.Instance.isPause) {
            GlobalEventSender.SendContinue(new EventArgs());
        }
        else {
            GlobalEventSender.SendPause(new EventArgs());
        }
    }

    private void OnDisable() {
        m_actions.Player.MouseMove.performed -= HandleMouseMove;
        m_actions.Player.MouseClick.performed -= HandleMouseClick;
        m_actions.Player.MouseSecondaryClick.performed -= HandleMouseSecondaryClick;
        m_actions.Player.Pause.performed -= HandleEscape;
        m_actions.Disable();
    }
}
