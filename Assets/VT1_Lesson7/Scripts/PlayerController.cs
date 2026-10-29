using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("* * * 移動設定 * * *")]
    [SerializeField] private float _moveSpeed;

    [Header("* * * カメラ設定 * * *")]
    [SerializeField] private CameraController _cameraController;

    [Header("* * * Player Input * * *")]
    [SerializeField] private PlayerInput _playerInput;

    // private 変数
    private Vector3 _velocity; // 移動ベクトル
    private Vector3 _inputMoveVector; // 移動用の入力値

    void Start()
    {
        MoveInputEnable();
    }

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        var inputAxis = new Vector3(_inputMoveVector.x, 0, _inputMoveVector.y);

        // カメラの向いている方向を移動ベクトルとする。
        // 1.カメラの正面ベクトルを取得する
        var cameraForward = _cameraController.transform.forward;
        // 2.入力値を合わせる
        var moveVector = (inputAxis.x * _cameraController.transform.right) + (inputAxis.z * cameraForward);
        moveVector.y = 0;

        _velocity = moveVector.normalized;

        Vector3 finalVelocity = _moveSpeed * _velocity * Time.deltaTime;
        transform.Translate(finalVelocity, Space.World);
    }

    public void MoveInputEnable()
    {
        if(_playerInput == null)
        {
            Debug.LogError("PlayerInputが設定されていません。");
            return;
        }

        _playerInput.actions["Move"].performed += OnMoveInput;
        _playerInput.actions["Move"].canceled += OnMoveCanceled;

        _playerInput.actions["Move"].Enable();


        //_playerInput.actions["Move"].performed += context =>
        //{
        //    // 入力時の処理
        //    var value = context.ReadValue<Vector2>();   // 入力値の取得
        //    _inputMoveVector = value;                   // 変数に保存
        //};
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        // 入力時の処理
        var value = context.ReadValue<Vector2>();   // 入力値の取得
        _inputMoveVector = value;                   // 変数に保存
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _inputMoveVector = Vector3.zero;
    }
}
