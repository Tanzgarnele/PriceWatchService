using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PriceWatchApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceWatchController : ControllerBase
    {
        private readonly IAlarmRepository alarmRepository;

        public PriceWatchController(IAlarmRepository alarmRepository)
        {
            this.alarmRepository = alarmRepository ?? throw new ArgumentNullException(nameof(alarmRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alarm>>> GetAll()
        {
            try
            {
                IEnumerable<Alarm> alarms = await this.alarmRepository.GetAllAsync();
                return this.Ok(alarms);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Alarm>> GetByIdAsync(Int32 id)
        {
            try
            {
                if (id <= 0)
                {
                    return this.BadRequest("Id cannot be zero or negative.");
                }

                Alarm alarm = await this.alarmRepository.GetByIdAsync(id);

                if (alarm == null)
                {
                    return this.NotFound();
                }

                return this.Ok(alarm);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Alarm>>> AddAsync(Alarm alarm)
        {
            try
            {
                if (alarm == null)
                {
                    return this.BadRequest("Alarm object cannot be null.");
                }

                await this.alarmRepository.AddAsync(alarm);

                return this.Ok(await this.alarmRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Alarm>> UpdateAsync(Int32 id, Alarm alarm)
        {
            try
            {
                if (id <= 0)
                {
                    return this.BadRequest("Id cannot be zero or negative.");
                }

                if (alarm == null)
                {
                    return this.BadRequest("Alarm object cannot be null.");
                }

                if (id != alarm.Id)
                {
                    return this.BadRequest("Id mismatch between parameter and object.");
                }

                Alarm updatedAlarm = await this.alarmRepository.UpdateAsync(alarm);

                if (updatedAlarm == null)
                {
                    return this.NotFound();
                }

                return this.Ok(updatedAlarm);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Int32 id)
        {
            try
            {
                if (id <= 0)
                {
                    return this.BadRequest("Id cannot be zero or negative.");
                }

                await this.alarmRepository.DeleteAsync(id);

                return this.NoContent();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}