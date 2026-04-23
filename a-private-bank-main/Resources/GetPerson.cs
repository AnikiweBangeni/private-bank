using a_private_bank_main.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a_private_bank_main.Contracts;

namespace a_private_bank_main.Resources
{
    public class GetPerson : IPersonResourceContracts
    {
        private readonly AprivateBankContext _dbContext;

        public GetPerson(AprivateBankContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Person>> GetPersonAsync(int Id)
        {

            var personFromDb = await _dbContext.People.Select(x => new Person
            {

                PersonId = x.PersonId,
                FullName = x.FullName,
                Email = x.Email

            }).Where(x => x.PersonId == Id).ToListAsync();

            if (personFromDb == null)
            {
                throw new Exception("Person not found");
            }

            return personFromDb;


        }

        public async Task<List<Person>> GetPeopleAsync( )
        {

            var personFromDb = await _dbContext.People.Select(x => new Person
            {

                PersonId = x.PersonId,
                FullName = x.FullName,
                Email = x.Email

            }).ToListAsync();

            if (personFromDb == null)
            {
                throw new Exception("Person not found");
            }

            return personFromDb;


        }

    }
}
