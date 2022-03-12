using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private CollactableType collactableType = CollactableType.None;

    private void Update()
    {
        transform.Translate(Vector3.down*speed*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            switch (collactableType)
            {
                case CollactableType.Health:
                    other.GetComponent<PlayerController>().GotHealth();
                    gameObject.SetActive(false);
                    break;
                case CollactableType.PowerUp:
                    other.GetComponent<PlayerController>().GotPowerUp();
                    gameObject.SetActive(false);
                    break;
                case CollactableType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

public enum CollactableType
{
    Health,
    PowerUp,
    None
}
