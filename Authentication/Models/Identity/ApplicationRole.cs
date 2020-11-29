using AspNetCore.Identity.Mongo.Model;

namespace Authentication.Models.Identity
{
    public class ApplicationRole : MongoRole
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string name) : base(name)
        {
        }
    }
}