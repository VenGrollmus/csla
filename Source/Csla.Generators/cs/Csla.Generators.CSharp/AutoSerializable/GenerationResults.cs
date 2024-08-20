﻿//-----------------------------------------------------------------------
// <copyright file="GenerationResults.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>The results of source generation</summary>
//-----------------------------------------------------------------------

namespace Csla.Generators.CSharp.AutoSerialization
{

  /// <summary>
  /// The results of source generation
  /// </summary>
  public class GenerationResults
  {

    /// <summary>
    /// The fully qualified name of the generated type
    /// </summary>
    public string FullyQualifiedName { get; set; }

    /// <summary>
    /// The source code that has been generated by the builder
    /// </summary>
    public string GeneratedSource { get; set; }

  }

}
