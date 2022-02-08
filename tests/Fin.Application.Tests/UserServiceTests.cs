using Fin.Application.DTOs;
using Fin.Application.Services;
using Fin.Application.ViewModels;
using Fin.Domain.Entities;
using Fin.Domain.Exceptions;
using Fin.Domain.Tests;
using Fin.Infrastructure.Repositories;
using Fin.Infrastructure.Tests;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Fin.Application.Tests
{
    public class UserServiceTests : IClassFixture<InMemoryTestFixture>
    {
        public readonly InMemoryTestFixture fixture;
        public readonly UnitOfWork unitOfWork;
        public readonly UserService userService;
        public readonly TokenService tokenService;

        public UserServiceTests(InMemoryTestFixture dbContext)
        {
            this.fixture = dbContext;
            this.unitOfWork = new UnitOfWork(fixture.DbContext);
            this.tokenService = new TokenService();
            this.userService = new UserService(unitOfWork, tokenService);
        }

        [Fact]
        public async Task GetUserByIdAsync_Returns_User()
        {
            // Act
            var user = await userService.GetUserByIdAsync(MockData.UserA.Id);

            // Assert
            Assert.NotNull(user);
            Assert.IsAssignableFrom<UserResponse>(user);
        }

        [Fact]
        public async Task GetUserByIdAsync_Throws_NotFoundException()
        {
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await userService.GetUserByIdAsync(new Guid()));
        }

        [Fact]
        public async Task GetUserByEmailAsync_Returns_User()
        {
            // Act
            var user = await userService.GetUserByEmailAsync(MockData.UserB.Email);

            // Assert
            Assert.NotNull(user);
            Assert.IsAssignableFrom<UserResponse>(user);
        }

        [Fact]
        public async Task GetUserByEmailAsync_Throws_NotFoundException()
        {
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await userService.GetUserByEmailAsync("wrong@example.com"));
        }

        [Fact]
        public async Task GetAllByUserIdAsync_Returns_Empty()
        {
            // Act
            var users = await userService.GetAllAsync();

            // Assert
            Assert.NotNull(users);
            Assert.NotEmpty(users);
        }

        [Fact]
        public async Task SignupAsync_Returns_User()
        {
            // Arrange
            SignupRequest signupRequest = new SignupRequest()
            {
                Name = "Test",
                Username = "test",
                Email = "mail@example.com",
                Password = "pass1234",
                ConfirmPassword = "pass1234"
            };

            // Act
            var user = await userService.SignupAsync(signupRequest);

            // Assert
            Assert.NotNull(user);
            Assert.IsAssignableFrom<UserResponse>(user);
        }

        [Fact]
        public async Task SignupAsync_Throws_BadRequestException_EmailAlreadyInUse()
        {
            // Arrange
            SignupRequest signupRequest = new SignupRequest()
            {
                Name = "Test",
                Username = "test",
                Email = "aaa@example.com",
                Password = "pass1234",
                ConfirmPassword = "pass1234"
            };

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await userService.SignupAsync(signupRequest));
        }

        [Fact]
        public async Task SignupAsync_Throws_BadRequestException_PasswordMismatch()
        {
            // Arrange
            SignupRequest signupRequest = new SignupRequest()
            {
                Name = "Test",
                Username = "test",
                Email = "etc@example.com",
                Password = "pass1234",
                ConfirmPassword = "pass4321"
            };

            // Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await userService.SignupAsync(signupRequest));
        }
    }
}
