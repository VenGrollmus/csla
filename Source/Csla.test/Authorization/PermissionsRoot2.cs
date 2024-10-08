﻿//-----------------------------------------------------------------------
// <copyright file="PermissionsRoot.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: https://cslanet.com
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------

using Csla.Rules;
using Csla.Rules.CommonRules;

namespace Csla.Test.Security
{
#if TESTING
    [DebuggerNonUserCode]
#endif
  [Serializable()]
  public class PermissionsRoot2 : BusinessBase<PermissionsRoot2>
  {
    private int _ID = 0;
    protected override object GetIdValue()
    {
      return _ID;
    }

    public static PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(c => c.FirstName);
    private string _firstName = FirstNameProperty.DefaultValue;
    public string FirstName
    {
      get
      {
        if (CanReadProperty("FirstName"))
        {
          return _firstName;
        }
        else
        {
          throw new Csla.Security.SecurityException("Property get not allowed");
        }
      }
      set
      {
        if (CanWriteProperty("FirstName"))
        {
          _firstName = value;
        }
        else
        {
          throw new Csla.Security.SecurityException("Property set not allowed");
        }
      }
    }

    public readonly static Csla.Core.IMemberInfo DoWorkMethod = RegisterMethod(typeof(PermissionsRoot), "DoWork");

    public void DoWork()
    {
      CanExecuteMethod(DoWorkMethod, true);
    }

    #region Authorization

    public static void AddObjectAuthorizationRules()
    {
      // add rules for multiple rulesets
      BusinessRules.AddRule(typeof(PermissionsRoot2), new IsInRole(AuthorizationActions.DeleteObject, "User"));
      BusinessRules.AddRule(typeof(PermissionsRoot2), new IsInRole(AuthorizationActions.DeleteObject, "Admin"), "custom1");
      BusinessRules.AddRule(typeof(PermissionsRoot2), new IsInRole(AuthorizationActions.DeleteObject, "User", "Admin"), "custom2");
    }

    protected override void AddBusinessRules()
    {
      BusinessRules.AddRule(new Csla.Rules.CommonRules.IsInRole(Rules.AuthorizationActions.ReadProperty, FirstNameProperty, new List<string> { "Admin" }));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.IsInRole(Rules.AuthorizationActions.WriteProperty, FirstNameProperty, new List<string> { "Admin" }));
      BusinessRules.AddRule(new Csla.Rules.CommonRules.IsInRole(Rules.AuthorizationActions.ExecuteMethod, DoWorkMethod, new List<string> { "Admin" }));
    }

    #endregion

    #region "Criteria"

    [Serializable()]
    private class Criteria
    {
      //implement
    }

    #endregion

    [RunLocal()]
    [Create]
    protected void DataPortal_Create()
    {
      _firstName = "default value"; //for now...
    }
  }
}