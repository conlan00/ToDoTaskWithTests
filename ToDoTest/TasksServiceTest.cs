using Moq;
using ToDo.Repositories;
using ToDo.Services;
namespace Tests
{
    public class TasksServiceTest
    {
        private Mock<ITaskRepository> _taskRepositoryMock;
        private readonly ITaskService _taskService;
        public TasksServiceTest()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnTasks()
        {
            var mockTasks = new List<ToDo.Models.Task>
            {
            new ToDo.Models.Task { Id = 1, ExpiryDateTime = DateTime.Parse("2024-10-22T20:59:06.893Z", null, System.Globalization.DateTimeStyles.RoundtripKind), Title = "New title" ,Description="desc",PercentComplete=20},
            new ToDo.Models.Task {Id = 2,ExpiryDateTime = DateTime.Parse("2024-10-22T20:59:06.893Z", null, System.Globalization.DateTimeStyles.RoundtripKind), Title = "New title2" ,Description="desc",PercentComplete=24}
            };

            //config mock  repo
            _taskRepositoryMock.Setup(repo => repo.GetAllTasks())
                               .ReturnsAsync(mockTasks);

            // run method
            var result = await _taskService.GetAllTasksAsync();

            // check first task
            Assert.Equal(1, mockTasks[0].Id);
            Assert.Equal("New title", mockTasks[0].Title);
            Assert.Equal("desc", mockTasks[0].Description);
            Assert.Equal(20, mockTasks[0].PercentComplete);
            Assert.Equal(DateTime.Parse("2024-10-22T20:59:06.893Z", null, System.Globalization.DateTimeStyles.RoundtripKind), mockTasks[0].ExpiryDateTime);

            // check second task
            Assert.Equal(2, mockTasks[1].Id);
            Assert.Equal("New title2", mockTasks[1].Title);
            Assert.Equal("desc", mockTasks[1].Description);
            Assert.Equal(24, mockTasks[1].PercentComplete);
            Assert.Equal(DateTime.Parse("2024-10-22T20:59:06.893Z", null, System.Globalization.DateTimeStyles.RoundtripKind), mockTasks[1].ExpiryDateTime);
        }


        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExist()
        {
            var mockTask = new ToDo.Models.Task
            {
                Id = 1,
                ExpiryDateTime = DateTime.Parse("2024-10-22T20:59:06.893Z", null, System.Globalization.DateTimeStyles.RoundtripKind),
                Title = "New title",
                Description = "desc",
                PercentComplete = 20
            };

            //for return task with id 1
            _taskRepositoryMock.Setup(repo => repo.GetTaskById(1))
                               .ReturnsAsync(mockTask);

            var result = await _taskService.GetTaskByIdAsync(1);

            // check
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("New title", result.Title);
            Assert.Equal("desc", result.Description);
            Assert.Equal(20, result.PercentComplete);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            //return task which not exist
            _taskRepositoryMock.Setup(repo => repo.GetTaskById(2))
                               .ReturnsAsync((ToDo.Models.Task?)null);

            var result = await _taskService.GetTaskByIdAsync(2);

            // is null
            Assert.Null(result);
        }

