using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using UmbracoBlogCMS.App_Code.Models.ViewComponentModels;

namespace UmbracoBlogCMS.App_Code.ViewComponents
{
    [ViewComponent]
    public class MetaDataViewComponent : ViewComponent
    {
        private readonly ILogger<MetaDataViewComponent> logger;
        private readonly IUmbracoContextAccessor context;
        public MetaDataViewComponent(ILogger<MetaDataViewComponent> logger, IUmbracoContextAccessor context)
        {
            this.logger = logger;
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            MetaDataView dataView = new();
            try
            {
                var content = context.GetRequiredUmbracoContext()?.PublishedRequest?.PublishedContent;

                if (content == null) { return View(dataView); }

                dataView.Title = content?.Value<string>("title");
                dataView.Description = content?.Value<string>("description");
                dataView.Author = content?.Value<string>("author");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while processing {nameof(MetaDataViewComponent)}");
            }
            return View(dataView);
        }
    }
}
