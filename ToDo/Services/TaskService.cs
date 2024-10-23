using ToDo.Repositories;

namespace ToDo.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllTasks();
        }

        public async Task<Models.Task?> GetTaskByIdAsync(int id)
        {
            return await _taskRepository.GetTaskById(id);
        }

        public async Task<bool> AddTaskAsync(Models.Task task)
        {
            return await _taskRepository.AddTask(task);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteTask(id);
        }

        public async Task<Models.Task?> UpdateTaskAsync(int id, Models.Task task)
        {
            var existingTask = await _taskRepository.GetTaskById(id);
            if (existingTask == null) return null;

            // update
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.ExpiryDateTime = task.ExpiryDateTime;

            await _taskRepository.UpdateTask(existingTask);
            return existingTask;
        }

        public async Task<bool> SetTaskPercentCompleteAsync(int id, int percentComplete)
        {
            var existTask = await _taskRepository.GetTaskById(id);
            if (existTask == null) return false;
            var success = await _taskRepository.SetTaskPercentComplete(percentComplete, existTask);
            return success;
        }

        public async Task<bool> MarkTaskAsDoneAsync(int id)
        {
            var existTask = await _taskRepository.GetTaskById(id);
            if (existTask == null) return false;
            var success = await _taskRepository.MarkTaskAsDone(existTask);
            return success;
        }
        public async Task<List<Models.Task>> GetTasksForTodayAsync()
        {
            return await _taskRepository.GetTaskForToday();
        }

        public async Task<List<Models.Task>> GetTasksForNextDayAsync()
        {
            return await _taskRepository.GetTaskForNextDay();
        }

        public async Task<List<Models.Task>> GetTasksForCurrentWeekAsync()
        {
            return await _taskRepository.GetTaskForCurrentWeek();
        }
    }
}
