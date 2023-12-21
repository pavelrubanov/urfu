using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace func_rocket;

public class LevelsTask
{
	static readonly Physics standardPhysics = new();

    public static Vector BlackHole(Vector v, Vector target, Vector rocketLocation)
    {
        var d = ((target + rocketLocation) / 2 - v).Length;
        return ((target + rocketLocation) / 2 - v).Normalize() * 300 * d / (d * d + 1);
    }
    public static Vector WhiteHole(Vector v, Vector target)
    {
        var d = (v - target).Length;
        return (v - target).Normalize() * 140 * d / (d * d + 1);
    }
    public static IEnumerable<Level> CreateLevels()
	{


		var target = new Vector(600, 200);
		var rocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);

        yield return new Level("Zero", 
			rocket,
            target, 
			(size, v) => Vector.Zero, 
			standardPhysics);

        yield return new Level("Heavy",
            rocket,
            target,
            (size, v) => new Vector(0, 0.9), 
			standardPhysics);	

        yield return new Level("Up",
			rocket,
            new Vector(700, 500),
            (size, v) => new Vector(0, -300 / ((size.Y - v.Y) + 300)),
			standardPhysics);

        yield return new Level("WhiteHole",
            rocket,
            target,
            (size, v) =>
                WhiteHole(v, target),
            standardPhysics);

        yield return new Level("BlackHole",
            rocket,
            target,
            (size, v) => BlackHole(v, target, rocket.Location), 
            standardPhysics);

        yield return new Level("BlackAndWhite",
            rocket,
            target,
            (size, v) =>
                (WhiteHole(v, target) + BlackHole(v, target, rocket.Location)) / 2,
            standardPhysics);
    } 
}