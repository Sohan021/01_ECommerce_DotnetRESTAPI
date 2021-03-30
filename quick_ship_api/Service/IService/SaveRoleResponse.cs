using quick_ship_api.Models.User;

namespace quick_ship_api.Service.IService
{
    public class SaveRoleResponse : BaseResponse<ApplicationRole>
    {
        public SaveRoleResponse(ApplicationRole applicationRole) : base(applicationRole)
        { }


        public SaveRoleResponse(string message) : base(message)
        { }
    }
}
