using System;
using UnityEngine;

/// <summary>
/// An alternative to <see cref="Vector2"/> that uses decimals to minimize the rounding errors of floats.
/// </summary>
/// <remarks>Derived from Monogame's Vector2 <see cref="https://github.com/ManojLakshan/monogame/blob/master/MonoGame.Framework/Vector2.cs"/></remarks>
public struct Vector2Decimal
{
	#region Private Fields

	private static Vector2Decimal zeroVector = new Vector2Decimal(0m, 0m);
	private static Vector2Decimal unitVector = new Vector2Decimal(1m, 1m);
	private static Vector2Decimal unitXVector = new Vector2Decimal(1m, 0m);
	private static Vector2Decimal unitYVector = new Vector2Decimal(0m, 1m);

	#endregion Private Fields


	#region Public Fields

	public decimal X;
	public decimal Y;

	#endregion Public Fields


	#region Properties

	public static Vector2Decimal Zero
	{
		get { return zeroVector; }
	}

	public static Vector2Decimal One
	{
		get { return unitVector; }
	}

	public static Vector2Decimal UnitX
	{
		get { return unitXVector; }
	}

	public static Vector2Decimal UnitY
	{
		get { return unitYVector; }
	}

	#endregion Properties


	#region Constructors

	public Vector2Decimal(Vector2 v)
	{
		this.X = (decimal)v.x;
		this.Y = (decimal)v.y;
	}

	public Vector2Decimal(Vector3 v)
	{
		this.X = (decimal)v.x;
		this.Y = (decimal)v.y;
	}

	public Vector2Decimal(float x, float y)
	{
		this.X = (decimal)x;
		this.Y = (decimal)y;
	}

	//public Vector2Decimal(PointF value)
	//{
	//	this.X = value.X;
	//	this.Y = value.Y;
	//}

	public Vector2Decimal(float value)
	{
		this.X = (decimal)value;
		this.Y = (decimal)value;
	}

	public Vector2Decimal(decimal  x, decimal y)
	{
		this.X = x;
		this.Y = y;
	}

	public Vector2Decimal(decimal value)
	{
		this.X = value;
		this.Y = value;
	}
	#endregion Constructors


	#region Public Methods

	public static Vector2Decimal Add(Vector2Decimal value1, Vector2Decimal value2)
	{
		value1.X += value2.X;
		value1.Y += value2.Y;
		return value1;
	}

	public static void Add(ref Vector2Decimal value1, ref Vector2Decimal value2, out Vector2Decimal result)
	{
		result.X = value1.X + value2.X;
		result.Y = value1.Y + value2.Y;
	}

	//public static Vector2Decimal Barycentric(Vector2Decimal value1, Vector2Decimal value2, Vector2Decimal value3, float amount1, float amount2)
	//{
	//	return new Vector2Decimal(
	//		MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
	//		MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
	//}

	//public static void Barycentric(ref Vector2Decimal value1, ref Vector2Decimal value2, ref Vector2Decimal value3, float amount1, float amount2, out Vector2Decimal result)
	//{
	//	result = new Vector2Decimal(
	//		MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
	//		MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
	//}

	//public static Vector2Decimal CatmullRom(Vector2Decimal value1, Vector2Decimal value2, Vector2Decimal value3, Vector2Decimal value4, float amount)
	//{
	//	return new Vector2Decimal(
	//		MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
	//		MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
	//}

	//public static void CatmullRom(ref Vector2Decimal value1, ref Vector2Decimal value2, ref Vector2Decimal value3, ref Vector2Decimal value4, float amount, out Vector2Decimal result)
	//{
	//	result = new Vector2Decimal(
	//		MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
	//		MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
	//}
	static decimal Clamp(decimal min, decimal max, decimal val)
	{
		if (val < min)
			return min;
		if (val > max)
			return max;
		return val;
	}
	public static Vector2Decimal Clamp(Vector2Decimal value1, Vector2Decimal min, Vector2Decimal max)
	{

		return new Vector2Decimal(
			Clamp(value1.X, min.X, max.X),
			Clamp(value1.Y, min.Y, max.Y));
	}

