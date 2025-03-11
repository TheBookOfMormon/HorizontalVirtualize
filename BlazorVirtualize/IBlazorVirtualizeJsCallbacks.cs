// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace BlazorVirtualize;

internal interface IBlazorVirtualizeJsCallbacks
{
    void OnBeforeSpacerVisible(float spacerSize, float spacerSeparation, float containerSize);
    void OnAfterSpacerVisible(float spacerSize, float spacerSeparation, float containerSize);
}
