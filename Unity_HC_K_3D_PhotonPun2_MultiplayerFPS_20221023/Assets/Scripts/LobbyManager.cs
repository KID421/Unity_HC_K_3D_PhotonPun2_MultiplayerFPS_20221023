using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace KID
{
    /// <summary>
    /// 大廳管理器：玩家名稱、創建、加入房間
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region 介面資料
        private TMP_InputField inputFieldPlayerName;
        private TMP_InputField inputFieldCreateRoomName;
        private TMP_InputField inputFieldJoinRoomName;
        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Button btnJoinRandomRoom;
        #endregion

        private CanvasGroup groupLobbyMain;

        private void Awake()
        {
            #region 搜尋物件
            inputFieldPlayerName = GameObject.Find("輸入欄位玩家名稱").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("輸入欄位創建房間名稱").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("輸入欄位加入房間名稱").GetComponent<TMP_InputField>();
            btnCreateRoom = GameObject.Find("按鈕創建房間").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("按鈕加入房間").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("按鈕加入隨機房間").GetComponent<Button>();
            #endregion

            groupLobbyMain = GameObject.Find("遊戲大廳主要介面").GetComponent<CanvasGroup>();

            JoinMaster();
        }

        /// <summary>
        /// 加入 Photon 伺服器
        /// </summary>
        private void JoinMaster()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 加入伺服器成功會執行的方法
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            groupLobbyMain.interactable = true;
            print("<color=yellow>加入伺服器成功！</color>");
        }
    }
}
