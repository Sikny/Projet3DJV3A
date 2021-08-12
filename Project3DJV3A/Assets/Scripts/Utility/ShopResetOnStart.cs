using System.Collections;
using System.Collections.Generic;
using Game;
using Items;
using UnityEngine;
using Utility;

public class ShopResetOnStart : MonoBehaviour
{

  
    // Start is called before the first frame update
    void Start()
    {
        Shop.Instance.ClearShop();
    }


}
