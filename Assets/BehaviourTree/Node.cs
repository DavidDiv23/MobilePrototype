using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public enum Status { SUCCESS, RUNNING, FAILURE };
    public Status status;
    public List<Node> children = new List<Node>();
    public int currentChild = 0;
    public string name;
    public int sortOrder;

    public Node() { }

    public Node(string n)
    {
        name = n;
    }

    public Node(string n, int order)
    {
        name = n;
        sortOrder = order;
    }

    public void Reset()
    {
        foreach (Node n in children)
            n.Reset();
        currentChild = 0;
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public void AddChild(Node n)
    {
        children.Add(n);
    }
}
