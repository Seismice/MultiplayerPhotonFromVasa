using UnityEngine;
using Photon.Pun;
using System;

public class Player : MonoBehaviour
{
    private Animator anim;
    private PhotonView View;

    [Header("Ўвидк≥сть перем≥щенн€ персонажа")]
    public float spead = 7f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        View = GetComponent<PhotonView>();
    }

    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.UpArrow) && View.IsMine)
        {
            
            transform.localPosition += transform.up * spead * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow) && View.IsMine)
        {
            
            transform.localPosition += -transform.up * spead * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow) && View.IsMine)
        {
            transform.localPosition += -transform.right * spead * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow) && View.IsMine)
        {
            transform.localPosition += transform.right * spead * Time.deltaTime;
        }

    }
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt) && View.IsMine)
        {
            //anim.SetTrigger("ChangeColor");
            SendAnimationToBlue();
        }

        if (Input.GetKeyDown(KeyCode.RightAlt) && View.IsMine)
        {
            //anim.SetTrigger("ChangeColor");
            SendAnimationToWight();
        }

    }

    private void SendAnimationToBlue()
    {
        View.RPC("Send_AnimationToBlue", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Send_AnimationToBlue()
    {
        anim.SetTrigger("ChangeColor");
    }

    private void SendAnimationToWight()
    {
        View.RPC("Send_AnimationToWight", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Send_AnimationToWight()
    {
        anim.SetTrigger("ToWight");
    }
}
