using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main;
using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class FGUIUtil
{
    public static void SetWorldPos(GObject g, Vector3 pos)
    {
        g.position = g.parent.GlobalToLocal(pos);
    }
    public static Vector3 GetWorldPos(GObject g)
    {
        return g.LocalToGlobal(new Vector3());
    }

    public static void SetSamePos(GObject follower, GObject aim)
    {
        SetWorldPos(follower, GetWorldPos(aim));
    }

    public static T CreateWindow<T>(string name) where T : FairyWindow
    {
        GComponent gcom = UIPackage.CreateObject("Main", name).asCom;
        GRoot.inst.AddChild(gcom);
        gcom.MakeFullScreen();
        return (T)gcom;
    }

    /// <summary>
    /// Position/rotate/stretch a "Line" object so it spans from `from` to `to`.
    /// Assumes the Line component is authored as a horizontal bar with its
    /// pivot at the left-center (0, 0.5), so that setting width = length and
    /// rotation = angle stretches+rotates it to connect the two points.
    /// `from`/`to` should be in `line.parent`'s local coordinate space.
    /// </summary>
    public static void SetLine(GObject line, Vector2 from, Vector2 to)
    {
        Vector2 delta = to - from;
        line.SetXY(from.x, from.y);
        line.width = delta.magnitude;
        line.rotation = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Tests whether a global-space point falls inside an object's bounds.
    /// </summary>
    public static bool HitTest(GObject obj, Vector2 globalPos)
    {
        Vector2 local = obj.GlobalToLocal(globalPos);
        return local.x >= 0 && local.y >= 0 && local.x <= obj.width && local.y <= obj.height;
    }
}
