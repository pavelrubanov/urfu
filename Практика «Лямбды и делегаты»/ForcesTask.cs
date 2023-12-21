using System;

namespace func_rocket;

public class ForcesTask
{
	/// <summary>
	/// Создает делегат, возвращающий по ракете вектор силы тяги двигателей этой ракеты.
	/// Сила тяги направлена вдоль ракеты и равна по модулю forceValue.
	/// </summary>
	public static RocketForce GetThrustForce(double forceValue) =>
    r =>
    {
        var vector = new Vector(forceValue, 0);
        var x = Math.Cos(r.Direction) * vector.X - Math.Sin(r.Direction) * vector.Y;
        var y = Math.Sin(r.Direction) * vector.X + Math.Cos(r.Direction) * vector.Y;
        return new Vector(x, y);
    };
		
    /// <summary>
    /// Преобразует делегат силы гравитации, в делегат силы, действующей на ракету
    /// </summary>
    public static RocketForce ConvertGravityToForce(Gravity gravity, Vector spaceSize) => 
		r => gravity(spaceSize, r.Location);

	/// <summary>
	/// Суммирует все переданные силы, действующие на ракету, и возвращает суммарную силу.
	/// </summary>
	public static RocketForce Sum(params RocketForce[] forces) => r => 
			{
				var result_vector = new Vector(0, 0);
				var vector = new Vector(0, 0);
				foreach(var e in forces)
				{
                    result_vector += e(new Rocket(vector, vector, 0));
                }
				return result_vector;
			};
}