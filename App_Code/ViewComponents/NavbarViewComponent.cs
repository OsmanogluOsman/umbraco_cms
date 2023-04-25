using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;
using UmbracoBlogCMS.App_Code.Models.ViewComponentModels;

namespace UmbracoBlogCMS.App_Code.ViewComponents
{
    [ViewComponent]
    public class NavbarViewComponent : ViewComponent
    {
        private readonly ILogger<NavbarViewComponent> logger;
        private readonly IUmbracoContextAccessor context;
        private readonly UmbracoHelper umbracoHelper;

        public NavbarViewComponent(ILogger<NavbarViewComponent> logger, IUmbracoContextAccessor context, UmbracoHelper umbracoHelper)
        {
            this.logger = logger;
            this.context = context;
            this.umbracoHelper = umbracoHelper;
        }

        public IViewComponentResult Invoke()
        {
            NavbarView navbarView = new();
            try
            {
                var navbar = umbracoHelper?.ContentAtRoot()?.FirstOrDefault(x => x.IsDocumentType("home") && x.IsVisible()) as Home;
                if (navbar == null) { return View(); }

                navbarView.SiteName = navbar?.SiteName;
                navbarView.LogoUrl = navbar?.Logo?.Url();

                foreach (var item in navbar.Children)
                {
                    navbarView.NavbarChildren.Add(
                        new NavbarChild
                        {
                            Name = item.Name,
                            Url = item?.Url()
                        });
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while processing {nameof(NavbarViewComponent)}");
            }
            return View(navbarView);
        }
    }
}
