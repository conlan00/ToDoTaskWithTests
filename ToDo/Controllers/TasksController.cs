using Microsoft.AspNetCore.Mvc;
using ToDo.Repositories;
using ToDo.Services;

namespace ToDo.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // get all tasks
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        //get specific task
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task==null)
            {
                return NotFound($"Task with id {id} not found.");
            }
            return Ok(task);
        }

        //add new task
        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskService.AddTaskAsync(task);
            return Ok();
        }

        // update task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTask = await _taskService.UpdateTaskAsync(id, task);
            if (updatedTask == null)
            {
                return NotFound($"Task with id {id} not found.");
            }
            return Ok(updatedTask);
        }

        // delete task
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var success = await _taskService.DeleteTaskAsync(id);
            if (!success)
            {
                return NotFound($"Task with id {id} not found.");
            }
            return NoContent(); // Zgodnie z REST, gdy usuwamy zasób, zwracamy 204 No Content
        }

        // set percent value
        [HttpPut("{id}/percentComplete")]
        public async Task<IActionResult> SetTaskPercentComplete(int id, [FromBody] int percentComplete)
        {
            var success = await _taskService.SetTaskPercentCompleteAsync(id, percentComplete);
            if (!success)
            {
                return NotFound($"Task with id {id} not found or bad value");
            }
            return Ok();
        }

        // mark task as done
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> MarkTaskAsDone(int id)
        {
            var success = await _taskService.MarkTaskAsDoneAsync(id);
            if (!success)
            {
                return BadRequest($"Task with id {id} not found or is done");
            }
            return Ok();
        }
        //tasks for today
       [HttpGet("getTaskForToday")]
        public async Task<IActionResult> GetTaskForToday()
        {
            var tasks = await _taskService.GetTasksForTodayAsync();
            if (tasks.Any())
            {
                return Ok(tasks);
            }
            else
            {
                return NotFound("Brak zadań na dzisiaj");
            }
        }

        // tasks for the next day
        [HttpGet("getTaskForNextDay")]
        public async Task<IActionResult> GetTaskForNextDay()
        {
            var tasks = await _taskService.GetTasksForNextDayAsync();
            if (tasks.Any())
            {
                return Ok(tasks);
            }
            else
            {
                return NotFound("Brak zadań na następny dzień");
            }
        }

        // tasks for current week
        [HttpGet("getTaskForCurrentWeek")]
        public async Task<IActionResult> GetTaskForCurrentWeek()
        {
            var tasks = await _taskService.GetTasksForCurrentWeekAsync();
            if (tasks.Any())
            {
                return Ok(tasks);
            }
            else
            {
                return NotFound("Brak zadań w tym tygodniu");
            }
        }
    }
}
