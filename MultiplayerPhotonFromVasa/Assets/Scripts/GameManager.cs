using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text textLastMassage;
    [SerializeField] private TMP_InputField textMassageField;

    private PhotonView PhotonView;
    private void Start()
    {
        PhotonView = GetComponent<PhotonView>();
    }
    public void SendButton()
    {
        PhotonView.RPC("Send_Data", RpcTarget.AllBuffered, PhotonNetwork.NickName, textMassageField.text);
    }

    [PunRPC]
    private void Send_Data(string nick, string massage)
    {
        textLastMassage.text = nick + ": "+ massage;
    }
}
