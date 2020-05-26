//http://notargs.com/blog/?p=183

using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using System.Runtime.InteropServices;

public class ParticleUpdater : MonoBehaviour
{
	[SerializeField] ComputeShader computeShader;
	[SerializeField] Shader renderer;
	[SerializeField] int particlesCount = 1000;
	[SerializeField] Texture2D texture;
	[SerializeField] Mesh mesh;
	[SerializeField] CameraEvent cameraevent;

	float time;
	Material mat;
	ComputeBuffer particleBuffer;

	struct Particle
	{
		public Vector3 position;
		public Vector2 uv;
	}

	// Use this for initialization
	void Start ()
	{
		particleBuffer = new ComputeBuffer (particlesCount, Marshal.SizeOf (typeof(Particle)));

		//最初に全部初期化するんじゃなくて、毎フレーム少しずつやっていけば発射されてるようになるのでは
		Particle[] particles = new Particle[particlesCount];
		for (int i = 0; i < particlesCount; i++) {
			Particle _particle = new Particle ();
			_particle.position = new Vector3 (Random.Range (-10.0f, 10.0f), Random.Range (-10.0f, 10.0f), 0);
			_particle.uv = new Vector2 ((_particle.position.x + 10f) * 0.05f, (_particle.position.y + 10f) * 0.05f);
			particles [i] = _particle;
		}
			
		particleBuffer.SetData (particles);

//		AddEvent ();
	}

	// Update is called once per frame
	void Update ()
	{
		UpdateParticle ();
		time += Time.deltaTime;
	}

	void UpdateParticle ()
	{
		computeShader.SetBuffer (0, "_Particles", particleBuffer);
		computeShader.SetFloat ("_Time", time);
		computeShader.SetInt ("_ParticlesCount", particlesCount);
		computeShader.Dispatch (0, particlesCount / 8 + 1, 1, 1);
	}

	void AddEvent ()
	{
		if (mat == null) {
			mat = new Material (renderer);
		}
			
//		var commandbuffer = new CommandBuffer ();
//		mat.SetBuffer ("_Particles", particleBuffer);
		mat.SetPass (0);
//		mat.SetTexture ("_Tex", texture);
		Graphics.DrawMesh (mesh, Matrix4x4.identity, mat, 0);
//		Graphics.DrawProcedural (MeshTopology.Points, particlesCount);
//		commandbuffer.DrawMesh (mesh, Matrix4x4.identity, mat);
//		commandbuffer.DrawProceduralIndirect (Matrix4x4.identity, mat, 0, MeshTopology.Points, particlesCount);
//		Camera.main.AddCommandBuffer (cameraevent, commandbuffer);
	}

	void OnRenderObject ()
	{
		AddEvent ();
	}

	void OnDisable ()
	{
		particleBuffer.Release ();
	}
}