	public static void Clamp(ref Vector2Decimal value1, ref Vector2Decimal min, ref Vector2Decimal max, out Vector2Decimal result)
	{
		result = new Vector2Decimal(
			Clamp(value1.X, min.X, max.X),
			Clamp(value1.Y, min.Y, max.Y));
	}

	public static decimal Distance(Vector2Decimal value1, Vector2Decimal value2)
	{
		decimal v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
		return (decimal)Math.Sqrt((double)((v1 * v1) + (v2 * v2)));
	}

	public static void Distance(ref Vector2Decimal value1, ref Vector2Decimal value2, out decimal result)
	{
		decimal v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
		result = (decimal)Math.Sqrt((double)((v1 * v1) + (v2 * v2)));
	}

	public static decimal DistanceSquared(Vector2Decimal value1, Vector2Decimal value2)
	{
		decimal v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
		return (v1 * v1) + (v2 * v2);
	}

	public static void DistanceSquared(ref Vector2Decimal value1, ref Vector2Decimal value2, out decimal result)
	{
		decimal v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
		result = (v1 * v1) + (v2 * v2);
	}

	public static Vector2Decimal Divide(Vector2Decimal value1, Vector2Decimal value2)
	{
		value1.X /= value2.X;
		value1.Y /= value2.Y;
		return value1;
	}

	public static void Divide(ref Vector2Decimal value1, ref Vector2Decimal value2, out Vector2Decimal result)
	{
		result.X = value1.X / value2.X;
		result.Y = value1.Y / value2.Y;
	}

	public static Vector2Decimal Divide(Vector2Decimal value1, decimal divider)
	{
		decimal factor = 1 / divider;
		value1.X *= factor;
		value1.Y *= factor;
		return value1;
	}

	public static void Divide(ref Vector2Decimal value1, decimal divider, out Vector2Decimal result)
	{
		decimal factor = 1 / divider;
		result.X = value1.X * factor;
		result.Y = value1.Y * factor;
	}

	public static decimal Dot(Vector2Decimal value1, Vector2Decimal value2)
	{
		return (value1.X * value2.X) + (value1.Y * value2.Y);
	}

	public static void Dot(ref Vector2Decimal value1, ref Vector2Decimal value2, out decimal result)
	{
		result = (value1.X * value2.X) + (value1.Y * value2.Y);
	}

	public override bool Equals(object obj)
	{
		if (obj is Vector2Decimal)
		{
			return Equals((Vector2Decimal)this);
		}

		return false;
	}

	public bool Equals(Vector2Decimal other)
	{
		return (X == other.X) && (Y == other.Y);
	}

	public static Vector2Decimal Reflect(Vector2Decimal vector, Vector2Decimal normal)
	{
		Vector2Decimal result = Vector2Decimal.Zero;
		decimal val = 2.0m * ((vector.X * normal.X) + (vector.Y * normal.Y));
		result.X = vector.X - (normal.X * val);
		result.Y = vector.Y - (normal.Y * val);
		return result;
	}

	public static void Reflect(ref Vector2Decimal vector, ref Vector2Decimal normal, out Vector2Decimal result)
	{
		decimal val = 2.0m * ((vector.X * normal.X) + (vector.Y * normal.Y));
		result = new Vector2Decimal(vector.X - (normal.X * val), vector.Y - (normal.Y * val));
	}

	public override int GetHashCode()
	{
		return X.GetHashCode() + Y.GetHashCode();
	}

	//public static Vector2Decimal Hermite(Vector2Decimal value1, Vector2Decimal tangent1, Vector2Decimal value2, Vector2Decimal tangent2, decimal amount)
	//{
	//	Vector2Decimal result = new Vector2Decimal();
	//	Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
	//	return result;
	//}

	//public static void Hermite(ref Vector2Decimal value1, ref Vector2Decimal tangent1, ref Vector2Decimal value2, ref Vector2Decimal tangent2, float amount, out Vector2Decimal result)
	//{
	//	result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
	//	result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
	//}

	public decimal Length()
	{
		return (decimal)Math.Sqrt((double)((X * X) + (Y * Y)));
	}

	public decimal LengthSquared()
	{
		return (X * X) + (Y * Y);
	}

