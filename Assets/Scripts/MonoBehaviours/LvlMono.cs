using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarGame
{
    public class LvlMono : MonoBehaviourPunCallbacks
    {
        #region photon methods

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}
