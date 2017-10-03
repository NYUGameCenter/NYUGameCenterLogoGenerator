// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/ConnectionShader" {
Properties {
    _Center ("Center", Vector) = (0,0.7071,-1.414214,0.7071)
//    _Vector1 ("Circ1", Vector) = (1,0,0,0)
//    _Mod1    ("Mod1", Vector) = (1,0,0,0)
}

SubShader {
 
    Tags { "RenderType"="Transparent" "Queue"="Transparent" }
    LOD 200
    Blend SrcAlpha OneMinusSrcAlpha
    ZTest Less
 
    Pass {
 
		CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members srcPos)
			#pragma exclude_renderers d3d11 xbox360
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
	 
			float4 _Center;
//			float4 _Vector1;
//			float4 _Mod1;
	 
			struct v2f {
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			    float4 srcPos;
			};
			
			struct vertexInput {
				float4 vertex : POSITION;
			};
			
			struct vertexOutput {
				float4 pos : SV_POSITION;
			    fixed3 color : COLOR0;
				float4 position_in_world_space : TEXCOORD0;
			};
			
			bool sameSide(float4 p1, float4 p2, float4 a1, float4 b1){
	    		float3 cp1 = cross(b1.xyz-a1.xyz, p1.xyz-a1.xyz);
	    		float3 cp2 = cross(b1.xyz-a1.xyz, p2.xyz-a1.xyz);
	    
	    		return dot(cp1, cp2) >= 0;
	    	}

			bool pointInTriangle(float4 p, float4 a1, float4 b1, float4 c1){
	    		if (sameSide(p, a1, b1, c1) && sameSide(p, b1, a1, c1) && sameSide(p, c1, a1, b1)){
	    			return true;
	    		} else {
	    			return false;
	    		}
	    	}
	    	
	     float4 makeDistPoint(float4 worldVert, float4 edge, int axis){
//	     	return float4(worldVert.x, worldVert.y, worldVert.z, 0);
	     
	     	if(axis == 0){
	     		return float4(edge.x, worldVert.y, worldVert.z, 0);
	     	} else if (axis == 1){
	     		return float4(worldVert.x, worldVert.y, worldVert.z, 0);
	     	} else {
	     		return float4(worldVert.x, worldVert.y, edge.z, 0);
	     	}
	     
	     }
		
		 float4 newWorldSpace(float4 worldVert, float4 edge, int axis){

			float4 modDir1 = _Center - edge;
			modDir1 = normalize(modDir1);
			modDir1.w = 0.5;

			float4 edge1 = float4(edge.x, edge.y, edge.z, 0);
	
			float4 distPos = makeDistPoint(worldVert, edge, axis);
									
            float dist1 = distance(distPos, edge1);
            
            //Catch to keep from crashing with an infinite loop
            if(modDir1.x + modDir1.y + modDir1.z == 0){
            	modDir1.x = 1;
            }
            
            while(dist1 < modDir1.w){
            
            	float4 slope = normalize(edge1 - worldVert);
            	
            	float mod = 0.01;
            	
            	worldVert.x += modDir1.x * mod;
            	worldVert.y += modDir1.y * mod;
            	worldVert.z += modDir1.z * mod;
            	
            	
			 	distPos = makeDistPoint(worldVert, edge, axis);
            	
            	dist1 = distance(distPos, edge1);
            }
            
            return worldVert;
		 }

         vertexOutput vert(appdata_base input) 
         {
            vertexOutput output; 
            
            output.color = input.normal * 0.5 + 0.5;
 
            output.pos =  UnityObjectToClipPos(input.vertex);
            output.position_in_world_space = 
               mul(unity_ObjectToWorld, input.vertex);
               
            float center = float4(
            					output.position_in_world_space.x,
            					output.position_in_world_space.y,
            					output.position_in_world_space.z,
            					0);
             
            float4 dirE = float4(_Center.x + _Center.w,
            					_Center.y, _Center.z, 0);	
            float4 dirW = float4(_Center.x - _Center.w,
            					_Center.y, _Center.z, 0);	
            float4 dirF = float4(_Center.x, _Center.y,
            					 _Center.z - _Center.w, 0);	
            float4 dirB = float4(_Center.x, _Center.y,
            					 _Center.z + _Center.w, 0);	
            float4 dirU = float4(_Center.x, 
            					_Center.y - _Center.w,
            					 _Center.z, 0);	
            float4 dirD = float4(_Center.x, 
            					_Center.y + _Center.w,
            					 _Center.z, 0);	

            output.position_in_world_space = newWorldSpace(output.position_in_world_space, dirE, 2);
            output.position_in_world_space = newWorldSpace(output.position_in_world_space, dirW, 2);
            output.position_in_world_space = newWorldSpace(output.position_in_world_space, dirU, 1);
            output.position_in_world_space = newWorldSpace(output.position_in_world_space, dirD, 1);
            output.position_in_world_space = newWorldSpace(output.position_in_world_space, dirF, 0);
            output.position_in_world_space = newWorldSpace(output.position_in_world_space, dirB, 0);
            
            float4 tempVec = mul(unity_WorldToObject, output.position_in_world_space);
            	
           	output.pos = UnityObjectToClipPos(tempVec);
            
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR 
         {
         	float4 result = float4(1, 1, 1, 1); 
//         
////			float4 slope = normalize(_Vector1 - _Vector2);
////			float4 revSlope = float4(slope.y, -slope.x, slope.z, slope.a);
////
////			float lineDist = 0.35;
////			float lineDist2 = lineDist * 1.1;
//			float circRad = 1.23;
//			float circRad2 = circRad * 1.005;
////		
//			float dist1 = distance(input.position_in_world_space, _Vector1);
//			float dist2 = distance(input.position_in_world_space, _Vector2);
////			float dist3 = distance(input.position_in_world_space, _Vector3);
////
// 			if((dist1 < circRad) || (dist2 < circRad)){
//				result =  float4(1.0, 0.0, 0.0, 1.0); 
//			} 
			
			return result; 
//			 return fixed4 (input.color, 1);
         }
		ENDCG
 
	    }
	}

Fallback "VertexLit"
} 