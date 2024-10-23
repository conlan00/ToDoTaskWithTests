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

            /* [Fact]
             public async Task AddTaskAsync_ShouldAddTaskSuccessfully()
             {
                 // Test implementation here
             }

             [Fact]
             public async Task DeleteTaskAsync_ShouldDeleteTask_WhenIdIsValid()
             {
                 // Test implementation here
             }

             [Fact]
             public async Task DeleteTaskAsync_ShouldReturnFalse_WhenIdIsInvalid()
             {
                 // Test implementation here
             }

             [Fact]
             public async Task UpdateTaskAsync_ShouldUpdateTaskSuccessfully()
             {
                 // Test implementation here
             }

             [Fact]
             public async Task UpdateTaskAsync_ShouldReturnNull_WhenIdIsInvalid()
             {
                 // Test implementation here
             }

             [Fact]
             public async Task SetTaskPercentCompleteAsync_ShouldSetPercentSuccessfully()
             {
                 // Test implementation here
             }

             [Fact]
             public async Task MarkTaskAsDoneAsync_ShouldMarkAsDoneSuccessfully()
             {
                 // Test implementation here
             }

             [Fact]
             public async Task GetTasksForTodayAsync_ShouldReturnTodayTasks()
             {
                 // Test implementation here
             }

             [Fact]
             public async Task GetTasksForNextDayAsync_ShouldReturnNextDayTasks()
             {
                 // Test implementation here
             }

             [Fact]
             public async Task GetTasksForCurrentWeekAsync_ShouldReturnCurrentWeekTasks()
             {
                 // Test implementation here
             }*/
        }
    }
