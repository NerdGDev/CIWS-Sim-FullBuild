using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Systembase : MonoBehaviour
{
    public delegate void OrderDelegate(MonoBehaviour go);
    protected Queue<OrderDelegate> OrderQueue = new Queue<OrderDelegate>();

    protected void AddOrder(OrderDelegate order)
    {
        OrderQueue.Enqueue(order);
        if (OrderQueue.Count == 1)
        {
            OrderQueue.Peek().Invoke(this);
        }
    }

    protected void NextOrder()
    {
        OrderQueue.Dequeue();
        if (OrderQueue.Count > 0)
        {
            OrderQueue.Peek().Invoke(this);
        }
    }
}
