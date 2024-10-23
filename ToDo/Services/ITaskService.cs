namespace ToDo.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Models.Task>> GetAllTasksAsync();
        Task<Models.Task?> GetTaskByIdAsync(int id);
        Task<bool> AddTaskAsync(Models.Task task);
        Task<bool> DeleteTaskAsync(int id);
        Task<Models.Task?> UpdateTaskAsync(int id, Models.Task task);
        Task<bool> SetTaskPercentCompleteAsync(int id, int percentComplete);
        Task<bool> MarkTaskAsDoneAsync(int id);
        Task<List<Models.Task>> GetTasksForTodayAsync();
        Task<List<Models.Task>> GetTasksForNextDayAsync();
        Task<List<Models.Task>> GetTasksForCurrentWeekAsync();
    }
}
