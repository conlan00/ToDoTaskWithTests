using Microsoft.EntityFrameworkCore;
using System.Data;
using ToDo.Data;

namespace ToDo.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DataDbContext _dbContext;
        //dependency injection
        public TaskRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // add Task to do 
        public async Task<bool> AddTask(Models.Task newTask)
        {
            var isExistingTask = await _dbContext.Task.AnyAsync(i => i.Id == newTask.Id);
            //add task if not exist
            if (!isExistingTask)
            {
                // id is set as autoincrement so i dont have process id 

                //add task to MariaDB using database instance
                _dbContext.Task.Add(newTask);
                //save changes
                await _dbContext.SaveChangesAsync();
                //return true if operation done
                return true;
            }
            else
            {
                //task exist
                return false;
            }
        }
        // get all tasks to do 
        public async Task<IEnumerable<Models.Task>> GetAllTasks()
        {
            return await _dbContext.Task.ToListAsync();
        }
        //get task by Id
        public async Task<Models.Task?> GetTaskById(int id)
        {
            
            return await _dbContext.Task.FindAsync(id);
        }
        //delete task by id 
        public async Task<bool> DeleteTask(int id)
        {
            //find element in db 
            var isExistingTask = await _dbContext.Task.FindAsync(id);
            //if exist 
            if(isExistingTask!=null)
            {
                _dbContext.Task.Remove(isExistingTask);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        //update task
        public async Task<bool> UpdateTask(Models.Task model)
        {
            _dbContext.Task.Update(model);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        //get incoming to do for today 
        public async Task<List<Models.Task>> GetTaskForToday()
        {
            var today = DateTime.Today;
            //using Orm to get task using Date
            return await _dbContext.Task
                .Where(i => i.ExpiryDateTime.Date == today)
                .ToListAsync();
        }
        //get incoming to do for next day
        public async Task<List<Models.Task>> GetTaskForNextDay()
        {
            var nextDay = DateTime.Today.AddDays(1);
            return await _dbContext.Task
                .Where(i => i.ExpiryDateTime.Date == nextDay)
                .ToListAsync();
        }
        //get incoming to do for current week
        public async Task<List<Models.Task>> GetTaskForCurrentWeek()
        {
            #region How works enum DayOfWeek in C#
            /*
             public enum DayOfWeek
                 {
                     Sunday = 0,
                     Monday = 1,
                     Tuesday = 2,
                     Wednesday = 3,
                     Thursday = 4,
                     Friday = 5,
                     Saturday = 6
                 }
             }*/
            #endregion
            var today = DateTime.Today;
            //convert day of week as integer value and substract from today
            var firstDayOfCurrentWeek=today.AddDays(-(int)today.DayOfWeek);//Sunday
            //first day of week is sunday
            //create last day of week - add 7 days to first day of week
            var lastDayOfCurrentWeek = firstDayOfCurrentWeek.AddDays(6);//Saturday
            return await _dbContext.Task
                .Where(i=>i.ExpiryDateTime.Date>=firstDayOfCurrentWeek && i.ExpiryDateTime.Date<=lastDayOfCurrentWeek)
                .ToListAsync();
        }
        //set to do percent complete
        public async Task<bool> SetTaskPercentComplete(int percentComplete, Models.Task model)
        {
            // set precent only upper than actual value
            if (model.PercentComplete < percentComplete)
            {
                model.PercentComplete = percentComplete;
               return await UpdateTask(model);
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> MarkTaskAsDone(Models.Task model)
        {
            //if precent value lower then 100. 100 = done
            if (model.PercentComplete < 100)
            {
                model.PercentComplete = 100;
                return await UpdateTask(model);
            }
            else
            {
                return false;
            }
        }
    }
}
