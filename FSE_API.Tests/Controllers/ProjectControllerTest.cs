using FSE_API.Controllers;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FSE_API.DBContext;
using FSE_API.Repository;
using System.Collections.Generic;
using Moq;
using System.Linq;
using System.Web.Http.Results;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using FSE_API.Models;

namespace FSE_API.Tests.Controllers
{
    [TestClass()]
    public class ProjectControllerTest
    {

        List<Project> _randomProjects = null;
        List<ProjectModel> _randomProjectModel = null;
        IProjectRepository projectRepository = null;    

        public ProjectControllerTest()
        {
            _randomProjects = SetupProjects();
            _randomProjectModel = SetupProjectModel();
            projectRepository = SetupTaskRepository();
        }

        public List<Project> SetupProjects()
        {
            var projects = new List<Project>();
            projects.Add(new Project
            {
                Project_ID = 1,
                Project1 = "Test",
                Start_Date = DateTime.Now,
                End_Date = DateTime.Now,
                Priority = 1
            });
            //tasks.Add(new Task { Task_ID = 2 });
            //tasks.Add(new Task { Task_ID = 3 });
            //tasks.Add(new Task { Task_ID = 4 });

            return projects;

        }

        public List<ProjectModel> SetupProjectModel()
        {
            var projects = new List<ProjectModel>();
            projects.Add(new ProjectModel()
            {
                Project_ID = 1,
                Project_Name = "Test",
                Start_Date = DateTime.Now,
                End_Date = DateTime.Now,
                Priority = 1,
                NoOfTasks = 1,
                CompletedTask = 1

            });
            //tasks.Add(new Task { Task_ID = 2 });
            //tasks.Add(new Task { Task_ID = 3 });
            //tasks.Add(new Task { Task_ID = 4 });

            return projects;



        }

        public IProjectRepository SetupTaskRepository()
        {
            // Init repository
            var repo = new Mock<IProjectRepository>();

            //var mockRepository = new Mock<ITasksRepository>();
            //repo.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });
            // Setup mocking behavior
            repo.Setup(r => r.Get()).Returns(_randomProjects);

            repo.Setup(r => r.GetProject(It.IsAny<int>()))
                .Returns(new Func<int, Project>(
                    id => _randomProjects.Find(a => a.Project_ID.Equals(id))));

            repo.Setup(r => r.Post(It.IsAny<Project>()))
                .Callback(new Action<Project>(newArticle =>
                {
                    dynamic maxArticleID = _randomProjects.Last().Project_ID;
                    dynamic nextArticleID = maxArticleID + 1;
                    newArticle.Project_ID = nextArticleID;
                    //newArticle.Start_Date = DateTime.Now;
                    _randomProjects.Add(newArticle);
                }));

            repo.Setup(r => r.Put(1, It.IsAny<Project>()))
                .Callback<int, Project>((o, x) =>
                {
                    var oldArticle = _randomProjects.Find(a => a.Project_ID == 1);
                    _randomProjects.Find(a => a.Project_ID == 1).Priority = x.Priority;
                    oldArticle = x;
                });


            //repo.Setup(r => r.Put(1, It.IsAny<Task>()))
            //    .Callback<int, Task>(new Action<int,Task>((1,x) =>
            //    {
            //        var oldArticle = _randomArticles.Find(a => a.Task_ID == 1);
            //        //oldArticle.Start_Date = DateTime.Now;
            //        oldArticle = x;
            //    }));

            repo.Setup(r => r.Delete(1))
                .Callback(new Action<int>(x =>
                {
                    var _articleToRemove = _randomProjects.Find(a => a.Project_ID == x);

                    if (_articleToRemove != null)
                        _randomProjects.Remove(_articleToRemove);
                }));

            // Return mock implementation
            return repo.Object;
        }

        [TestMethod()]
        public void GetProjectTest()
        {

            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            var controller = new ProjectController(projectRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get();

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            //var responseString = response.Content.ReadAsStringAsync().Result;
            //// Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            //Assert.AreEqual(1, status);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);

        }

        [TestMethod()]
        public void GetProjectByIDTest()
        {
            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            var controller = new ProjectController(projectRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetProject(1);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            dynamic jsonObject = JObject.Parse(responseString);
            int status = (int)jsonObject.Project_ID;
            Assert.AreEqual(1, status);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);

        }

        [TestMethod()]
        public void PostProjectTest()
        {
            //var mockRepository = new Mock<ITasksRepository>();
            //mockRepository.Setup(x => x.GetTask(1))
            //    .Returns(new Task { Task_ID = 1 });

            int count = _randomProjects.Count;

            var controller = new ProjectController(projectRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            Project task = new Project() { Project_ID = 2 };
            // Act
            var response = controller.Post(task);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);



            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            Assert.AreEqual(count + 1, _randomProjects.Count);
            //Assert.IsTrue(response.TryGetContentValue<Task>(out task));
            //Assert.AreEqual(10, task.Task_ID);
        }

        [TestMethod()]
        public void PutProjectTest()
        {
            var controller = new ProjectController(projectRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            Project task = new Project() { Project_ID = 1, Priority = 1 };
            // Act
            var response = controller.Put(1, task);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(_randomProjects.Find(a => a.Project_ID == 1).Priority, 1);


            var responseString = response.Content.ReadAsStringAsync().Result;
            // Assert
            //dynamic jsonObject = JObject.Parse(responseString);
            //int status = (int)jsonObject.Task_ID;
            //Assert.AreEqual(0, responseString);
        }

        [TestMethod()]
        public void DeleteProjectTest()
        {
            var controller = new ProjectController(projectRepository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            Project task = new Project() { Project_ID = 1, Priority = 1 };
            // Act
            var response = controller.Delete(1);

            Assert.IsNotNull(response.Content);
            Assert.IsNotNull(response.Content.Headers.ContentType);
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.AreEqual(_randomProjects.Count, 0);


        }
    }
}
