using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDropOff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            GameController.instance.BankPoints();

            Destroy(gameObject);
        }
    }
}
