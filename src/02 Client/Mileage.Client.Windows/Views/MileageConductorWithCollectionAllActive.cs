﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using ReactiveUI;

namespace Mileage.Client.Windows.Views
{
    public partial class MileageConductor<T>
    {
        /// <summary>
        /// An implementation of <see cref="IConductor"/> that holds on many items.
        /// </summary>
        public partial class Collection
        {
            /// <summary>
            /// An implementation of <see cref="IConductor"/> that holds on to many items wich are all activated.
            /// </summary>
            public abstract class AllActive : MileageConductorBase<T>
            {
                readonly ReactiveObservableCollection<T> items = new ReactiveObservableCollection<T>();
                
                /// <summary>
                /// Initializes a new instance of the <see cref="Conductor&lt;T&gt;.Collection.AllActive"/> class.
                /// </summary>
                public AllActive(IWindsorContainer container) : base(container)
                {
                    items.CollectionChanged += (s, e) =>
                    {
                        switch (e.Action)
                        {
                            case NotifyCollectionChangedAction.Add:
                                e.NewItems.OfType<IChild>().Apply(x => x.Parent = this);
                                break;
                            case NotifyCollectionChangedAction.Remove:
                                e.OldItems.OfType<IChild>().Apply(x => x.Parent = null);
                                break;
                            case NotifyCollectionChangedAction.Replace:
                                e.NewItems.OfType<IChild>().Apply(x => x.Parent = this);
                                e.OldItems.OfType<IChild>().Apply(x => x.Parent = null);
                                break;
                            case NotifyCollectionChangedAction.Reset:
                                items.OfType<IChild>().Apply(x => x.Parent = this);
                                break;
                        }
                    };
                }

                /// <summary>
                /// Gets the items that are currently being conducted.
                /// </summary>
                public IObservableCollection<T> Items
                {
                    get { return items; }
                }

                /// <summary>
                /// Gets the items that are currently being conducted as a ReactiveList.
                /// </summary>
                public ReactiveList<T> ReactiveItems
                {
                    get { return items as ReactiveList<T>; }
                }

                /// <summary>
                /// Called when activating.
                /// </summary>
                protected override void OnActivate()
                {
                    items.OfType<IActivate>().Apply(x => x.Activate());
                }

                /// <summary>
                /// Called when deactivating.
                /// </summary>
                /// <param name="close">Inidicates whether this instance will be closed.</param>
                protected override void OnDeactivate(bool close)
                {
                    items.OfType<IDeactivate>().Apply(x => x.Deactivate(close));
                    if (close)
                    {
                        items.Clear();
                    }
                }

                /// <summary>
                /// Called to check whether or not this instance can close.
                /// </summary>
                /// <param name="callback">The implementor calls this action with the result of the close check.</param>
                public override void CanClose(Action<bool> callback)
                {
                    CloseStrategy.Execute(items.ToList(), (canClose, closable) =>
                    {
                        if (!canClose && closable.Any())
                        {
                            closable.OfType<IDeactivate>().Apply(x => x.Deactivate(true));
                            items.RemoveRange(closable);
                        }

                        callback(canClose);
                    });
                }
                /// <summary>
                /// Activates the specified item.
                /// </summary>
                /// <param name="item">The item to activate.</param>
                public override void ActivateItem(T item)
                {
                    if (item == null)
                    {
                        return;
                    }

                    item = EnsureItem(item);

                    if (IsActive)
                    {
                        ScreenExtensions.TryActivate(item);
                    }

                    OnActivationProcessed(item, true);
                }

                /// <summary>
                /// Deactivates the specified item.
                /// </summary>
                /// <param name="item">The item to close.</param>
                /// <param name="close">Indicates whether or not to close the item after deactivating it.</param>
                public override void DeactivateItem(T item, bool close)
                {
                    if (item == null)
                    {
                        return;
                    }

                    if (close)
                    {
                        CloseStrategy.Execute(new[] { item }, (canClose, closable) =>
                        {
                            if (canClose)
                                CloseItemCore(item);
                        });
                    }
                    else
                    {
                        ScreenExtensions.TryDeactivate(item, false);
                    }
                }

                /// <summary>
                /// Gets the children.
                /// </summary>
                /// <returns>The collection of children.</returns>
                public override IEnumerable<T> GetChildren()
                {
                    return items;
                }

                void CloseItemCore(T item)
                {
                    ScreenExtensions.TryDeactivate(item, true);
                    items.Remove(item);
                }

                /// <summary>
                /// Ensures that an item is ready to be activated.
                /// </summary>
                /// <param name="newItem"></param>
                /// <returns>The item to be activated.</returns>
                protected override T EnsureItem(T newItem)
                {
                    var index = items.IndexOf(newItem);

                    if (index == -1)
                    {
                        items.Add(newItem);
                    }
                    else
                    {
                        newItem = items[index];
                    }

                    return base.EnsureItem(newItem);
                }
            }
        }
    }
}