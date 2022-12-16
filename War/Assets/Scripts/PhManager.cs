using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhManager : MonoBehaviourPunCallbacks
{
    int karakterRespawnX;
    int karakterRespawnZ;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

        karakterRespawnX = Random.Range(610, 620);
        karakterRespawnZ = Random.Range(360, 380);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("servere girdi");
        PhotonNetwork.JoinLobby(); // server a girildi þimdi lobiye gir
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("lobiye girdi");
        PhotonNetwork.JoinOrCreateRoom("oda", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default); // lobiye de girdin þimdi odaya gir
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("odaya girildi");

        GameObject karakter = PhotonNetwork.Instantiate("Karakter", new Vector3(karakterRespawnX, 44f, karakterRespawnZ), Quaternion.identity, 0, null);
        //karakter.GetComponent<PhotonView>().Owner.NickName = "Fatih"; join lobby sahnesi yapýldýðýnda input text e girilen deðeri fatih kýsmýna yaz ve kiþinin girdiði isim orda yazsýn sahne içinde de karakterin üstüne text koyup ismini yazdýr.
    }

}