	//public static Vector2Decimal Lerp(Vector2Decimal value1, Vector2Decimal value2, decimal amount)
	//{
	//	return new Vector2Decimal(
	//		MathHelper.Lerp(value1.X, value2.X, amount),
	//		MathHelper.Lerp(value1.Y, value2.Y, amount));
	//}

	//public static void Lerp(ref Vector2Decimal value1, ref Vector2Decimal value2, float amount, out Vector2Decimal result)
	//{
	//	result = new Vector2Decimal(
	//		MathHelper.Lerp(value1.X, value2.X, amount),
	//		MathHelper.Lerp(value1.Y, value2.Y, amount));
	//}

	public static Vector2Decimal Max(Vector2Decimal value1, Vector2Decimal value2)
	{
		return new Vector2Decimal(value1.X > value2.X ? value1.X : value2.X,
						   value1.Y > value2.Y ? value1.Y : value2.Y);
	}

	public static void Max(ref Vector2Decimal value1, ref Vector2Decimal value2, out Vector2Decimal result)
	{
		result.X = value1.X > value2.X ? value1.X : value2.X;
		result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
	}

	public static Vector2Decimal Min(Vector2Decimal value1, Vector2Decimal value2)
	{
		return new Vector2Decimal(value1.X < value2.X ? value1.X : value2.X,
						   value1.Y < value2.Y ? value1.Y : value2.Y);
	}

	public static void Min(ref Vector2Decimal value1, ref Vector2Decimal value2, out Vector2Decimal result)
	{
		result.X = value1.X < value2.X ? value1.X : value2.X;
		result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
	}

	public static Vector2Decimal Multiply(Vector2Decimal value1, Vector2Decimal value2)
	{
		value1.X *= value2.X;
		value1.Y *= value2.Y;
		return value1;
	}

	public static Vector2Decimal Multiply(Vector2Decimal value1, decimal scaleFactor)
	{
		value1.X *= scaleFactor;
		value1.Y *= scaleFactor;
		return value1;
	}

	public static void Multiply(ref Vector2Decimal value1, decimal scaleFactor, out Vector2Decimal result)
	{
		result.X = value1.X * scaleFactor;
		result.Y = value1.Y * scaleFactor;
	}

	public static void Multiply(ref Vector2Decimal value1, ref Vector2Decimal value2, out Vector2Decimal result)
	{
		result.X = value1.X * value2.X;
		result.Y = value1.Y * value2.Y;
	}

	public static Vector2Decimal Negate(Vector2Decimal value)
	{
		value.X = -value.X;
		value.Y = -value.Y;
		return value;
	}

	public static void Negate(ref Vector2Decimal value, out Vector2Decimal result)
	{
		result.X = -value.X;
		result.Y = -value.Y;
	}

	public void Normalize()
	{
		decimal val = 1.0m / (decimal)Math.Sqrt((double)((X * X) + (Y * Y)));
		X *= val;
		Y *= val;
	}

	public static Vector2Decimal Normalize(Vector2Decimal value)
	{
		decimal val = 1.0m / (decimal)Math.Sqrt((double)((value.X * value.X) + (value.Y * value.Y)));
		value.X *= val;
		value.Y *= val;
		return value;
	}

	public static void Normalize(ref Vector2Decimal value, out Vector2Decimal result)
	{
		decimal val = 1.0m / (decimal)Math.Sqrt((double)((value.X * value.X) + (value.Y * value.Y)));
		result.X = value.X * val;
		result.Y = value.Y * val;
	}

	//public static Vector2Decimal SmoothStep(Vector2Decimal value1, Vector2Decimal value2, float amount)
	//{
	//	return new Vector2Decimal(
	//		MathHelper.SmoothStep(value1.X, value2.X, amount),
	//		MathHelper.SmoothStep(value1.Y, value2.Y, amount));
	//}

	//public static void SmoothStep(ref Vector2Decimal value1, ref Vector2Decimal value2, float amount, out Vector2Decimal result)
	//{
	//	result = new Vector2Decimal(
	//		MathHelper.SmoothStep(value1.X, value2.X, amount),
	//		MathHelper.SmoothStep(value1.Y, value2.Y, amount));
	//}

