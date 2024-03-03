using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class PauseMenu : MonoBehaviour
{
    private PlayerInput _input;
    public GameObject pauseUI;
    private void Start()
    {
        _input = InputManager.inputActions;
        _input.Movement.Escape.performed += PauseEscapePerformed;
        _input.Pause.Disable();
        _input.Pause.Escape.performed += ResumeEscapePerformed;
    }
    private void ResumeEscapePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        pauseUI.SetActive(false);
        _input.Pause.Disable();
        _input.Movement.Enable();
    }
    private void PauseEscapePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        pauseUI.SetActive(true);
        _input.Pause.Enable();
        _input.Movement.Disable();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
