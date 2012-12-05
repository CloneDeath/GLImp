using OpenTK;
using System;
using OpenTK.Graphics.OpenGL;

public class Frustum {
	private int TOP = 0;
	private int BOTTOM = 1;
	private int LEFT = 2;
	private int RIGHT = 3;
	private int NEARP = 4;
	private int FARP = 5;

	public static int OUTSIDE = 0;
	public static int INTERSECT = 1;
	public static int INSIDE = 2;

	private Plane[] pl = new Plane[6];

	Vec3 ntl,ntr,nbl,nbr,ftl,ftr,fbl,fbr;
	float nearD, farD, ratio, angle,tang;
	float nw,nh,fw,fh;
	
	private double ANG2RAD = 3.14159265358979323846/180.0;

	public void setCamInternals(float angle, float ratio, float nearD, float farD) {
		this.ratio = ratio;
		this.angle = angle;
		this.nearD = nearD;
		this.farD = farD;

		tang = (float)Math.Tan(angle* ANG2RAD * 0.5) ;
		nh = nearD * tang;
		nw = nh * ratio; 
		fh = farD  * tang;
		fw = fh * ratio;
		
		for (int i = 0; i < 6; i++){
			pl[i] = new Plane();
		}
	}

	public void setCamDef(Vec3 p, Vec3 l, Vec3 u) {
		Vec3 nc,fc,X,Y,Z;

		Z = p.sub(l);
		Z.normalize();

		X = u.cross(Z);
		X.normalize();

		Y = Z.cross(X);

		nc = p.sub(Z.scale(nearD));
		fc = p.sub(Z.scale(farD));

		ntl = nc.add(Y.scale(nh)).sub(X.scale(nw));
		ntr = nc.add(Y.scale(nh)).add(X.scale(nw));
		nbl = nc.sub(Y.scale(nh)).sub(X.scale(nw));
		nbr = nc.sub(Y.scale(nh)).add(X.scale(nw));

		ftl = fc.add(Y.scale(fh)).sub(X.scale(fw));
		ftr = fc.add(Y.scale(fh)).add(X.scale(fw));
		fbl = fc.sub(Y.scale(fh)).sub(X.scale(fw));
		fbr = fc.sub(Y.scale(fh)).add(X.scale(fw));

		pl[TOP].set3Points(ntr,ntl,ftl);
		pl[BOTTOM].set3Points(nbl,nbr,fbr);
		pl[LEFT].set3Points(ntl,nbl,fbl);
		pl[RIGHT].set3Points(nbr,ntr,fbr);
		pl[NEARP].set3Points(ntl,ntr,nbr);
		pl[FARP].set3Points(ftr,ftl,fbl);
	}

	public int pointInFrustum(Vec3 p) {
		int result = INSIDE;
		for(int i=TOP; i <= FARP; i++) {

			if (pl[i].distance(p) < 0)
				return OUTSIDE;
		}
		return(result);
	}

	public int sphereInFrustum(Vec3 p, float raio)
	{
		int result = INSIDE;
		float distance;

		for(int i=0; i < 6; i++) {
			distance = pl[i].distance(p);
			if (distance < -raio)
				return OUTSIDE;
			else if (distance < raio)
				result =  INTERSECT;
		}
		return(result);
	}

	private Vec3 getVertexP(Vec3 normal, Vec3 pos) {
		Vec3 res = pos;

		if (normal.x > 0)
			res.x += 1.0f;

		if (normal.y > 0)
			res.y += 1.0f;

		if (normal.z > 0)
			res.z += 1.0f;

		return(res);
	}

	private Vec3 getVertexN(Vec3 normal, Vec3 pos) {
		Vec3 res = pos;

		if (normal.x < 0)
			res.x += 1.0f;

		if (normal.y < 0)
			res.y += 1.0f;

		if (normal.z < 0)
			res.z += 1.0f;

		return(res);
	}

	public int boxInFrustum(Vec3 box)
	{
		int result = INSIDE;
		for(int i=0; i < 6; i++) {

			if (pl[i].distance(getVertexP(pl[i].normal, box)) < 0)
				return OUTSIDE;
			else if (pl[i].distance(getVertexN(pl[i].normal, box)) < 0)
				result =  INTERSECT;
		}
		return(result);
	 }

	public void drawPoints()
	{
		GL.Begin(BeginMode.Points);

			GL.Vertex3(ntl.x,ntl.y,ntl.z);
			GL.Vertex3(ntr.x, ntr.y, ntr.z);
			GL.Vertex3(nbl.x, nbl.y, nbl.z);
			GL.Vertex3(nbr.x, nbr.y, nbr.z);

			GL.Vertex3(ftl.x, ftl.y, ftl.z);
			GL.Vertex3(ftr.x, ftr.y, ftr.z);
			GL.Vertex3(fbl.x, fbl.y, fbl.z);
			GL.Vertex3(fbr.x, fbr.y, fbr.z);

		GL.End();
	}