	public static Vector2Decimal Subtract(Vector2Decimal value1, Vector2Decimal value2)
	{
		value1.X -= value2.X;
		value1.Y -= value2.Y;
		return value1;
	}

	public static void Subtract(ref Vector2Decimal value1, ref Vector2Decimal value2, out Vector2Decimal result)
	{
		result.X = value1.X - value2.X;
		result.Y = value1.Y - value2.Y;
	}

	//public static Vector2Decimal Transform(Vector2Decimal position, Matrix matrix)
	//{
	//	Transform(ref position, ref matrix, out position);
	//	return position;
	//}

	//public static void Transform(ref Vector2Decimal position, ref Matrix matrix, out Vector2Decimal result)
	//{
	//	result = new Vector2Decimal((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M41,
	//						 (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M42);
	//}

	//public static void Transform(
	// Vector2Decimal[] sourceArray,
	// ref Matrix matrix,
	// Vector2Decimal[] destinationArray)
	//{
	//}


	//public static void Transform(
	// Vector2Decimal[] sourceArray,
	// int sourceIndex,
	// ref Matrix matrix,
	// Vector2Decimal[] destinationArray,
	// int destinationIndex,
	// int length)
	//{
	//}

	//public static Vector2Decimal TransformNormal(Vector2Decimal normal, Matrix matrix)
	//{
	//	Vector2Decimal.TransformNormal(ref normal, ref matrix, out normal);
	//	return normal;
	//}

	//public static void TransformNormal(ref Vector2Decimal normal, ref Matrix matrix, out Vector2Decimal result)
	//{
	//	result = new Vector2Decimal((normal.X * matrix.M11) + (normal.Y * matrix.M21),
	//						 (normal.X * matrix.M12) + (normal.Y * matrix.M22));
	//}

	public override string ToString()
	{
		//CultureInfo currentCulture = CultureInfo.CurrentCulture;
		//return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[] {
		//		this.X.ToString(currentCulture), this.Y.ToString(currentCulture) });
		return $"{{X:{X} Y:{Y}}}";
	}

	#endregion Public Methods


	#region Operators

	public static Vector2Decimal operator -(Vector2Decimal value)
	{
		value.X = -value.X;
		value.Y = -value.Y;
		return value;
	}


	public static bool operator ==(Vector2Decimal value1, Vector2Decimal value2)
	{
		return value1.X == value2.X && value1.Y == value2.Y;
	}


	public static bool operator !=(Vector2Decimal value1, Vector2Decimal value2)
	{
		return value1.X != value2.X || value1.Y != value2.Y;
	}


	public static Vector2Decimal operator +(Vector2Decimal value1, Vector2Decimal value2)
	{
		value1.X += value2.X;
		value1.Y += value2.Y;
		return value1;
	}


	public static Vector2Decimal operator -(Vector2Decimal value1, Vector2Decimal value2)
	{
		value1.X -= value2.X;
		value1.Y -= value2.Y;
		return value1;
	}


	public static Vector2Decimal operator *(Vector2Decimal value1, Vector2Decimal value2)
	{
		value1.X *= value2.X;
		value1.Y *= value2.Y;
		return value1;
	}


	public static Vector2Decimal operator *(Vector2Decimal value, decimal scaleFactor)
	{
		value.X *= scaleFactor;
		value.Y *= scaleFactor;
		return value;
	}


	public static Vector2Decimal operator *(decimal scaleFactor, Vector2Decimal value)
	{
		value.X *= scaleFactor;
		value.Y *= scaleFactor;
		return value;
	}


	public static Vector2Decimal operator /(Vector2Decimal value1, Vector2Decimal value2)
	{
		value1.X /= value2.X;
		value1.Y /= value2.Y;
		return value1;
	}


	public static Vector2Decimal operator /(Vector2Decimal value1, decimal divider)
	{
		decimal factor = 1 / divider;
		value1.X *= factor;
		value1.Y *= factor;
		return value1;
	}

	#endregion Operators

	#region Casts
	public static implicit operator Vector2(Vector2Decimal s)
	{
		return new Vector2((float)s.X, (float)s.Y);
	}
	public static implicit operator Vector3(Vector2Decimal s)
	{
		return new Vector3((float)s.X, (float)s.Y);
	}
	#endregion
}