using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

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
        private CanvasGroup groupLobbyMain;
        private CanvasGroup groupRoom;
        private Button btnStart;
        private TextMeshProUGUI textRoom;
        private TextMeshProUGUI textRoomPlayerList; 
        #endregion

        #region 私人資料
        private string namePlayer;
        private string nameCreateRoom;
        private string nameJoinRoom;
        private byte maxPlayer = 10;
        #endregion

        #region 事件
        private void Awake()
        {
            #region 搜尋物件
            inputFieldPlayerName = GameObject.Find("輸入欄位玩家名稱").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("輸入欄位創建房間名稱").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("輸入欄位加入房間名稱").GetComponent<TMP_InputField>();
            btnCreateRoom = GameObject.Find("按鈕創建房間").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("按鈕加入房間").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("按鈕加入隨機房間").GetComponent<Button>();
            groupLobbyMain = GameObject.Find("遊戲大廳主要介面").GetComponent<CanvasGroup>();

            groupRoom = GameObject.Find("房間介面").GetComponent<CanvasGroup>();
            btnStart = GameObject.Find("按鈕開始遊戲").GetComponent<Button>();
            textRoom = GameObject.Find("房間名稱").GetComponent<TextMeshProUGUI>();
            textRoomPlayerList = GameObject.Find("房間玩家名稱清單").GetComponent<TextMeshProUGUI>();
            #endregion

            JoinMaster();

            inputFieldPlayerName.onEndEdit.AddListener(input =>
            { 
                namePlayer = input;
                PhotonNetwork.NickName = namePlayer;    // 伺服器.暱稱 = 玩家的名稱
            });
            inputFieldCreateRoomName.onEndEdit.AddListener(input => nameCreateRoom = input);
            inputFieldJoinRoomName.onEndEdit.AddListener(input => nameJoinRoom = input);

            btnCreateRoom.onClick.AddListener(BtnCreateRoom);
            btnJoinRoom.onClick.AddListener(BtnJoinRoom);
            btnJoinRandomRoom.onClick.AddListener(BtnJoinRandomRoom);

            btnStart.onClick.AddListener(() => photonView.RPC("RPCStartGame", RpcTarget.All));
        }
        #endregion

        #region 私人方法
        [PunRPC]
        private void RPCStartGame()
        {
            PhotonNetwork.LoadLevel("遊戲場景");
        }

        /// <summary>
        /// 加入 Photon 伺服器
        /// </summary>
        private void JoinMaster()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 按鈕事件：建立房間
        /// </summary>
        private void BtnCreateRoom()
        {
            RoomOptions ro = new RoomOptions();             // 房間資訊
            ro.MaxPlayers = maxPlayer;                      // 最大人數
            PhotonNetwork.CreateRoom(nameCreateRoom, ro);   // PUN 建立房間(房間名稱，房間資訊)
        }

        /// <summary>
        /// 按鈕事件：加入房間
        /// </summary>
        private void BtnJoinRoom()
        {
            PhotonNetwork.JoinRoom(nameJoinRoom);
        }

        /// <summary>
        /// 按鈕事件：加入隨機房間
        /// </summary>
        private void BtnJoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        /// <summary>
        /// 更新玩家清單
        /// </summary>
        private void UpdatePlayerList()
        {
            textRoomPlayerList.text = "";

            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                if (i == 0) textRoomPlayerList.text = "房主：" + players[i].NickName;
                else textRoomPlayerList.text += "玩家：" + players[i].NickName;

                textRoomPlayerList.text += "\n";
            }
        } 
        #endregion

        #region Photon 事件
        /// <summary>
        /// 加入伺服器成功會執行的方法
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            groupLobbyMain.interactable = true;
            print("<color=yellow>加入伺服器成功！</color>");
        }

        /// <summary>
        /// 創建房間成功會執行的方法
        /// </summary>
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print($"<color=orange>成功創建房間，房間名稱：{ nameCreateRoom } </color>");

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;
            btnStart.interactable = true;

            textRoom.text = "房間名稱：" + nameCreateRoom;
            textRoomPlayerList.text = "";
            textRoomPlayerList.text = "房主：" + namePlayer;
        }

        /// <summary>
        ///  加入房間成功後會執行的方法
        /// </summary>
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoom.text = "房間名稱：" + PhotonNetwork.CurrentRoom.Name;

            UpdatePlayerList();
        }

        /// <summary>
        /// 有新玩家加入房間時會執行一次的方法
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            UpdatePlayerList();
        } 
        #endregion
    }
}
