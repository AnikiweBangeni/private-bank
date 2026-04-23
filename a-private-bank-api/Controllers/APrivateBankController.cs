using a_private_bank_main.Contracts;
using a_private_bank_main.Models;
using a_private_bank_main.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace a_private_bank_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APrivateBankController : ControllerBase
    {
        private readonly IDataResourceContracts _dataResourceContracts;
        private readonly IPersonResourceContracts _personResourceContracts;
        private readonly IIncomeResourceContract _incomeResourceContract;

        public APrivateBankController(IDataResourceContracts dataResourceContracts, IPersonResourceContracts personResourceContracts, IIncomeResourceContract incomeResourceContract)
        {
            _dataResourceContracts = dataResourceContracts;
            _personResourceContracts = personResourceContracts;
            _incomeResourceContract = incomeResourceContract;
        }

        [HttpGet("GetPersonAsync")]
        public async Task<IActionResult> GetPersonAsync(int personId )
        {
            var results = await _personResourceContracts.GetPersonAsync(personId);

            return Ok(results);
        }

        [HttpGet("ImportDataAsync")]
        public async Task<IActionResult> ImportData()
        {
            var resuls = await _dataResourceContracts.ImportDataDocAsync();

            return Ok(resuls);
        }

        [HttpPost("PostImportedDataToDbAsync")]
        public async Task<bool> PostImportedDataToDbAsync()
        {
            var resuls = await _dataResourceContracts.PostImportedDataToDbAsync();
            return resuls;
        }   


        [HttpGet("GetPeopleAsync")]
        public async Task<IActionResult> GetPeopleAsync()
        {
            var people = await _personResourceContracts.GetPeopleAsync();
            return Ok(people);
        }

        [HttpGet("GetCycleIncomeAsync")]
        public async Task<IActionResult> GetCycleIncomeAsync( )
        {
            var cycleIncome = await _incomeResourceContract.GetCycleIncomeAsync();
            return Ok(cycleIncome);
        }
    }
}