        [Fact]
        public async Task AddTaskAsync_ShouldAddTaskSuccessfully()
        {

            var newTask = new ToDo.Models.Task
            {
                Id = 1,
                ExpiryDateTime = DateTime.Parse("2024-10-22T20:59:06.893Z", null, System.Globalization.DateTimeStyles.RoundtripKind),
                Title = "New title",
                Description = "desc",
                PercentComplete = 20
            };

            // mock config 
            _taskRepositoryMock.Setup(repo => repo.AddTask(newTask))
                               .ReturnsAsync(true);


            var result = await _taskService.AddTaskAsync(newTask);

            // Assert
            Assert.True(result); //check
                                 //check if the method executed once
            _taskRepositoryMock.Verify(repo => repo.AddTask(newTask), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldDeleteTask_WhenIdIsValid()
        {
            int validTaskId = 1;

            // mock config 
            _taskRepositoryMock.Setup(repo => repo.DeleteTask(validTaskId))
                               .ReturnsAsync(true);


            var result = await _taskService.DeleteTaskAsync(validTaskId);

            // Assert
            Assert.True(result);
            _taskRepositoryMock.Verify(repo => repo.DeleteTask(validTaskId), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldReturnFalse_WhenIdIsInvalid()
        {

            int invalidTaskId = 2;


            _taskRepositoryMock.Setup(repo => repo.DeleteTask(invalidTaskId))
                               .ReturnsAsync(false);


            var result = await _taskService.DeleteTaskAsync(invalidTaskId);

            // Assert
            Assert.False(result);
            _taskRepositoryMock.Verify(repo => repo.DeleteTask(invalidTaskId), Times.Once);
        }


        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTaskSuccessfully()
        {
            // Arrange
            int taskId = 1;
            var updatedTask = new ToDo.Models.Task
            {
                Id = taskId,
                ExpiryDateTime = DateTime.Parse("2024-10-22T20:59:06.893Z", null, System.Globalization.DateTimeStyles.RoundtripKind),
                Title = "New title",
                Description = "desc",
                PercentComplete = 20
            };
            _taskRepositoryMock.Setup(repo => repo.GetTaskById(taskId)).ReturnsAsync(updatedTask);
            _taskRepositoryMock.Setup(repo => repo.UpdateTask(updatedTask)).ReturnsAsync(true);


            var result = await _taskService.UpdateTaskAsync(taskId, updatedTask);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedTask.Title, result.Title);
            _taskRepositoryMock.Verify(repo => repo.UpdateTask(updatedTask), Times.Once);
        }

        [Fact]
        public async Task SetTaskPercentCompleteAsync_ShouldSetPercentSuccessfully()
        {
            int taskId = 1;
            int percentComplete = 50;
            var updatedTask = new ToDo.Models.Task
            {
                Id = taskId,
                ExpiryDateTime = DateTime.Parse("2024-10-22T20:59:06.893Z", null, System.Globalization.DateTimeStyles.RoundtripKind),
                Title = "New title",
                Description = "desc",
                PercentComplete = 20
            };
            _taskRepositoryMock.Setup(repo => repo.GetTaskById(taskId)).ReturnsAsync(updatedTask);
            _taskRepositoryMock.Setup(repo => repo.SetTaskPercentComplete(percentComplete, updatedTask)).ReturnsAsync(true);


            var result = await _taskService.SetTaskPercentCompleteAsync(taskId, percentComplete);

            // Assert
            Assert.True(result);
            _taskRepositoryMock.Verify(repo => repo.SetTaskPercentComplete(percentComplete, updatedTask), Times.Once);
        }

        [Fact]
        public async Task MarkTaskAsDoneAsync_ShouldMarkAsDoneSuccessfully()
        {
            int taskId = 1;
            var updatedTask = new ToDo.Models.Task
            {
                Id = taskId,
                ExpiryDateTime = DateTime.Parse("2024-10-22T20:59:06.893Z", null, System.Globalization.DateTimeStyles.RoundtripKind),
                Title = "New title",
                Description = "desc",
                PercentComplete = 20
            };
            _taskRepositoryMock.Setup(repo => repo.GetTaskById(taskId)).ReturnsAsync(updatedTask);
            _taskRepositoryMock.Setup(repo => repo.MarkTaskAsDone(updatedTask)).ReturnsAsync(true);


            var result = await _taskService.MarkTaskAsDoneAsync(taskId);

            // Assert
            Assert.True(result);
            _taskRepositoryMock.Verify(repo => repo.MarkTaskAsDone(updatedTask), Times.Once);
        }

        [Fact]
        public async Task GetTasksForTodayAsync_ShouldReturnTodayTasks()
        {
            var todayTasks = new List<ToDo.Models.Task>
            {
            new ToDo.Models.Task { Id = 1, ExpiryDateTime = DateTime.Today, Title = "New title" ,Description="desc",PercentComplete=20},
            new ToDo.Models.Task {Id = 2,ExpiryDateTime = DateTime.Today, Title = "New title2" ,Description="desc",PercentComplete=24}
            };

            _taskRepositoryMock.Setup(repo => repo.GetTaskForToday()).ReturnsAsync(todayTasks);


            var result = await _taskService.GetTasksForTodayAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            _taskRepositoryMock.Verify(repo => repo.GetTaskForToday(), Times.Once);
        }

        [Fact]
        public async Task GetTasksForNextDayAsync_ShouldReturnNextDayTasks()
        {
            var nextDayTasks = new List<ToDo.Models.Task>
            {
            new ToDo.Models.Task { Id = 1, ExpiryDateTime = DateTime.Today.AddDays(1), Title = "New title" ,Description="desc",PercentComplete=20},
            new ToDo.Models.Task {Id = 2,ExpiryDateTime = DateTime.Today.AddDays(1), Title = "New title2" ,Description="desc",PercentComplete=24}
            };

            _taskRepositoryMock.Setup(repo => repo.GetTaskForNextDay()).ReturnsAsync(nextDayTasks);

            var result = await _taskService.GetTasksForNextDayAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            _taskRepositoryMock.Verify(repo => repo.GetTaskForNextDay(), Times.Once);
        }

        [Fact]
        public async Task GetTasksForCurrentWeekAsync_ShouldReturnCurrentWeekTasks()
        {
            var currentWeekTasks = new List<ToDo.Models.Task>
            {
            new ToDo.Models.Task { Id = 1, ExpiryDateTime = DateTime.Today.AddDays(1), Title = "New title" ,Description="desc",PercentComplete=20},
            new ToDo.Models.Task {Id = 2,ExpiryDateTime = DateTime.Today.AddDays(6), Title = "New title2" ,Description="desc",PercentComplete=24},
            };

            _taskRepositoryMock.Setup(repo => repo.GetTaskForCurrentWeek()).ReturnsAsync(currentWeekTasks);

            var result = await _taskService.GetTasksForCurrentWeekAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            _taskRepositoryMock.Verify(repo => repo.GetTaskForCurrentWeek(), Times.Once);
        }
    }
}
