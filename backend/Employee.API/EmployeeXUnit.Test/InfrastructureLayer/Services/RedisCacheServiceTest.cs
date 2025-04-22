using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Employee.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace EmployeeXUnit.Test.InfrastructureLayer.Services
{
    public class RedisCacheServiceTests
    {
        private readonly Mock<IDistributedCache> _cacheMock;
        private readonly RedisCacheService _service;
        private const string Key = "test-key";

        public RedisCacheServiceTests()
        {
            _cacheMock = new Mock<IDistributedCache>();
            _service = new RedisCacheService(_cacheMock.Object);
        }

        // A simple DTO for testing
        private class TestData
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public async Task GetAsync_ReturnsDefault_WhenCacheMiss()
        {
            // Arrange: underlying GetAsync returns null bytes
            _cacheMock
                .Setup(c => c.GetAsync(Key, It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[])null);

            // Act
            var result = await _service.GetAsync<TestData>(Key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_DeserializesAndReturnsObject_WhenCacheHit()
        {
            // Arrange
            var expected = new TestData { Id = 42, Name = "Foo" };
            var json = JsonConvert.SerializeObject(expected);
            var bytes = Encoding.UTF8.GetBytes(json);

            _cacheMock
                .Setup(c => c.GetAsync(Key, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bytes);

            // Act
            var result = await _service.GetAsync<TestData>(Key);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Name, result.Name);
        }

        [Fact]
        public async Task SetAsync_UsesDefaultExpiration_WhenExpireTimeNotProvided()
        {
            // Arrange
            var value = new TestData { Id = 7, Name = "Bar" };
            var json = JsonConvert.SerializeObject(value);

            // We just need to make SetAsync return completed task
            _cacheMock
                .Setup(c => c.SetAsync(
                    Key,
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.SetAsync(Key, value);

            // Assert: default is 10 minutes
            _cacheMock.Verify(c => c.SetAsync(
                Key,
                It.Is<byte[]>(b => Encoding.UTF8.GetString(b) == json),
                It.Is<DistributedCacheEntryOptions>(opts =>
                    opts.AbsoluteExpirationRelativeToNow.HasValue &&
                    opts.AbsoluteExpirationRelativeToNow.Value == TimeSpan.FromMinutes(10)),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task SetAsync_UsesCustomExpiration_WhenExpireTimeProvided()
        {
            // Arrange
            var value = new TestData { Id = 8, Name = "Baz" };
            var expiration = TimeSpan.FromSeconds(30);
            var json = JsonConvert.SerializeObject(value);

            _cacheMock
                .Setup(c => c.SetAsync(
                    Key,
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.SetAsync(Key, value, expiration);

            // Assert
            _cacheMock.Verify(c => c.SetAsync(
                Key,
                It.Is<byte[]>(b => Encoding.UTF8.GetString(b) == json),
                It.Is<DistributedCacheEntryOptions>(opts =>
                    opts.AbsoluteExpirationRelativeToNow.HasValue &&
                    opts.AbsoluteExpirationRelativeToNow.Value == expiration),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_InvokesUnderlyingCacheRemove()
        {
            // Arrange
            _cacheMock
                .Setup(c => c.RemoveAsync(Key, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.RemoveAsync(Key);

            // Assert
            _cacheMock.Verify(c =>
                c.RemoveAsync(Key, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
