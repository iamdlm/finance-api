using Fin.Domain.Entities;
using Fin.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fin.Infrastructure.Tests
{
    public class UserRepositoryTests : IClassFixture<InMemoryTestFixture>
    {
        public readonly InMemoryTestFixture fixture;
        public readonly UserRepository userRepository;

        public UserRepositoryTests(InMemoryTestFixture dbContext)
        {
            this.fixture = dbContext;
            this.userRepository = new UserRepository(fixture.DbContext);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_User()
        {
            // Act
            User user = await userRepository.GetByIdAsync(new Guid("7ab39994-e375-450d-851d-3dc92a7e1fad"));

            // Assert  
            Assert.NotNull(user);
            Assert.IsAssignableFrom<User>(user);
        }

        [Fact]
        public async Task GetByIdAsyncWithIncludes_Returns_UserWithPortfolio()
        {
            // Act
            User user = await userRepository.GetByIdAsync(new Guid("7ab39994-e375-450d-851d-3dc92a7e1fad"), u => u.Portfolios);

            // Assert  
            Assert.NotNull(user);
            Assert.NotNull(user.Portfolios);
            Assert.IsAssignableFrom<User>(user);
        }

        [Fact]
        public async Task GetByIdAsync_Returns_Null()
        {
            // Act
            User user = await userRepository.GetByIdAsync(Guid.NewGuid());

            // Assert  
            Assert.Null(user);
        }

        [Fact]
        public async Task GetByEmailAsync_Returns_User()
        {
            // Act            
            User user = await userRepository.GetByEmailAsync("bbb@example.com");

            // Assert  
            Assert.NotNull(user);
            Assert.IsAssignableFrom<User>(user);
        }

        [Fact]
        public async Task GetByEmailAsync_Returns_Null()
        {
            // Act            
            User user = await userRepository.GetByEmailAsync("wrong@mail.com");

            // Assert  
            Assert.Null(user);
        }

        [Fact]
        public async Task GetAllAsync_Returns_Users()
        {
            // Act
            IEnumerable<User> users = await userRepository.GetAllAsync();

            // Assert  
            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.Equal(2, users.ToList().Count);
        }

        [Fact]
        public async Task GetAllWithExpressionAsync_Returns_Users()
        {
            // Act
            IEnumerable<User> users =
                await userRepository.GetAllAsync(u => u.Email == "aaa@example.com");

            // Assert  
            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.Single(users.ToList());
        }

        [Fact]
        public async Task GetAllWithExpressionAsync_Returns_Empty()
        {
            // Act
            IEnumerable<User> users = await userRepository.GetAllAsync(u => u.Email == "wrong@mail.com");

            // Assert  
            Assert.NotNull(users);
            Assert.Empty(users);
        }

        [Fact]
        public async Task Add_Increases_Count()
        {
            // Arrange
            User user = new User()
            {
                Id = new Guid(),
                Name = "CCC",
                Username = "ccc",
                Email = "ccc@example.com"
            };

            // Act
            IEnumerable<User> usersBeforeAdd = await userRepository.GetAllAsync();
            userRepository.Add(user);
            fixture.DbContext.SaveChanges();
            IEnumerable<User> usersAfterAdd = await userRepository.GetAllAsync();

            // Assert  
            Assert.Equal(usersBeforeAdd.ToList().Count + 1, usersAfterAdd.ToList().Count);
        }

        [Fact]
        public async Task Delete_Decreases_Count()
        {
            // Act
            IEnumerable<User> usersBeforeAdd = await userRepository.GetAllAsync();
            User user = await userRepository.GetByIdAsync(new Guid("7ab39994-e375-450d-851d-3dc92a7e1fad"));
            userRepository.Delete(user);
            fixture.DbContext.SaveChanges();
            IEnumerable<User> usersAfterAdd = await userRepository.GetAllAsync();

            // Assert  
            Assert.NotEmpty(usersBeforeAdd);
            Assert.NotEmpty(usersAfterAdd);
            Assert.NotNull(user);
            Assert.Equal(usersBeforeAdd.ToList().Count - 1, usersAfterAdd.ToList().Count);
        }

        [Fact]
        public async Task Update_Returns_User()
        {
            // Act
            User userBeforeUpdate = await userRepository.GetByIdAsync(new Guid("7ab39994-e375-450d-851d-3dc92a7e1fad"));
            userBeforeUpdate.Email = "updated@example.com";
            userRepository.Update(userBeforeUpdate);
            fixture.DbContext.SaveChanges();
            User userAfterUpdate = await userRepository.GetByIdAsync(new Guid("7ab39994-e375-450d-851d-3dc92a7e1fad"));

            // Assert  
            Assert.NotNull(userBeforeUpdate);
            Assert.NotNull(userAfterUpdate);
            Assert.Equal("updated@example.com", userAfterUpdate.Email);
        }
    }
}
