﻿using System;
using Tao.FreeGlut;
using Tao.OpenGl;
using Box2CS;

namespace Testbed
{
	public class TestDebugDraw : DebugDraw
	{
		public override void DrawCircle(Vec2 center, float radius, ColorF color)
		{
			const float k_segments = 16.0f;
			const float k_increment = (float)(2.0f * Math.PI / k_segments);
			float theta = 0.0f;
			Gl.glColor3f(color.R, color.G, color.B);
			Gl.glBegin(Gl.GL_LINE_LOOP);
			for (int i = 0; i < k_segments; ++i)
			{
				Vec2 v = center + radius * new Vec2((float)Math.Cos(theta), (float)Math.Sin(theta));
				Gl.glVertex2f(v.X, v.Y);
				theta += k_increment;
			}
			Gl.glEnd();
		}

		public override void DrawPolygon(Vec2[] vertices, int vertexCount, ColorF color)
		{
			Gl.glColor3f(color.R, color.G, color.B);
			Gl.glBegin(Gl.GL_LINE_LOOP);
			for (int i = 0; i < vertexCount; ++i)
			{
				Gl.glVertex2f(vertices[i].X, vertices[i].Y);
			}
			Gl.glEnd();
		}

		public override void DrawSegment(Vec2 p1, Vec2 p2, ColorF color)
		{
			Gl.glColor3f(color.R, color.G, color.B);
			Gl.glBegin(Gl.GL_LINES);
			Gl.glVertex2f(p1.X, p1.Y);
			Gl.glVertex2f(p2.X, p2.Y);
			Gl.glEnd();
		}

		public override void DrawSolidCircle(Vec2 center, float radius, Vec2 axis, ColorF color)
		{
			const float k_segments = 16.0f;
			const float k_increment = (float)(2.0f * Math.PI / k_segments);
			float theta = 0.0f;
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glColor4f(0.5f * color.R, 0.5f * color.G, 0.5f * color.B, 0.5f);
			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			for (int i = 0; i < k_segments; ++i)
			{
				Vec2 v = center + radius * new Vec2((float)Math.Cos(theta), (float)Math.Sin(theta));
				Gl.glVertex2f(v.X, v.Y);
				theta += k_increment;
			}
			Gl.glEnd();
			Gl.glDisable(Gl.GL_BLEND);

			theta = 0.0f;
			Gl.glColor4f(color.R, color.G, color.B, 1.0f);
			Gl.glBegin(Gl.GL_LINE_LOOP);
			for (int i = 0; i < k_segments; ++i)
			{
				Vec2 v = center + radius * new Vec2((float)Math.Cos(theta), (float)Math.Sin(theta));
				Gl.glVertex2f(v.X, v.Y);
				theta += k_increment;
			}
			Gl.glEnd();

			Vec2 p = center + radius * axis;
			Gl.glBegin(Gl.GL_LINES);
			Gl.glVertex2f(center.X, center.Y);
			Gl.glVertex2f(p.X, p.Y);
			Gl.glEnd();
		}

		public override void DrawSolidPolygon(Vec2[] vertices, int vertexCount, ColorF color)
		{
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glColor4f(0.5f * color.R, 0.5f * color.G, 0.5f * color.B, 0.5f);
			Gl.glBegin(Gl.GL_TRIANGLE_FAN);
			for (int i = 0; i < vertexCount; ++i)
			{
				Gl.glVertex2f(vertices[i].X, vertices[i].Y);
			}
			Gl.glEnd();
			Gl.glDisable(Gl.GL_BLEND);

			Gl.glColor4f(color.R, color.G, color.B, 1.0f);
			Gl.glBegin(Gl.GL_LINE_LOOP);
			for (int i = 0; i < vertexCount; ++i)
			{
				Gl.glVertex2f(vertices[i].X, vertices[i].Y);
			}
			Gl.glEnd();
		}

		public override void DrawTransform(Transform xf)
		{
			Vec2 p1 = xf.Position, p2;
			const float k_axisScale = 0.4f;
			Gl.glBegin(Gl.GL_LINES);

			Gl.glColor3f(1.0f, 0.0f, 0.0f);
			Gl.glVertex2f(p1.X, p1.Y);
			p2 = p1 + k_axisScale * xf.R.Col1;
			Gl.glVertex2f(p2.X, p2.Y);

			Gl.glColor3f(0.0f, 1.0f, 0.0f);
			Gl.glVertex2f(p1.X, p1.Y);
			p2 = p1 + k_axisScale * xf.R.Col2;
			Gl.glVertex2f(p2.X, p2.Y);

			Gl.glEnd();
		}

		public void DrawPoint(Vec2 p, float size, ColorF color)
		{
			Gl.glPointSize(size);
			Gl.glBegin(Gl.GL_POINTS);
			Gl.glColor3f(color.R, color.G, color.B);
			Gl.glVertex2f(p.X, p.Y);
			Gl.glEnd();
			Gl.glPointSize(1.0f);
		}

		public void DrawString(int x, int y, string str)
		{
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glPushMatrix();
			Gl.glLoadIdentity();
			int w = (int)Main.GLWindow.Width;
			int h = (int)Main.GLWindow.Height;
			Glu.gluOrtho2D(0, w, h, 0);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glPushMatrix();
			Gl.glLoadIdentity();

			Gl.glColor3f(0.9f, 0.6f, 0.6f);
			Gl.glRasterPos2i(x, y);

			foreach (var c in str)
				Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_8_BY_13, c);

			Gl.glPopMatrix();
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glPopMatrix();
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
		}

		public void DrawAABB(AABB aabb, ColorF c)
		{
			Gl.glColor3f(c.R, c.G, c.B);
			Gl.glBegin(Gl.GL_LINE_LOOP);
			Gl.glVertex2f(aabb.LowerBound.X, aabb.LowerBound.Y);
			Gl.glVertex2f(aabb.UpperBound.X, aabb.LowerBound.Y);
			Gl.glVertex2f(aabb.UpperBound.X, aabb.UpperBound.Y);
			Gl.glVertex2f(aabb.LowerBound.X, aabb.UpperBound.Y);
			Gl.glEnd();
		}
	}
}
