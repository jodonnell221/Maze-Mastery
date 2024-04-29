using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody rb; 
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private int jumps;

    private string power;
    private string item;

    public TextMeshProUGUI powerText;
    public TextMeshProUGUI itemText;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = gameObject.AddComponent<CharacterController>();
        power = "None";
        SetPowerText();
        item = "None";
        SetItemText();
    }

    void Update()
    {
        if (power == "Double Jump" && groundedPlayer)
        {
            jumps = 2;
        }
        else if (groundedPlayer)
        {
            jumps = 1;
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (!groundedPlayer)
            {
                jumps -= 1;
            }

            if (jumps > 0)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                jumps -= 1;
            }
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            other.gameObject.SetActive(false);
            power = other.gameObject.name;
            SetPowerText();
        }

        if (other.gameObject.CompareTag("Item"))
        {
            other.gameObject.SetActive(false);
            item = other.gameObject.name;
            SetItemText();
        }
    }

    void SetPowerText()
    {
        powerText.text = "Power: " + power;
    }

    void SetItemText()
    {
        itemText.text = "Item: " + item;
    }
}


