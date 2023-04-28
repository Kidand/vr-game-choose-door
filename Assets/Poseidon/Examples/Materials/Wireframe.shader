Shader "Cinderflame/Wireframe"
{
    Properties
    {
		_WireColor ("Wire Color", Color) = (0.0, 1.0, 0.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM

            #pragma fragment Fragment
            #pragma geometry Geometry
            #pragma vertex Vertex
            
            #pragma target 5.0
            
            #include <UnityShaderUtilities.cginc>

            fixed4 _WireColor;
            
            struct VertexData
            {
                float4 vertex : SV_Position;
                float2 barycentric : BARYCENTRIC;
            };
 
            void Vertex(inout float4 vertex:POSITION) { /* Don't do anything here */ }
 
            [maxvertexcount(3)]
            void Geometry(triangle float4 tri[3] : SV_Position, inout TriangleStream<VertexData> stream)
            {
                VertexData geometry;
                for (uint i = 0; i < 3; i++)
                {
                    geometry.vertex = UnityObjectToClipPos(tri[i]);
                    geometry.barycentric = float2(fmod(i, 2.0), step(2.0, i));
                    stream.Append(geometry);
                }
                stream.RestartStrip();
            }
 
            float4 Fragment(VertexData data) : SV_Target
            {
                float3 coord = float3(data.barycentric, 1.0 - data.barycentric.x - data.barycentric.y);
                coord = smoothstep(fwidth(coord) * 0.1, fwidth(coord) * 0.1 + fwidth(coord), coord);
                float3 color = lerp(_WireColor.xyz, float3(1,1,1), _SinTime.y * .5f + .5f);
                float alpha = _WireColor.w * (_SinTime.w * .25f + .5f) * (1 - min(coord.x, min(coord.y, coord.z)));
                return float4(color, alpha);
            }
            ENDCG
        }
    }
}
