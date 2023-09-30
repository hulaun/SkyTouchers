using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get =>  instance; }

    private float dirX;
    public float DirX { get => dirX; }

    private bool jumpInput;
    public bool JumpInput { get => jumpInput; }

    private bool dash;
    public bool Dash { get => dash; }

    private bool playerMeleeAttack;
    public bool PlayerMeleeAttack { get => playerMeleeAttack; }
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null) Debug.LogError("InputManagementNull");
        InputManager.instance= this;
    }

    private void Update()
    {
        Players();
    }
    public virtual void Players()
    {
        this.dirX = Input.GetAxisRaw("Horizontal");
        this.jumpInput = Input.GetButtonDown("Jump");
        this.dash = Input.GetKeyDown(KeyCode.K);
        this.playerMeleeAttack = Input.GetKeyDown(KeyCode.J);
    }
}
