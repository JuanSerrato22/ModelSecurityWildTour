using Entity.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Interface
{
    public interface IActivityController
    {
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> GetActive();
        Task<IActionResult> GetByCategory(string category);
        Task<IActionResult> Create(ActivityDTO activityDto);
        Task<IActionResult> Update(int id, ActivityDTO activityDto);
        Task<IActionResult> UpdatePartial(int id, JsonPatchDocument<ActivityDTO> patchDoc);
        Task<IActionResult> SoftDelete(int id);
        Task<IActionResult> Delete(int id);
    }
}