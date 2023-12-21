using System;

namespace func_rocket;

public class ControlTask
{
    public static double angle = 0.0;
    public static Turn ControlRocket(Rocket rocket, Vector target)
    {

        var distance = target - rocket.Location;
        if ((Math.Abs(distance.Angle - rocket.Velocity.Angle) < 0.5) || (Math.Abs(distance.Angle - rocket.Direction) < 0.5))
            angle = (distance.Angle * 2 - rocket.Velocity.Angle - rocket.Direction) / 2;
        else angle = distance.Angle - rocket.Direction;

        if (angle > 0)
            return Turn.Right;
        else if (angle < 0)
            return Turn.Left;
        return Turn.None;
    }
}