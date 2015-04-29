using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using Caliburn.Micro.ReactiveUI;
using Castle.Windsor;
using JetBrains.Annotations;
using Mileage.Shared.Models;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.TagCloud
{
    public class TagCloudViewModel : MileageScreen
    {
        #region Fields
        private readonly ObservableAsPropertyHelper<ReactiveObservableCollection<TagWithCount>> _tagsHelper;
        private readonly ObservableAsPropertyHelper<int> _minTagCountHelper;
        private readonly ObservableAsPropertyHelper<int> _maxTagCountHelper;
        #endregion

        #region Properties
        public ReactiveObservableCollection<TagWithCount> Tags
        {
            get { return this._tagsHelper.Value; }
        }
        public int MinTagCount
        {
            get { return this._minTagCountHelper.Value; }
        }
        public int MaxTagCount
        {
            get { return this._maxTagCountHelper.Value; }
        }
        #endregion

        #region Commands
        public ReactiveCommand<ReactiveObservableCollection<TagWithCount>> LoadTags { get; private set; }
        #endregion

        public TagCloudViewModel([NotNull] IWindsorContainer container)
            : base(container)
        {
            this.LoadTags = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                HttpResponseMessage tagsResponse = await this.WebService.SearchClient.GetTags();

                if (tagsResponse.StatusCode == HttpStatusCode.Found)
                {
                    var tags = await tagsResponse.Content.ReadAsAsync<ReactiveObservableCollection<TagWithCount>>();
                    return tags;
                }

                return null;
            });
            this.LoadTags.ThrownExceptions.Subscribe(this.ExceptionHandler.Handle);
            this.LoadTags.ToProperty(this, f => f.Tags, out this._tagsHelper);

            this.WhenAnyValue(f => f.Tags)
                .Select(f => f.Min(d => d.Count))
                .ToProperty(this, f => f.MinTagCount, out this._minTagCountHelper);

            this.WhenAnyValue(f => f.Tags)
                .Select(f => f.Max(d => d.Count))
                .ToProperty(this, f => f.MaxTagCount, out this._maxTagCountHelper);
        }

        protected override async void OnInitialize()
        {
            await this.LoadTags.ExecuteAsyncTask();
        }
    }
}