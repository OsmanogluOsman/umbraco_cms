using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using UmbracoBlogCMS.App_Code.Models.ViewComponentModels;

namespace UmbracoBlogCMS.App_Code.ViewComponents
{
    [ViewComponent]
    public class PageHeaderViewComponent : ViewComponent
    {
        private readonly ILogger<PageHeaderViewComponent> logger;
        private readonly IUmbracoContextAccessor context;
        public PageHeaderViewComponent(ILogger<PageHeaderViewComponent> logger, IUmbracoContextAccessor context)
        {
            this.logger = logger;
            this.context = context;
        }

        public IViewComponentResult Invoke()
        {
            PageHeaderView headerView = new();
            try
            {
                var content = context.GetRequiredUmbracoContext().PublishedRequest.PublishedContent;

                if (content == null) { return View(headerView); }

                headerView.Title = content?.Value<string>("title");
                headerView.SubTitle = content?.Value<string>("subTitle");
                headerView.ImageUrl = content?.Value<IPublishedContent>("pageBanner")?.Url();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while processing {nameof(PageHeaderViewComponent)}");
            }
            return View(headerView);
        }
    }
}
