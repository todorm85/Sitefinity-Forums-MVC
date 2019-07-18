using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Forums;
using Telerik.Sitefinity.Forums.Model;
using Telerik.Sitefinity.Frontend.Mvc.Helpers;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.Mvc.Models;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Pages.Configuration;
using Telerik.Sitefinity.Mvc;

namespace MVC_Forums.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "ForumThreads_MVC", Title = "Forum Threads", SectionName = ToolboxesConfig.ContentToolboxSectionName, ModuleName = "Forums")]
    public class ThreadsController : ContentBaseController
    {
        // TODO: designer with selector for forum or perhaps not needed - app will have only one forum, we need to list only threads with their posts
        public string ForumName { get; set; }

        public ActionResult Index(int? page)
        {
            var pageNumber = this.ExtractValidPage(page);
            var items = this.GetThreads().Skip((pageNumber - 1) * 50).Select(x => new ItemViewModel(x));

            var viewModel = new ContentListViewModel()
            {
                Items = items.ToList()
            };

            // TODO: Cache dependencies?

            return this.View("List.ForumThreadsList", viewModel);
        }

        public ActionResult Details(ForumThread item)
        {
            this.ViewBag.ThreadTitle = item.Title;
            this.InitializeMetadataDetailsViewBag(item);  // Sitefinity SEO optimizations
            var posts = this.GetPosts(item).Select(x => new ItemViewModel(x));

            var viewModel = new ContentListViewModel()
            {
                Items = posts.ToList()
            };

            return this.View("Detail.ForumThreadDetails", viewModel);
        }

        private IQueryable<ForumThread> GetThreads()
        {
            // TODO: provider
            return ForumsManager.GetManager().GetThreads().Where(x => x.Forum.Title == this.ForumName);
        }

        private IQueryable<ForumPost> GetPosts(ForumThread thread)
        {
            // TODO: provider
            return ForumsManager.GetManager().GetPosts(thread.Id);
        }
    }
}