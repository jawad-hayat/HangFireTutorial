using Hangfire;
using HangFireTutorial.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace HangFireTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        [HttpGet]
        [Route("FireAndForgetJob")]
        public ActionResult CreateFireAndForgetJob()
        {
            BackgroundJob.Enqueue<TestJob>(x => x.WriteLog("Fire-and-Forget Job"));
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-Forget Job"));
            return Ok();
        }

        [HttpGet]
        [Route("DelayJob")]
        public ActionResult CreateDelayJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);
            BackgroundJob.Schedule<TestJob>(x => x.WriteLog("Delay-and-Schedule Job"), dateTimeOffset);
            //BackgroundJob.Schedule(() => Console.WriteLine("Delay-and-Schedule Job"),dateTimeOffset);
            return Ok();
        }
        [HttpGet]
        [Route("ContinuationJob")]
        public ActionResult CreateContinuationJob()
        {
            var scheduleDateTime = DateTime.UtcNow.AddSeconds(5);
            var dateTimeOffset = new DateTimeOffset(scheduleDateTime);
            var jobID = BackgroundJob.Schedule<TestJob>(x => x.WriteLog("Delay-and-Schedule 2nd Job"), dateTimeOffset);
            //var jobID = BackgroundJob.Schedule(() => Console.WriteLine("Delay-and-Schedule 2nd Job"), dateTimeOffset);

            var job2ID = BackgroundJob.ContinueJobWith<TestJob>(jobID, x => x.WriteLog("ContinuationJob1 triggered"));
            var job3ID = BackgroundJob.ContinueJobWith<TestJob>(job2ID, x => x.WriteLog("ContinuationJob2 triggered"));
            var job4ID = BackgroundJob.ContinueJobWith<TestJob>(job3ID, x => x.WriteLog("ContinuationJob3 triggered"));
            return Ok();
        }
        [HttpGet]
        [Route("RecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate<TestJob>("RecurringJob1", x => x.WriteLog("RecurringJob triggered"), "* * * * *");
            return Ok();
        }
    }
}
