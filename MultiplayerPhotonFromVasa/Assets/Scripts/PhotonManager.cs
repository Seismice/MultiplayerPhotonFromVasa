using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string region;
    [SerializeField] private string nickName;
    [SerializeField] private TMP_InputField RoomName;
    [SerializeField] private ListItem itemPrefab;
    [SerializeField] private Transform content;

    private GameObject player;
    [SerializeField] private GameObject player_pref;

    List<RoomInfo> allRoomsInfo = new List<RoomInfo>();
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);

        if (SceneManager.GetActiveScene().name == "game_scene")
        {
            player = PhotonNetwork.Instantiate(player_pref.name, Vector3.zero, Quaternion.identity);
            player.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Ви підключилися до: " + PhotonNetwork.CloudRegion);

        if (nickName == "")
        {
            PhotonNetwork.NickName = "User";
        }
        else
        {
            PhotonNetwork.NickName = nickName;
        }

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Ви відключені від сервера!");
    }

    public void CreateRoomButton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Кімната створена: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Кімната не створена!");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            for (int i = 0; i < allRoomsInfo.Count; i++)
            {
                if (allRoomsInfo[i].masterClientId == info.masterClientId)
                    return;
            }

            ListItem listItem = Instantiate(itemPrefab, content);

            if (listItem != null)
            {
                listItem.SetInfo(info);
                allRoomsInfo.Add(info);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("game_scene");
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(RoomName.text);
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(player.gameObject);
        PhotonNetwork.LoadLevel("Main");
    }
}
