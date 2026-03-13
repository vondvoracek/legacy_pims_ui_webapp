using MI.PIMS.BO.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MI.PIMS.UI.Areas.DPOC.Components
{
    public class _Guideline_DiagnosisCodes_Hover(): ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(
            string p_DPOC_HIERARCHY_KEY,
            DateTime p_DPOC_VER_EFF_DT,
            string p_DPOC_PACKAGE,
            string p_DPOC_RELEASE,
            string p_DPOC_VER_NUM)
        {

            return await Task.FromResult(View(new DPOC_Gdln_Param_Dto
            {
                p_DPOC_HIERARCHY_KEY = p_DPOC_HIERARCHY_KEY,
                p_DPOC_VER_EFF_DT = p_DPOC_VER_EFF_DT,
                p_DPOC_PACKAGE = p_DPOC_PACKAGE,
                p_DPOC_RELEASE = p_DPOC_RELEASE,
                p_DPOC_VER_NUM = p_DPOC_VER_NUM
            }));
        }
    }
}
