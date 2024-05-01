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

    // Reference to the health controller
    public HealthController healthController;

    private void Start()
    {
        // Initialize the HealthController component
        healthController = GetComponent<HealthController>();

        // Check if the HealthController component is attached
        if (healthController == null)
        {
            Debug.LogError("HealthController reference is not set!");
        }

        rb = GetComponent<Rigidbody>();
        controller = gameObject.AddComponent<CharacterController>();
        power = "None"; // Initial power setting
        SetPowerText(); // Update power display
        item = "None";  // Initial item setting
        SetItemText();  // Update item display
    }

    void Update()
    {
        // Determine the number of jumps available based on power status
        if (power == "Double Jump" && groundedPlayer)
        {
            jumps = 2;
        }
        else if (groundedPlayer)
        {
            jumps = 1;
        }

        // Update grounding status and reset vertical velocity if grounded
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Handle horizontal movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Align player to the moving direction
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Handle jumping
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

        // Apply gravity to player velocity and move the player
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Collect power-ups and items, and deactivate them
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

        // Take damage if collides with a hurtful object
        if (other.gameObject.CompareTag("hurt"))
        {
            healthController.TakeDamage(10.0f); // Specify damage amount
        }
    }
    // Update the power text UI

    void SetPowerText()
    {
        powerText.text = "Power: " + power;
    }

    // Update the item text UI
    void SetItemText()
    {
        itemText.text = "Item: " + item;
    }
}
