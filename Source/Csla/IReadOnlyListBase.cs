﻿using Csla.Core;
using Csla.Serialization.Mobile;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Csla
{
  /// <summary>
  /// This is the base class from which readonly collections
  /// of readonly objects should be derived.
  /// </summary>
  public interface IReadOnlyListBase<C>
 : IReadOnlyCollection, IBusinessObject, ICloneable, IObservableBindingList,
    INotifyBusy, INotifyUnhandledAsyncException, INotifyChildChanged, ISerializationNotification,
    IMobileObject, INotifyCollectionChanged, INotifyPropertyChanged,
    IList<C>, ICollection<C>, IEnumerable<C>;
}
