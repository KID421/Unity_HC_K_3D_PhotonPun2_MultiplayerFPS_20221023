using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace KID
{
    /// <summary>
    /// �j�U�޲z���G���a�W�١B�ЫءB�[�J�ж�
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        #region �������
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

        #region �p�H���
        private string namePlayer;
        private string nameCreateRoom;
        private string nameJoinRoom;
        private byte maxPlayer = 10;
        #endregion

        #region �ƥ�
        private void Awake()
        {
            #region �j�M����
            inputFieldPlayerName = GameObject.Find("��J��쪱�a�W��").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("��J���Ыةж��W��").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("��J���[�J�ж��W��").GetComponent<TMP_InputField>();
            btnCreateRoom = GameObject.Find("���s�Ыةж�").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("���s�[�J�ж�").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("���s�[�J�H���ж�").GetComponent<Button>();
            groupLobbyMain = GameObject.Find("�C���j�U�D�n����").GetComponent<CanvasGroup>();

            groupRoom = GameObject.Find("�ж�����").GetComponent<CanvasGroup>();
            btnStart = GameObject.Find("���s�}�l�C��").GetComponent<Button>();
            textRoom = GameObject.Find("�ж��W��").GetComponent<TextMeshProUGUI>();
            textRoomPlayerList = GameObject.Find("�ж����a�W�ٲM��").GetComponent<TextMeshProUGUI>();
            #endregion

            JoinMaster();

            inputFieldPlayerName.onEndEdit.AddListener(input =>
            { 
                namePlayer = input;
                PhotonNetwork.NickName = namePlayer;    // ���A��.�ʺ� = ���a���W��
            });
            inputFieldCreateRoomName.onEndEdit.AddListener(input => nameCreateRoom = input);
            inputFieldJoinRoomName.onEndEdit.AddListener(input => nameJoinRoom = input);

            btnCreateRoom.onClick.AddListener(BtnCreateRoom);
            btnJoinRoom.onClick.AddListener(BtnJoinRoom);
            btnJoinRandomRoom.onClick.AddListener(BtnJoinRandomRoom);

            btnStart.onClick.AddListener(() => photonView.RPC("RPCStartGame", RpcTarget.All));
        }
        #endregion

        #region �p�H��k
        [PunRPC]
        private void RPCStartGame()
        {
            PhotonNetwork.LoadLevel("�C������");
        }

        /// <summary>
        /// �[�J Photon ���A��
        /// </summary>
        private void JoinMaster()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// ���s�ƥ�G�إߩж�
        /// </summary>
        private void BtnCreateRoom()
        {
            RoomOptions ro = new RoomOptions();             // �ж���T
            ro.MaxPlayers = maxPlayer;                      // �̤j�H��
            PhotonNetwork.CreateRoom(nameCreateRoom, ro);   // PUN �إߩж�(�ж��W�١A�ж���T)
        }

        /// <summary>
        /// ���s�ƥ�G�[�J�ж�
        /// </summary>
        private void BtnJoinRoom()
        {
            PhotonNetwork.JoinRoom(nameJoinRoom);
        }

        /// <summary>
        /// ���s�ƥ�G�[�J�H���ж�
        /// </summary>
        private void BtnJoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        /// <summary>
        /// ��s���a�M��
        /// </summary>
        private void UpdatePlayerList()
        {
            textRoomPlayerList.text = "";

            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                if (i == 0) textRoomPlayerList.text = "�ХD�G" + players[i].NickName;
                else textRoomPlayerList.text += "���a�G" + players[i].NickName;

                textRoomPlayerList.text += "\n";
            }
        } 
        #endregion

        #region Photon �ƥ�
        /// <summary>
        /// �[�J���A�����\�|���檺��k
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            groupLobbyMain.interactable = true;
            print("<color=yellow>�[�J���A�����\�I</color>");
        }

        /// <summary>
        /// �Ыةж����\�|���檺��k
        /// </summary>
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print($"<color=orange>���\�Ыةж��A�ж��W�١G{ nameCreateRoom } </color>");

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;
            btnStart.interactable = true;

            textRoom.text = "�ж��W�١G" + nameCreateRoom;
            textRoomPlayerList.text = "";
            textRoomPlayerList.text = "�ХD�G" + namePlayer;
        }

        /// <summary>
        ///  �[�J�ж����\��|���檺��k
        /// </summary>
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoom.text = "�ж��W�١G" + PhotonNetwork.CurrentRoom.Name;

            UpdatePlayerList();
        }

        /// <summary>
        /// ���s���a�[�J�ж��ɷ|����@������k
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
