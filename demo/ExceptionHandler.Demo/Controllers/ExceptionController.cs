using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TanvirArjel.ExceptionHandler;
using TanvirArjel.ExceptionHandler.Dto;
using TanvirArjel.ExceptionHandler.Services;

namespace ExceptionHandler.Demo.Controllers
{
    public class ExceptionController : Controller
    {
        private readonly IExceptionService _exceptionService;

        public ExceptionController(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
        }

        // GET: Exception
        public IActionResult ExceptionList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadExceptionListForDataTable([FromBody] DataTableDto dataTablesParams)
        {
            object dataTableObject = await _exceptionService.GetDataTableObject(dataTablesParams);
            return Json(dataTableObject);
        }

        // GET: Exception/Details/5
        public async Task<IActionResult> ExceptionDetails(long id)
        {
            var exceptionModel = await _exceptionService.GetExceptionAsync(id);
            if (exceptionModel == null)
            {
                return NotFound();
            }

            return View(exceptionModel);
        }

        // GET: Exception/Edit/5
        public async Task<IActionResult> UpdateException(long id)
        {
            var exceptionModel = await _exceptionService.GetExceptionAsync(id);
            if (exceptionModel == null)
            {
                return NotFound();
            }
            return View(exceptionModel);
        }
       

        // POST: Exception/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateException(long id, ExceptionModel exceptionModel)
        {
            if (id != exceptionModel.ExceptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _exceptionService.MarkExceptionAsFixedAsync(id);
                return RedirectToAction(nameof(ExceptionList));
            }
            return View(exceptionModel);
        }

        // GET: Exception/Delete/5
        public async Task<IActionResult> DeleteException(long id)
        {
            var exceptionModel = await _exceptionService.GetExceptionAsync(id);
            if (exceptionModel == null)
            {
                return NotFound();
            }

            return View(exceptionModel);
        }

        // POST: Exception/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _exceptionService.DeleteExceptionAsync(id);
            return RedirectToAction(nameof(ExceptionList));
        }

    }
}
