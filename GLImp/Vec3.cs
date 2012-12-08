using OpenTK;
using System;

public class Vec3 {
	public double x;
	public double y;
	public double z;
	
	public Vec3(Vector3d v){
		x = v.X;
		y = v.Y;
		z = v.Z;
	}
	
	public Vec3(double x, double y, double z) {
		this.x = x;
		this.y = y;
		this.z = z;

	}

	public Vec3(Vec3 v) {
		x = v.x;
		y = v.y;
		z = v.z;
	}


	public Vec3() {
		x = 0;
		y = 0;
		z = 0;

	}

	public Vec3 add(Vec3 v) {
		Vec3 res = new Vec3();

		res.x = x + v.x;
		res.y = y + v.y;
		res.z = z + v.z;

		return(res);
	}
	
	public Vec3 sub(Vec3 v) {
		Vec3 res = new Vec3();

		res.x = x - v.x;
		res.y = y - v.y;
		res.z = z - v.z;

		return(res);
	}


	public Vec3 negate()
	{
		Vec3 res = new Vec3();

		res.x = -x;
		res.y = -y;
		res.z = -z;

		return(res);
	}

	// cross product
	public Vec3 cross(Vec3 v)
	{
		Vec3 res = new Vec3();

		res.x = y * v.z - z * v.y;
		res.y = z * v.x - x * v.z;
		res.z = x * v.y - y * v.x;

		return (res);
	}

	public Vec3 scale(double t) {
		Vec3 res = new Vec3();

		res.x = x * t;
		res.y = y * t;
		res.z = z * t;

		return (res);
	}


	public Vec3 divide(double t)
	{

		Vec3 res = new Vec3();

		res.x = x / t;
		res.y = y / t;
		res.z = z / t;

		return (res);
	}



	public double length()
	{
		return((double)Math.Sqrt(x*x + y*y + z*z));
	}

	public void normalize()
	{
		double len;

		len = length();
		if (len != 0) {
			x /= len;
			y /= len;
			z /= len;
		}
	}


	//AKA dot product.
	public double innerProduct(Vec3 v)
	{
		return (x * v.x + y * v.y + z * v.z);
	}

	public void copy(Vec3 v)
	{
		x = v.x;
		y = v.y;
		z = v.z;
	}

	public Vec3 copy() {
		Vec3 ret = new Vec3();
		ret.x = x;
		ret.y = y;
		ret.z = z;
		return ret;
	}

	public Vector3d getVector3() {
		return new Vector3d(x, y, z);
	}

	public void set(double x, double y, double z)
	{

		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Vec3 scalarMult(double a)
	{

		Vec3 res = new Vec3();

		res.x = x * a;
		res.y = y * a;
		res.z = z * a;

		return res;
	}


	public void print()
	{
		Console.WriteLine("Vec3("+x+", "+y+", "+z+")");
	}
}