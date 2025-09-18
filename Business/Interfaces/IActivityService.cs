using Entity.DTO;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Results;

namespace Business.Interfaces
{
    public interface IActivityService
    {
        Task<ServiceResult<IEnumerable<ActivityDTO>>> GetAllAsync();
        Task<ServiceResult<ActivityDTO>> GetByIdAsync(int id);
        Task<ServiceResult<ActivityDTO>> CreateAsync(ActivityDTO activityDto);
        Task<ServiceResult<ActivityDTO>> UpdateAsync(int id, ActivityDTO activityDto);
        Task<ServiceResult<ActivityDTO>> UpdatePartialAsync(int id, JsonPatchDocument<ActivityDTO> patchDoc);
        Task<ServiceResult> SoftDeleteAsync(int id);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult<IEnumerable<ActivityDTO>>> GetActivitiesByCategoryAsync(string category);
        Task<ServiceResult<IEnumerable<ActivityDTO>>> GetActiveActivitiesAsync();
    }
}