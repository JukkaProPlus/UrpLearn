#ifndef CUSTOM_UNLIT_PASS_INCLUDED
#define CUSTOM_UNLIT_PASS_INCLUDED
//顶点函数
float4 UnlitPassVertex(float3 positionOS:POSITION):SV_POSITION
{
    return float4(positionOS, 1.0);
}
//片元函数
void UnlitPassFragment()
{
    
}
#endif