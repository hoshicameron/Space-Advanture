using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Meteor") || other.gameObject.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
