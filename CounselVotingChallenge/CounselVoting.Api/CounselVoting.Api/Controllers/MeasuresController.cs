using CounselVoting.Domain.Enum;
using CounselVoting.Domain.Model;
using CounselVoting.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CounselVoting.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MeasuresController : ControllerBase
    {
        private readonly ILogger<MeasuresController> _logger;
        private readonly IMeasureService _service;
        private readonly IDateTimeService _dateTime;

        public MeasuresController(
            ILogger<MeasuresController> logger,
            IMeasureService service,
            IDateTimeService dateTime)
        {
            _logger = logger;
            _service = service;
            _dateTime = dateTime;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<Measure> Post([FromBody] CreateMeasureRequest model)
        {
            var measure = new Measure
            {
                Subject = model.Subject,
                Description = model.Description,
                Status = MeasureStatus.Open
            };

            return await _service.InsertAsync(measure);
        }

        [HttpPut("{measureId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<Measure> Put(int measureId, [FromBody] Measure model)
        {
            return await _service.UpdateAsync(measureId, model);
        }

        [HttpDelete("{measureId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<Measure> Delete(int measureId)
        {
            return await _service.DeleteAsync(measureId);
        }

        [HttpGet("{measureId:int}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get(int measureId)
        {
            var measure = await _service.GetAsync(measureId);
            if (measure == null)
            {
                return NotFound();
            }
                
            return Ok(measure);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Measure>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Measure>>> GetMeasuresAsync()
        {
            var measures = await _service.GetAllAsync();

            return Ok(measures);
        }

        [HttpPost("vote")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<bool> Vote([FromBody] VoteMeasureRequest model)
        {
            var vote = new MeasureVote
            {
                MeasureId = model.MeasureId,
                Name = model.Name,
                VoteChoice = model.VoteChoice,
                VoteDate = _dateTime.Now
            };

            return await _service.VoteAsync(vote);
        }
    }

    public record CreateMeasureRequest(string Subject, string Description);
    public record VoteMeasureRequest(int MeasureId, string Name, VoteChoice VoteChoice);
}
