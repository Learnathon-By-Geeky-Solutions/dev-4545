using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Employee.API.ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace EmployeeXUnit.Test.PresentationLayer
{
    public class GlobalExceptionHandlerTests
    {
        [Fact]
        public async Task TryHandleAsync_Writes_Correct_Response_And_ReturnsFalse_DefaultStatusCode()
        {
            // Arrange
            var handler = new GlobalExceptionHandler();
            var context = new DefaultHttpContext();
            // Default status code is 200
            var exception = new Exception("Test error message");
            using var bodyStream = new MemoryStream();
            context.Response.Body = bodyStream;

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            Assert.False(result);
            // Read response body
            bodyStream.Seek(0, SeekOrigin.Begin);
            var json = await new StreamReader(bodyStream).ReadToEndAsync();

            // Parse and verify
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            Assert.Equal(200, root.GetProperty("statusCode").GetInt32());
            Assert.Equal("Test error message", root.GetProperty("message").GetString());
            Assert.Equal("Something went wrong.", root.GetProperty("title").GetString());
        }

        [Fact]
        public async Task TryHandleAsync_Writes_Correct_Response_And_ReturnsFalse_CustomStatusCode()
        {
            // Arrange
            var handler = new GlobalExceptionHandler();
            var context = new DefaultHttpContext();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var exception = new InvalidOperationException("Ops!");
            using var bodyStream = new MemoryStream();
            context.Response.Body = bodyStream;

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            Assert.False(result);
            bodyStream.Seek(0, SeekOrigin.Begin);
            var json = await new StreamReader(bodyStream).ReadToEndAsync();

            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            Assert.Equal(500, root.GetProperty("statusCode").GetInt32());
            Assert.Equal("Ops!", root.GetProperty("message").GetString());
            Assert.Equal("Something went wrong.", root.GetProperty("title").GetString());
        }

        [Fact]
        public async Task TryHandleAsync_Honors_CancellationToken()
        {
            // Arrange
            var handler = new GlobalExceptionHandler();
            var context = new DefaultHttpContext();
            using var bodyStream = new MemoryStream();
            context.Response.Body = bodyStream;

            var exception = new Exception("Cancelled");
            using var cts = new CancellationTokenSource();
            cts.Cancel(); // Cancel immediately

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => handler.TryHandleAsync(context, exception, cts.Token).AsTask());
        }
    }
}