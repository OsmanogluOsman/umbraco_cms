using Microsoft.AspNetCore.Mvc;
using NUglify.Helpers;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;
using UmbracoBlogCMS.App_Code.Models.ViewComponentModels;

namespace UmbracoBlogCMS.App_Code.ViewComponents
{
    [ViewComponent]
    public class FooterViewComponent : ViewComponent
    {
        private readonly ILogger<FooterViewComponent> logger;
        private readonly UmbracoHelper umbracoHelper;
        public FooterViewComponent(ILogger<FooterViewComponent> logger, UmbracoHelper umbracoHelper)
        {
            this.logger = logger;
            this.umbracoHelper = umbracoHelper;
        }

        public IViewComponentResult Invoke()
        {
            FooterView footerView = new();
            try
            {
                var homePage = umbracoHelper?.ContentAtRoot()?.FirstOrDefault(x => x.IsDocumentType("home") && x.IsVisible()) as Home;
                if (homePage == null) { return View(); }

                homePage?.FooterLinks?.ForEach(x => footerView?.FooterLinks?.Add(new FooterLink
                {
                    Name = x.Name,
                    Target = x.Target,
                    Url = x.Url,
                }));

                foreach (var item in homePage?.SocialMediaPicker)
                {
                    var socialMediaElement = item.Content as SocialMediaElement;
                    footerView?.SocialMedia?.Add(new SocialMedia
                    {
                        Icon = socialMediaElement?.Icon?.ToString(),
                        Target = socialMediaElement?.Target,
                        Url = socialMediaElement?.SocialMediaUrl
                    });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while processing {nameof(FooterViewComponent)}");
            }
            return View(footerView);
        }
    }
}
