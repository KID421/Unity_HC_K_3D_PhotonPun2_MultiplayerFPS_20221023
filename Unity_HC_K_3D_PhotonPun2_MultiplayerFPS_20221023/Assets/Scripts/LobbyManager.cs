using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KID
{
    /// <summary>
    /// �j�U�޲z���G���a�W�١B�ЫءB�[�J�ж�
    /// </summary>
    public class LobbyManager : MonoBehaviour
    {
        private TMP_InputField inputFieldPlayerName;
        private TMP_InputField inputFieldCreateRoomName;
        private TMP_InputField inputFieldJoinRoomName;
        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Button btnJoinRandomRoom;

        private void Awake()
        {
            inputFieldPlayerName = GameObject.Find("��J��쪱�a�W��").GetComponent<TMP_InputField>();
            inputFieldCreateRoomName = GameObject.Find("��J���Ыةж��W��").GetComponent<TMP_InputField>();
            inputFieldJoinRoomName = GameObject.Find("��J���[�J�ж��W��").GetComponent<TMP_InputField>();
            btnCreateRoom = GameObject.Find("���s�Ыةж�").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("���s�[�J�ж�").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("���s�[�J�H���ж�").GetComponent<Button>();
        }
    }
}
