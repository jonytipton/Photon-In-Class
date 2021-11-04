using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
    #pragma warning disable 649

    public class NetworkedPlayer : MonoBehaviourPunCallbacks, IPunObservable
    {

        public GameObject avatar;

        public Transform playerGlobal;
        public Transform playerLocal;

        void Start()
        {
            Debug.Log("i'm instantiated");

            if (photonView.isMine)
            {
                Debug.Log("player is mine");

                playerGlobal = GameObject.Find("OVRPlayerController").transform;
                playerLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");

                this.transform.SetParent(playerLocal);
                this.transform.localPosition = Vector3.zero;

                // avatar.SetActive(false);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(playerGlobal.position);
                stream.SendNext(playerGlobal.rotation);
                stream.SendNext(playerLocal.localPosition);
                stream.SendNext(playerLocal.localRotation);
            }
            else
            {
                this.transform.position = (Vector3)stream.ReceiveNext();
                this.transform.rotation = (Quaternion)stream.ReceiveNext();
                avatar.transform.localPosition = (Vector3)stream.ReceiveNext();
                avatar.transform.localRotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}