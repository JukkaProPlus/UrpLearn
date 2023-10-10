#ifndef CUSTOM_UNLIT_PASS_INCLUDED
#define CUSTOM_UNLIT_PASS_INCLUDED

#include "../ShaderLibrary/Common.hlsl"
//顶点函数
float4 UnlitPassVertex(float3 positionOS:POSITION):SV_POSITION
{
    float3 positionWS = TransformObjectToWorld(positionOS.xyz);
    return TransformWorldToHClip(positionWS);
}

//所有材质的属性我们需要在常量缓冲区里定义
CBUFFER_START(UnityPerMaterial)
    float4 _BaseColor;
CBUFFER_END

//片元函数
float4 UnlitPassFragment():SV_TARGET
{
    return _BaseColor;
}
#endif