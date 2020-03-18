using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(DepthCurvatureEffectRenderer), PostProcessEvent.AfterStack, "Azee/Depth Curvature", false)]
public sealed class DepthCurvatureEffect : PostProcessEffectSettings
{
    public FloatParameter curve = new FloatParameter { value = 0.5f };

    [Range(0, 1)]
    public FloatParameter minDepth = new FloatParameter { value = 0.5f };
}

public sealed class DepthCurvatureEffectRenderer : PostProcessEffectRenderer<DepthCurvatureEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        // var sheet = context.propertySheets.Get(Shader.Find("Azee/DepthCurvatureShader2"));
        // context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);

        Material material = new Material(Shader.Find("Azee/DepthCurvatureShader"));
        material.SetFloat("_Curve", settings.curve);
        material.SetFloat("_MinDepth", settings.minDepth);
        context.command.Blit(context.source, context.destination, material);
    }
}
