using OpenTK;
using System;

public class Plane {
	public Vec3 normal,point;
	float d;

	public Plane(){
	}

	public Plane( Vec3 v1,  Vec3 v2,  Vec3 v3) {
		set3Points(v1,v2,v3);
	}


	public void set3Points( Vec3 v1,  Vec3 v2, Vec3 v3) {
		Vec3 aux1, aux2;

		aux1 = v1.sub(v2);
		aux2 = v3.sub(v2);
		
		normal = aux2.cross(aux1);
		normal.normalize();
		
		point = v2;
		
		d = -normal.innerProduct(point);
	}

	public void setNormalAndPoint(Vec3 normal, Vec3 point)
	{

		this.normal = normal;
		this.normal.normalize();
		d = -(this.normal.innerProduct(point));
	}

	public void setCoefficients(float a, float b, float c, float d)
	{
		normal.set(a,b,c);
		float l = normal.length();
		normal.set(a/l,b/l,c/l);
		this.d = d/l;
	}

	public float distance(Vec3 p)
	{

		return (d + normal.innerProduct(p));
	}

	public void print()
	{
		Console.WriteLine("Plane: Normal: " + normal.ToString() + " D: " + d);
	}

}