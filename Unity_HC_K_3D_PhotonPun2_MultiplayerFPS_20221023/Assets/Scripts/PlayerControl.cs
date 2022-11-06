using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using StarterAssets;

namespace KID
{
    /// <summary>
    /// 玩家控制器
    /// 啟動自己的控制器與攝影機
    /// </summary>
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        #region 序列化資料
        [SerializeField, Header("攝影機")]
        private GameObject goCamera;
        [SerializeField, Header("玩家輸入控制")]
        private PlayerInput playerInput;
        [SerializeField, Header("子彈預製物")]
        private GameObject prefabBullet;
        [SerializeField, Header("子彈速度"), Range(0, 5000)]
        private float speedBullet = 1500;
        [Header("介面")]
        [SerializeField] private GameObject goCanvasMain;
        [SerializeField] private Image imgHealth;
        [SerializeField] private TextMeshProUGUI textHealth;
        [SerializeField] private GameObject goFinal;
        [SerializeField] private Button btnReturnToLobby;
        #endregion

        #region 私人欄位
        private Animator ani;
        private CharacterController cc;
        private string parWalk = "開關走路";
        private string parAttack = "開關攻擊";

        private float timeAttackAnimation = 1.2f;
        private float timeSpawnBullet = 0.4f; 
        
        private float hp = 100;
        private float hpMax;
        private float damage = 10;
        #endregion

        private StarterAssetsInputs sai;

        private void Awake()
        {
            ani = GetComponent<Animator>();
            cc = GetComponent<CharacterController>();
            sai = GetComponent<StarterAssetsInputs>();

            hpMax = hp;

            if (photonView.IsMine)              // 如果 是自己的物件
            {
                goCamera.SetActive(true);       // 開啟攝影機
                playerInput.enabled = true;     // 開啟輸入控制
                goCanvasMain.SetActive(true);
                ChangeChildrenLayer();          // 變更子物件圖層
            }

            btnReturnToLobby.onClick.AddListener(() =>
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("遊戲大廳");
                }
            });
        }

        private void Update()
        {
            Attack();
            MoveAnimator();
        }

        private void OnParticleCollision(GameObject other)
        {
            hp -= damage;

            imgHealth.fillAmount = hp / hpMax;
            textHealth.text = hp + " / " + hpMax;
        }

        /// <summary>
        /// 受傷
        /// </summary>
        public void Damage()
        {
            hp -= damage;

            if (hp <= 0) Dead();

            imgHealth.fillAmount = hp / hpMax;
            textHealth.text = hp + " / " + hpMax;
        }

        /// <summary>
        /// 死亡
        /// </summary>
        private void Dead()
        {
            hp = 0;
            cc.enabled = false;
            goFinal.SetActive(true);
            transform.position = Vector3.one * 100;
            sai.cursorLocked = false;
            Cursor.lockState = CursorLockMode.Confined;
            enabled = false;
        }

        /// <summary>
        /// 移動動畫
        /// </summary>
        private void MoveAnimator()
        {
            bool isWalk = cc.velocity.magnitude > 0;
            ani.SetBool(parWalk, isWalk);
        }

        /// <summary>
        /// 變更子物件圖層
        /// </summary>
        private void ChangeChildrenLayer()
        {
            Transform[] children = GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                children[i].gameObject.layer = 6;
            }
        }

        /// <summary>
        /// 攻擊
        /// </summary>
        private void Attack()
        {
            if (!photonView.IsMine) return;             // 不是自己的物件不用更新攻擊動畫
            if (ani.GetBool(parAttack)) return;         // 如果 正在攻擊中 就跳出

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ani.SetBool(parAttack, true);
                Invoke("AttackEnd", timeAttackAnimation);
                Invoke("SpawnBullet", timeSpawnBullet);
            }
        }

        /// <summary>
        /// 攻擊結束
        /// </summary>
        private void AttackEnd()
        {
            ani.SetBool(parAttack, false);
        }

        /// <summary>
        /// 生成子彈
        /// </summary>
        private void SpawnBullet()
        {
            GameObject tempBullet = PhotonNetwork.Instantiate(
                prefabBullet.name,
                transform.position + transform.forward * 1.5f + transform.up * 1.5f,
                Quaternion.identity);

            tempBullet.GetComponent<Rigidbody>().AddForce(transform.forward * speedBullet);
        }
    }
}
