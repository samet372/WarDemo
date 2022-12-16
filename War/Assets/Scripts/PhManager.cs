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
        PhotonNetwork.JoinLobby(); // server a girildi �imdi lobiye gir
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("lobiye girdi");
        PhotonNetwork.JoinOrCreateRoom("oda", new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true }, TypedLobby.Default); // lobiye de girdin �imdi odaya gir
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("odaya girildi");

        GameObject karakter = PhotonNetwork.Instantiate("Karakter", new Vector3(karakterRespawnX, 44f, karakterRespawnZ), Quaternion.identity, 0, null);
        //karakter.GetComponent<PhotonView>().Owner.NickName = "Fatih"; join lobby sahnesi yap�ld���nda input text e girilen de�eri fatih k�sm�na yaz ve ki�inin girdi�i isim orda yazs�n sahne i�inde de karakterin �st�ne text koyup ismini yazd�r.
    }

}
