using System.Collections.Generic;
using System.Threading.Tasks;
using Sorter.Models;

namespace Sorter.DataAccessLogic
{
    public interface IKeyValueRepository : IRepository<KeyValueModel>
    {
        KeyValueModel GetToken(string key);
        List<KeyValueModel> GetTokenList(string key);

    }
}