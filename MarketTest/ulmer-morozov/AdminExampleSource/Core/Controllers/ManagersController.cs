using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Interfaces;
using Core.Models;

namespace Core.Controllers
{
    public class ManagersController : ApiController
    {
        private readonly IManagerRepository _manageRepository;

        public ManagersController(IManagerRepository manageRepository)
        {
            _manageRepository = manageRepository;
        }


        public async Task<IEnumerable<Manager>> Get(string search = null)
        {
            IEnumerable<Manager> managers;
            if (string.IsNullOrWhiteSpace(search))
            {
                managers = await _manageRepository.GetMany();
                return managers;
            }
            search = search.ToLowerInvariant();
            managers = await _manageRepository.GetMany(
                x =>
                    x.Name.First.ToLower().StartsWith(search) ||
                    x.Name.Middle.ToLower().StartsWith(search) ||
                    x.Name.Last.ToLower().StartsWith(search)
                );
            return managers;
        }
    }
}