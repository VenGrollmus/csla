﻿//-----------------------------------------------------------------------
// <copyright file="BlazorWasmConfigurationExtensions.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>Implement extension methods for .NET Core configuration</summary>
//-----------------------------------------------------------------------
using Csla.State;
using Csla.Blazor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Csla.Blazor.WebAssembly.Configuration;

namespace Csla.Configuration
{
  /// <summary>
  /// Implement extension methods for Blazor WebAssembly
  /// </summary>
  public static class BlazorWasmConfigurationExtensions
  {
    /// <summary>
    /// Registers services necessary for Blazor WebAssembly.
    /// </summary>
    /// <param name="config">CslaConfiguration object</param>
    public static CslaOptions AddBlazorWebAssembly(this CslaOptions config)
    {
      return AddBlazorWebAssembly(config, null);
    }

    /// <summary>
    /// Registers services necessary for Blazor WebAssembly.
    /// </summary>
    /// <param name="config">CslaConfiguration object</param>
    /// <param name="options">Options object</param>
    public static CslaOptions AddBlazorWebAssembly(this CslaOptions config, Action<BlazorWebAssemblyConfigurationOptions> options)
    {
      var blazorOptions = new BlazorWebAssemblyConfigurationOptions();
      options?.Invoke(blazorOptions);

      config.Services.AddScoped((_) => blazorOptions);
      config.Services.AddTransient(typeof(ViewModel<>), typeof(ViewModel<>));
      config.Services.AddScoped<IAuthorizationPolicyProvider, CslaPermissionsPolicyProvider>();
      config.Services.AddScoped<IAuthorizationHandler, CslaPermissionsHandler>();
      config.Services.AddScoped(typeof(Csla.Core.IContextManager), typeof(Csla.Blazor.WebAssembly.ApplicationContextManager));
      config.Services.AddScoped(typeof(AuthenticationStateProvider), typeof(Csla.Blazor.Authentication.CslaAuthenticationStateProvider));

      // use Blazor state management
      config.Services.AddScoped(typeof(ISessionManager), blazorOptions.SessionManagerType);
      config.Services.AddTransient<Blazor.State.StateManager>();

      return config;
    }
  }
}