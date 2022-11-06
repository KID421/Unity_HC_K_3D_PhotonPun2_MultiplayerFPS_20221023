using UnityEngine;
using Photon.Pun;

namespace KID
{
    /// <summary>
    /// 玩家生成控制器
    /// </summary>
    public class SpawnPlayer : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("玩家預製物")]
        private GameObject prefabPlayer;
        [SerializeField, Header("玩家生成點")]
        private Transform[] spawnPoints;

        private void Awake()
        {
            Spawn();
        }

        /// <summary>
        /// 生成
        /// </summary>
        private void Spawn()
        {
            int random = Random.Range(0, spawnPoints.Length);
            Vector3 pos = spawnPoints[random].position;

            // Instantiate(); // 非連線遊戲的生成
            // 伺服器.生成(物件名稱，座標，角度)
            PhotonNetwork.Instantiate(prefabPlayer.name, pos, Quaternion.identity);
        }
    }
}
