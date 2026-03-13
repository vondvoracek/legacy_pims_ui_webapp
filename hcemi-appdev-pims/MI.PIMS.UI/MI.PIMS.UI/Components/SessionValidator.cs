using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Components
{
    [ViewComponent(Name = "SessionValidator")]
    public class SessionValidator : ViewComponent
    {
        public SessionValidator() { }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View());
        }
    }
}
