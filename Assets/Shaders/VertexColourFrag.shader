Shader "Custom/VertexColourFrag" {

Properties {

		_MainTex ("Diffuse", 2D) = "white" {}

     

    }
  SubShader {
    Pass {
    
    Cull Back
			ZWrite On
			Alphatest Greater 0.5
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #include "UnityCG.cginc"
      
      sampler2D _MainTex;
      fixed4 _MainTex_ST;
      
      struct appdata {
    	float4 vertex : POSITION;
    	float2  texcoord : TEXCOORD0;
    	fixed4 color : COLOR;
	};

      struct v2f {
          float4 pos : POSITION;
          float2 uv : TEXCOORD0; 
          fixed4 color : COLOR;
      };

      v2f vert (appdata v)
      {
          v2f o;
          o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
          
          o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
          //o.color.xyz = v.normal * 0.5 + 0.5;
          //o.color.w = 1.0;
          o.color = v.color;
          return o;
      }

      fixed4 frag (v2f i) : COLOR {
      
      	float4 c = tex2D (_MainTex, i.uv);
      	
      	return c * i.color;
        //return i.color; 
        
        
        }
      ENDCG
    }
  } 
}
