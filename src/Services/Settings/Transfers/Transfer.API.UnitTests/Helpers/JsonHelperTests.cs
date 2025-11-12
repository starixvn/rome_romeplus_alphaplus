using AlphaPlus.Settings.Transfer.API.Helpers;
using Newtonsoft.Json;

namespace AlphaPlus.Settings.Transfer.API.UnitTests.Helpers;

public class JsonHelperTests
{
    [Fact]
    public void ToJsonFunc_Should_Return_Default_ToJson_Function_When_Not_Set()
    {
        // Arrange
        JsonHelper.ToJsonFunc = null; // Reset to default

        // Act
        var func = JsonHelper.ToJsonFunc;

        // Assert
        Assert.NotNull(func);
        
        // Test that it works with actual serialization
        var testObj = new { Name = "Test", Value = 123 };
        var result = func(testObj);
        var expected = JsonConvert.SerializeObject(testObj);
        
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToJsonFunc_Should_Allow_Custom_Function_To_Be_Set()
    {
        // Arrange
        var customFunc = new Func<object, string>(obj => $"Custom: {obj}");
        
        try
        {
            // Act
            JsonHelper.ToJsonFunc = customFunc;
            var retrievedFunc = JsonHelper.ToJsonFunc;

            // Assert
            Assert.Same(customFunc, retrievedFunc);
            
            var testObj = "TestValue";
            var result = retrievedFunc(testObj);
            Assert.Equal("Custom: TestValue", result);
        }
        finally
        {
            // Cleanup - Reset to default
            JsonHelper.ToJsonFunc = null;
        }
    }

    [Fact]
    public async Task ToJsonFunc_Should_Be_Thread_Safe()
    {
        // Arrange
        var results = new List<string>();
        var tasks = new List<Task>();
        var lockObject = new object();
        
        try
        {
            // Act - Set different functions on different threads
            for (int i = 0; i < 5; i++)
            {
                var threadId = i;
                tasks.Add(Task.Run(() =>
                {
                    JsonHelper.ToJsonFunc = obj => $"Thread{threadId}: {obj}";
                    var func = JsonHelper.ToJsonFunc;
                    var result = func("test");
                    
                    lock (lockObject)
                    {
                        results.Add(result);
                    }
                }));
            }

            await Task.WhenAll(tasks.ToArray());

            // Assert
            Assert.Equal(5, results.Count);
            
            // Each thread should have its own function due to [ThreadStatic]
            foreach (var result in results)
            {
                Assert.StartsWith("Thread", result);
                Assert.EndsWith(": test", result);
            }
        }
        finally
        {
            // Cleanup - Reset to default
            JsonHelper.ToJsonFunc = null;
        }
    }

    [Fact]
    public void ToJson_Internal_Method_Should_Serialize_Object_Correctly()
    {
        // Arrange
        var testObject = new TestClass
        {
            Id = 1,
            Name = "Test Name",
            IsActive = true,
            CreatedDate = new DateTime(2024, 1, 1, 12, 0, 0)
        };

        // Act
        var result = JsonHelper.ToJson(testObject);

        // Assert
        var expected = JsonConvert.SerializeObject(testObject);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToJson_Internal_Method_Should_Handle_Null_Object()
    {
        // Act
        var result = JsonHelper.ToJson(null);

        // Assert
        Assert.Equal("null", result);
    }

    [Fact]
    public void ToJson_Internal_Method_Should_Handle_Primitive_Types()
    {
        // Test string
        var stringResult = JsonHelper.ToJson("test string");
        Assert.Equal("\"test string\"", stringResult);

        // Test integer
        var intResult = JsonHelper.ToJson(42);
        Assert.Equal("42", intResult);

        // Test boolean
        var boolResult = JsonHelper.ToJson(true);
        Assert.Equal("true", boolResult);

        // Test decimal
        var decimalResult = JsonHelper.ToJson(123.45m);
        Assert.Equal("123.45", decimalResult);
    }

    [Fact]
    public void ToJson_Internal_Method_Should_Handle_Collections()
    {
        // Arrange
        var list = new List<string> { "item1", "item2", "item3" };
        var array = new[] { 1, 2, 3, 4, 5 };
        var dictionary = new Dictionary<string, int>
        {
            { "key1", 1 },
            { "key2", 2 }
        };

        // Act
        var listResult = JsonHelper.ToJson(list);
        var arrayResult = JsonHelper.ToJson(array);
        var dictResult = JsonHelper.ToJson(dictionary);

        // Assert
        var expectedList = JsonConvert.SerializeObject(list);
        var expectedArray = JsonConvert.SerializeObject(array);
        var expectedDict = JsonConvert.SerializeObject(dictionary);

        Assert.Equal(expectedList, listResult);
        Assert.Equal(expectedArray, arrayResult);
        Assert.Equal(expectedDict, dictResult);
    }

    [Fact]
    public void ToJson_Internal_Method_Should_Handle_Complex_Nested_Objects()
    {
        // Arrange
        var complexObject = new
        {
            User = new TestClass
            {
                Id = 1,
                Name = "John Doe",
                IsActive = true,
                CreatedDate = new DateTime(2024, 1, 1)
            },
            Settings = new
            {
                Theme = "Dark",
                Language = "en-US",
                Preferences = new[] { "setting1", "setting2" }
            },
            Metadata = new Dictionary<string, object>
            {
                { "version", "1.0" },
                { "build", 123 },
                { "features", new[] { "feature1", "feature2" } }
            }
        };

        // Act
        var result = JsonHelper.ToJson(complexObject);

        // Assert
        var expected = JsonConvert.SerializeObject(complexObject);
        Assert.Equal(expected, result);
        
        // Verify it's valid JSON by deserializing
        var deserialized = JsonConvert.DeserializeObject(result);
        Assert.NotNull(deserialized);
    }

    [Fact]
    public void ToJsonFunc_Default_Should_Handle_Exception_In_Serialization()
    {
        // Arrange
        var problematicObject = new CircularReferenceClass();
        problematicObject.Self = problematicObject; // Create circular reference

        // Act & Assert
        var exception = Assert.Throws<JsonSerializationException>(() => JsonHelper.ToJson(problematicObject));
        Assert.Contains("Self referencing loop", exception.Message);
    }

    // Test helper classes
    private class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    private class CircularReferenceClass
    {
        public CircularReferenceClass Self { get; set; }
    }
}
