using DeskBooker.Core.Domain;
using DeskBooker.Core.Processor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeskBooker.Web.Pages
{
    public class BookDeskModel : PageModel
    {
        public BookDeskModel(IDeskBookingRequestProcessor @object)
        {
            Object = @object;
        }

        [BindProperty]
        public DeskBookingRequest DeskBookingRequest { get; set; }
        public IDeskBookingRequestProcessor Object { get; }

        public void OnPost()
        {

        }
    }
}