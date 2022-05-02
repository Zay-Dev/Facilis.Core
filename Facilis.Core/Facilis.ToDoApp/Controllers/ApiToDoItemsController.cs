using Facilis.Core.Abstractions;
using Facilis.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Facilis.ToDoApp.Controllers
{
    [Route("~/api/to-do-items")]
    public class ApiToDoItemsController : Controller
    {
        private IEntities<ToDoItem> items { get; }

        #region Constructor(s)

        public ApiToDoItemsController(IEntities<ToDoItem> items)
        {
            this.items = items;
        }

        #endregion Constructor(s)

        public IActionResult Index()
        {
            return new JsonResult(this.items
                .WhereAll(null)
                .OrderBy(item => item.Status)
                .ThenBy(item => item.Name)
                .ToArray()
            );
        }

        [HttpPost]
        [Route("{name}")]
        public IActionResult Index(string name)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest();

            this.items.Add(new ToDoItem() { Name = name });
            return Ok();
        }

        [HttpPatch]
        [Route("{id}/done")]
        public IActionResult MarkDone(string id)
        {
            return this.UpdateStatus(id, StatusTypes.Disabled) ?
                Ok() : NotFound();
        }

        [HttpPatch]
        [Route("{id}/undo")]
        public IActionResult MarkNotDone(string id)
        {
            return this.UpdateStatus(id, StatusTypes.Enabled) ?
                Ok() : NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(string id)
        {
            return this.UpdateStatus(id, StatusTypes.Deleted) ?
                Ok() : NotFound();
        }

        private bool UpdateStatus(string id, StatusTypes status)
        {
            var item = this.items.FindById(id);
            if (item == null) return false;

            item.Status = status;
            this.items.Update(item);

            return true;
        }
    }
}