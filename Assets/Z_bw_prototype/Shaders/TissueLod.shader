Shader "Tissue LOD"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		// This shader is unaffected by lights in the scene
		_Detail1Tex ("Detail 1 Base (RGB)", 2D) = "white" {}
		_Detail1Scale ("Detail 1 Scale", Float) = 20
		// Inverse radius is the strength of the detail map. 0 is no effect. 1 places full black for black.
		_Detail1InverseRadius ("Detail 1 Inverse Radius", Float) = 0.2
		_Detail2Tex ("Detail 2 Base (RGB)", 2D) = "white" {}
		_Detail2Scale ("Detail 2 Scale", Float) = 200
		_Detail2InverseRadius ("Detail 2 Inverse Radius", Float) = 2
		_FoliageMap ("Foliage Map (RGB)", 2D) = "white" {}
		_TissueMap ("Tissue Map (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
		}
		Pass
		{
			Lighting Off
			Cull     Off
			
			CGPROGRAM
				#pragma vertex   Vert
				#pragma fragment Frag
				
				sampler2D _MainTex;
				sampler2D _BumpMap;
				sampler2D _Detail1Tex;
				float     _Detail1Scale;
				float     _Detail1InverseRadius;
				sampler2D _Detail2Tex;
				float     _Detail2Scale;
				float     _Detail2InverseRadius;
				
				struct a2v // a2v is "our input"
				{
					float4 vertex   : POSITION; //position in object co-ordinates, local or model co-ordinates
					float3 normal   : NORMAL; //surface normal vector
					float4 tangent  : TANGENT; //vector orthogonal to the surface normal
					float2 texcoord : TEXCOORD0;
				};
				
				struct v2f
				{
					float4 pos  : SV_POSITION; // The position of the vertex converted to projection space
					float2 uv0  : TEXCOORD0;
					float3 wP   : TEXCOORD1;
					float3 wN   : TEXCOORD2;
					float3 wT   : TEXCOORD3;
					float3 wB   : TEXCOORD4;
					float  fade : TEXCOORD5;
				};
				
				struct f2g
				{
					half4 col : COLOR;
				};
				
				inline fixed3 UnpackNormalDXT5nm(fixed4 packednormal)
				{
					fixed3 normal;
					normal.xy = packednormal.wy * 2 - 1; //Unity converts normal maps to 4-channel maps.
					// w is the forth component
					normal.z  = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
					
					// This line from UnityCG.cginc below works exactly the same as above.
					//normal.z = sqrt(1 - normal.x*normal.x - normal.y * normal.y);
					
					return normal;
				}
				
				inline fixed3 UnpackNormal(fixed4 packednormal)
				{
					#if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) && defined(SHADER_API_MOBILE)
						return packednormal.xyz * 2 - 1;
					#else
						return UnpackNormalDXT5nm(packednormal);
					#endif
				}
				
				void Vert(a2v i, out v2f o)
				{
					o.pos  = mul(UNITY_MATRIX_MVP, i.vertex); // current model*view*projection matrix
					o.uv0  = i.texcoord;
					o.wP   = mul(_Object2World, i.vertex).xyz; // current model matrix (is this position?)
					o.wN   = normalize(mul(_Object2World, float4(i.normal, 0.0f)).xyz);
					// i.normal is three co-ordinates? Why define a float4 then use only the first three?
					o.wT   = normalize(mul(_Object2World, i.tangent).xyz);
					// the tangent
					o.wB   = cross(o.wN, o.wT);
					// the cross-product of the normal and the tangent, known as the binormal
				}
				
				void Frag(v2f i, out f2g o)
				{
					float4   main  = tex2D(_MainTex, i.uv0); // Tex2D is standard texture lookup in CG
					float4   bump  = tex2D(_BumpMap, i.uv0);
					// bump alone appears as a greyscale image, lit from above
					
					float4   white = float4(1.0f, 1.0f, 1.0f, 1.0f);
					float4   det1  = tex2D(_Detail1Tex, i.uv0 * _Detail1Scale);
					float4   det2  = tex2D(_Detail2Tex, i.uv0 * _Detail2Scale);
					float3x3 tbn   = float3x3(i.wT, i.wB, i.wN);
					// Tangent, Binormal and Normal together in one variable
					// Commonly used when calculating lighting
					
					float3 c2v  = _WorldSpaceCameraPos - i.wP; //distance from camera to vertex in xyz
					float  c2vL = length(c2v); // the length of that vector3
					float3 c2vD = normalize(c2v); // the direction of that vector3 relative to the camera
					
					float3 normal    = mul(UnpackNormal(bump), tbn);
					// mul is the matrix multiplication command, but UnpackNormal(bump) is the matrix
					// you can have a float3x3 called rotation, a float3 called pointPos and float3 called 
					// lightDirection.
					// output.lightDirection = mul(rotation, pointPos);
					
					float brightness = dot(normal, c2vD) * 0.6f + 0.4f;
					
					// detail textures, calculated by their distance from the camera
					// saturate clamps between 0 and 1
					// this lessens the power of the detail map based on settings
					det1 = lerp(det1, white, saturate(c2vL * _Detail1InverseRadius));
					det2 = lerp(det2, white, saturate(c2vL * _Detail2InverseRadius));
					
					//main     *= det1 * det2;
					// detail maps are multipled by the main texture
					//main.xyz *= clamp(brightness, 0.0f, 1.0f);
					
					// EXPERIMENTATIONS BELOW
					
					//main.xyz = bump.xyz * 2 - 1;
					// ^ Returns a greyscale image
					
					//main.xyz = mul((bump.xyz * 2 - 1), tbn);
					
					// ^ Very psychodelic. Patches of colour similar to details on the normal map. 
					// The patches of colour change shape when the object moves in the world-space.
					
					//main.xyz = bump.z;
					// ^ changing bump.x to bump.y or bump.z brings no change. The texture looks like
					// it's being lit from the top.
					// Changing it to bump.w makes it appear to be lit from the right.
					// So Unity actually gives a bump map an extra channel. Is this only in Direct X?
					

					main.xy = bump.wy * 2 - 1;
					main.z = 0;
					// ^ bump.wx, bump.wy and bump.wz is the same. Lit green from above. Red to the right.
					//Black everywhere else.
					

					//main.xy = bump.wy * 2 - 1;
					//main.xyz = dot(main.xy, main.xy);
					// ^ Bizarre result - all detail facing directly upwards is black. Facing outwards is white.
					// I can see why that that second line would be the blue channel
					
					// .xyz is the same as .rgb
					
					//main.xyz = c2v;
					// ^ If the camera is a 0,0,0, the sphere is pure white and (-1.5,-1.5,-1.5). Should be easy to figure
					// out why. Normalizing this makes it pearl-like because all colours have to form the product of 1.
					// Remember that at (1.5,1.5,1.5) the values are black because they're negative.
					// Try making the camera a child of the sphere and rotating the sphere. 
					
					//main.xyz = dot(normal, c2vD);
					// ^ Amazing. Greyscale bumpy texture like there's a light on the camera. But how?
					// The " * 0.6f + 0.4f" is a hard-coded way to control the brightness and contrast.
					
					// EXPERIMENTATIONS ABOVE
					
					//main.xyz = normal;
					//main.xyz = dot(mul((bump.xyz * 2 - 1), tbn), c2vD);
					
					o.col = main;
				}
			ENDCG
		} // Pass
	} // SubShader
} // Shader