	public void drawLines() {
		GL.Color3(1.0f, 0.0f, 0.0f);
		GL.Begin(BeginMode.LineLoop);
		//near plane
			GL.Vertex3(ntl.x, ntl.y, ntl.z);
			GL.Vertex3(ntr.x, ntr.y, ntr.z);
			GL.Vertex3(nbr.x, nbr.y, nbr.z);
			GL.Vertex3(nbl.x, nbl.y, nbl.z);
		GL.End();

		GL.Begin(BeginMode.LineLoop);
		//far plane
			GL.Vertex3(ftr.x, ftr.y, ftr.z);
			GL.Vertex3(ftl.x, ftl.y, ftl.z);
			GL.Vertex3(fbl.x, fbl.y, fbl.z);
			GL.Vertex3(fbr.x, fbr.y, fbr.z);
		GL.End();

		GL.Begin(BeginMode.LineLoop);
		//bottom plane
			GL.Vertex3(nbl.x, nbl.y, nbl.z);
			GL.Vertex3(nbr.x, nbr.y, nbr.z);
			GL.Vertex3(fbr.x, fbr.y, fbr.z);
			GL.Vertex3(fbl.x, fbl.y, fbl.z);
		GL.End();

		GL.Begin(BeginMode.LineLoop);
		//top plane
			GL.Vertex3(ntr.x, ntr.y, ntr.z);
			GL.Vertex3(ntl.x, ntl.y, ntl.z);
			GL.Vertex3(ftl.x, ftl.y, ftl.z);
			GL.Vertex3(ftr.x, ftr.y, ftr.z);
		GL.End();

		GL.Begin(BeginMode.LineLoop);
		//left plane
			GL.Vertex3(ntl.x, ntl.y, ntl.z);
			GL.Vertex3(nbl.x, nbl.y, nbl.z);
			GL.Vertex3(fbl.x, fbl.y, fbl.z);
			GL.Vertex3(ftl.x, ftl.y, ftl.z);
		GL.End();

		GL.Begin(BeginMode.LineLoop);
		// right plane
			GL.Vertex3(nbr.x, nbr.y, nbr.z);
			GL.Vertex3(ntr.x, ntr.y, ntr.z);
			GL.Vertex3(ftr.x, ftr.y, ftr.z);
			GL.Vertex3(fbr.x, fbr.y, fbr.z);

		GL.End();
	}

	public void drawPlanes()
	{
		GL.Begin(BeginMode.Quads);

		//near plane
		GL.Color3(0.0f, 1.0f, 0.0f);//Green
			GL.Vertex3(ntl.x, ntl.y, ntl.z);
			GL.Vertex3(ntr.x, ntr.y, ntr.z);
			GL.Vertex3(nbr.x, nbr.y, nbr.z);
			GL.Vertex3(nbl.x, nbl.y, nbl.z);

		//far plane
			GL.Color3(0.0f, 1.0f, 0.0f); //Green
			GL.Vertex3(ftr.x, ftr.y, ftr.z);
			GL.Vertex3(ftl.x, ftl.y, ftl.z);
			GL.Vertex3(fbl.x, fbl.y, fbl.z);
			GL.Vertex3(fbr.x, fbr.y, fbr.z);

		//bottom plane
			GL.Color3(0.0f, 0.0f, 1.0f);//Blue
			GL.Vertex3(nbl.x, nbl.y, nbl.z);
			GL.Vertex3(nbr.x, nbr.y, nbr.z);
			GL.Vertex3(fbr.x, fbr.y, fbr.z);
			GL.Vertex3(fbl.x, fbl.y, fbl.z);

		//top plane
			GL.Color3(0.0f, 0.0f, 1.0f);//Blue
			GL.Vertex3(ntr.x, ntr.y, ntr.z);
			GL.Vertex3(ntl.x, ntl.y, ntl.z);
			GL.Vertex3(ftl.x, ftl.y, ftl.z);
			GL.Vertex3(ftr.x, ftr.y, ftr.z);

		//left plane
			GL.Color3(1.0f, 0.0f, 1.0f);//Purple
			GL.Vertex3(ntl.x, ntl.y, ntl.z);
			GL.Vertex3(nbl.x, nbl.y, nbl.z);
			GL.Vertex3(fbl.x, fbl.y, fbl.z);
			GL.Vertex3(ftl.x, ftl.y, ftl.z);

		// right plane
			GL.Color3(1.0f, 0.0f, 1.0f);//Purple
			GL.Vertex3(nbr.x, nbr.y, nbr.z);
			GL.Vertex3(ntr.x, ntr.y, ntr.z);
			GL.Vertex3(ftr.x, ftr.y, ftr.z);
			GL.Vertex3(fbr.x, fbr.y, fbr.z);

		GL.End();
	}

	public void drawNormals() {
		Vec3 a,b;

		GL.Begin(BeginMode.Lines);

			// near
			a = (ntr.add(ntl).add(nbr).add(nbl)).scale(0.25f);
			b = a.add(pl[NEARP].normal);
			GL.Vertex3(a.x, a.y, a.z);
			GL.Vertex3(b.x, b.y, b.z);

			// far
			a = (ftr.add(ftl).add(fbr).add(fbl)).scale(0.25f);
			b = a.add(pl[FARP].normal);
			GL.Vertex3(a.x, a.y, a.z);
			GL.Vertex3(b.x, b.y, b.z);

			// left
			a = (ftl.add(fbl).add(nbl).add(ntl)).scale(0.25f);
			b = a.add(pl[LEFT].normal);
			GL.Vertex3(a.x, a.y, a.z);
			GL.Vertex3(b.x, b.y, b.z);
			
			// right
			a = (ftr.add(nbr).add(fbr).add(ntr)).scale(0.25f);
			b = a.add(pl[RIGHT].normal);
			GL.Vertex3(a.x, a.y, a.z);
			GL.Vertex3(b.x, b.y, b.z);
			
			// top
			a = (ftr.add(ftl).add(ntr).add(ntl)).scale(0.25f);
			b = a.add(pl[TOP].normal);
			GL.Vertex3(a.x, a.y, a.z);
			GL.Vertex3(b.x, b.y, b.z);
			
			// bottom
			a = (fbr.add(fbl).add(nbr).add(nbl)).scale(0.25f);
			b = a.add(pl[BOTTOM].normal);
			GL.Vertex3(a.x, a.y, a.z);
			GL.Vertex3(b.x, b.y, b.z);

		GL.End();
	}
}