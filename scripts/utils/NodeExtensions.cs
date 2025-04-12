using Godot;
using System;
public static class NodeExtensions
{
    public static T FindFirstParent<T>(this Node node) where T : Node
    {
        Node current = node.GetParent();
        while (current != null)
        {
            if (current is T match)
                return match;
            current = current.GetParent();
        }
        return null;
    }

    public static Node FindFirstChildOfName(Node parent, string name)
    {
        foreach (Node child in parent.GetChildren())
        {
            if (child.Name == name)
                return child;

            
        }

        foreach (Node child in parent.GetChildren()) {
            var found = FindFirstChildOfName(child, name);
            if (found != null)
                return found;
        }
        return null;
    }
}