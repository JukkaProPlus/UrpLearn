using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPepeline : RenderPipeline
{
    bool useDynamicBatching, useGPUInstancing;
    private CameraRender cameraRender = new CameraRender();

    public CustomRenderPepeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
    {
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
    }
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (Camera camera in cameras)
        {
            cameraRender.Render(context, camera, useDynamicBatching, useGPUInstancing);
        }
    }
}