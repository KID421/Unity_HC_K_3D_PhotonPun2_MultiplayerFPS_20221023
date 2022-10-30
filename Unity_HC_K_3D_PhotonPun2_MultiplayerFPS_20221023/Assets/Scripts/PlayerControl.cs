using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KID
{
    /// <summary>
    /// 玩家控制器
    /// 啟動自己的控制器與攝影機
    /// </summary>
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("攝影機")]
        private GameObject goCamera;
        [SerializeField, Header("玩家輸入控制")]
        private PlayerInput playerInput;

        private void Awake()
        {
            if (photonView.IsMine)              // 如果 是自己的物件
            {
                goCamera.SetActive(true);       // 開啟攝影機
                playerInput.enabled = true;     // 開啟輸入控制
            }
        }
    }
}
