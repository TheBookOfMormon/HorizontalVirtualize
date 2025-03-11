// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorVirtualize.Extensions;

internal sealed class BlazorVirtualizeJsInterop : IAsyncDisposable
{
    private const string JsFunctionsPrefix = "BlazorVirtualize";

    private readonly IBlazorVirtualizeJsCallbacks _owner;

    private readonly IJSRuntime _jsRuntime;

    private DotNetObjectReference<BlazorVirtualizeJsInterop>? _selfReference;

    [DynamicDependency(nameof(OnSpacerBeforeVisible))]
    [DynamicDependency(nameof(OnSpacerAfterVisible))]
    public BlazorVirtualizeJsInterop(IBlazorVirtualizeJsCallbacks owner, IJSRuntime jsRuntime)
    {
        _owner = owner;
        _jsRuntime = jsRuntime;
    }


    private IJSObjectReference? _module;
    public async ValueTask InitializeAsync(ElementReference spacerBefore, ElementReference spacerAfter, bool horizontal)
    {
        _selfReference = DotNetObjectReference.Create(this);
        _module ??= await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/_content/BlazorVirtualize.Extensions/js/BlazorVirtualize.js");
        await _module.InvokeVoidAsync($"{JsFunctionsPrefix}.init", _selfReference, spacerBefore, spacerAfter, horizontal);
    }


    [JSInvokable]
    public void OnSpacerBeforeVisible(float spacerSize, float spacerSeparation, float containerSize)
    {
        _owner.OnBeforeSpacerVisible(spacerSize, spacerSeparation, containerSize);
    }

    [JSInvokable]
    public void OnSpacerAfterVisible(float spacerSize, float spacerSeparation, float containerSize)
    {
        _owner.OnAfterSpacerVisible(spacerSize, spacerSeparation, containerSize);
    }

    public async ValueTask DisposeAsync()
    {
        if (_selfReference != null)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync($"{JsFunctionsPrefix}.dispose", _selfReference);
            }
            catch (JSDisconnectedException)
            {
                // If the browser is gone, we don't need it to clean up any browser-side state
            }
        }
    }
}
