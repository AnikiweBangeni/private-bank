using a_private_bank_main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a_private_bank_main.Resources;

namespace a_private_bank_main.Contracts
{
    public interface IPersonResourceContracts
    {
        Task<List<Person>> GetPersonAsync(int Id);
        Task<List<Person>> GetPeopleAsync( );
    }
}
