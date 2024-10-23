﻿using Microsoft.AspNetCore.Mvc;

namespace ToDo.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Models.Task>> GetAllTasks();
        Task<bool> AddTask(Models.Task newTask);
        Task<Models.Task?> GetTaskById(int id);
        Task<bool> DeleteTask(int id);
        Task<bool> UpdateTask(Models.Task model);
        Task<List<Models.Task>> GetTaskForToday();
        Task<List<Models.Task>> GetTaskForNextDay();
        Task<List<Models.Task>> GetTaskForCurrentWeek();
        Task<bool> SetTaskPercentComplete(int percentComplete, Models.Task model);
        Task<bool> MarkTaskAsDone(Models.Task model);
    }
}
