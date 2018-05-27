using Film.Controllers;
using Film.Data;
using Film.Models;
using FilmTests.Stubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmTests.Mocks;

namespace FilmTests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private Mock<DbContextOptions<ApplicationDbContext>> dbContextShota;
        private Mock<ApplicationDbContext> applicationDbContext;
        private UserManager<ApplicationUser> userManager;
        private HomeController controller;

        public HomeControllerTests()
        {
            var appDbContext = new Mock<ApplicationDbContext>();
            var ddbContextShota = new DbContextOptions<ApplicationDbContext>();
            applicationDbContext = new Mock<ApplicationDbContext>(ddbContextShota);

            userManager = CreateUserManager();
            controller = new HomeController(applicationDbContext.Object, userManager);
        }

        public static UserManager<ApplicationUser> CreateUserManager(IUserStore<ApplicationUser> store = null)
        {
            store = store ?? new Mock<IUserStore<ApplicationUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<ApplicationUser>>();
            var validator = new Mock<IUserValidator<ApplicationUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<ApplicationUser>>();
            pwdValidators.Add(new PasswordValidator<ApplicationUser>());
            var userManager = new UserManagerStub(store, options.Object, new PasswordHasher<ApplicationUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<ApplicationUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            return userManager;
        }

        [TestMethod]
        public void DeleteCommentConfirmed_ParamasOk_FilmExists()
        {
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Body = "To Delete"
                }
            };

            applicationDbContext.Setup(x => x.Comments).ReturnsDbSet(comments);

            // Act
        }

        // [Fact]
        [TestMethod]
        public void Can_View_Films_Correctly()
        {
            ICollection<CategoryFilm> filmCategories = new List<CategoryFilm>
            {
                new CategoryFilm
                {
                    CategoryId = 1,
                    FilmId = 1
                }
            };

            var films = new List<Movie>
            {
                new Movie
                {
                   Id = 1,
                   Name = "Some",
                }
            };

            var categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Action",
                }
            };

            filmCategories.First().Category = categories.First();
            filmCategories.First().Name = films.First();
            filmCategories.First().Film = films.First();

            films.First().Categories = filmCategories;
            categories.First().Films = filmCategories;

            applicationDbContext.Setup(x => x.Films).ReturnsDbSet(films);
            applicationDbContext.Setup(x => x.Categories).ReturnsDbSet(categories);
            // Arrange
            /*var mock = new Mock<List<Movie>>();
            mock.Setup(x => x).Returns(new List<Movie>()
                {
                    new Movie{Id=1,Name="Film1"},
                    new Movie{Id=2,Name="Film2"}
                });*/
            // Act

            var result = ((List<Movie>)(((ViewResult)controller.Index(
                selectedCategories: new string[] { "Action" })).Model));

            Xunit.Assert.Equal(1, result.Count);
        }
    }
}
