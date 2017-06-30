using Xunit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ProjectName.Objects;

namespace ProjectName
{
  [Collection("ProjectName")]
  public class ObjectTest : IDisposable
  {
    public ObjectTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=database_name_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Object.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Override_ObjectsAreEqual()
    {
      //Arrange, Act
      Object Object1 = new Object("params");
      Object Object2 = new Object("params");
      //Assert
      Assert.Equal(Object1, Object2);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Object testObject = new Object("params");

      //Act
      testObject.Save();
      List<Object> result =Object.GetAll();
      List<Object> testList = new List<Object>{testObject};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
      public void Test_Find_FindObjectInDatabase()
      {
      //Arrange
      Object testObject = new Object("params");
      testObject.Save();

      //Act
      Object foundObject = Object.Find(testObject.GetId());

      //Assert
      Assert.Equal(testObject, foundObject);
    }

    [Fact]
    public void Test_Update_UpdatesObjectInDatabase()
    {
      //Arrange
      Object testObject = new Object("params");
      testObject.Save();
      string newProperty = "params";
      //Act
      testObject.Update("params");
      string result = testObject.GetProperty();

      //Assert
      Assert.Equal(newProperty, result);
    }

    [Fact]
    public void GetCategories_ReturnsAllObjectCategories_CategoryList()
    {
      //Arrange
      Object testObject = new Object("params");
      testObject.Save();

      Category testCategories1 = new Category("params");
      testCategories1.Save();

      Category testCategories2 = new Category("params");
      testCategories2.Save();

      //Act
      testObject.AddCategory(testCategories1);
      List<Category> result = testObject.GetCategories();
      List<Category> testList = new List<Category> {testCategories1};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void AddCategory_AddsCategoriesToObject_CategoriesList()
    {
      //Arrange
      Object testObject = new Object("params");
      testObject.Save();

      Category testCategories = new Category("params");
      testCategories.Save();

      //Act
      testObject.AddCategory(testCategories);

      List<Category> result = testObject.GetCategories();
      List<Category> testList = new List<Category>{testCategories};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Delete_DeletesObjectsAssociationsFromDatabase_ObjectsList()
    {
      //Arrange
      Category testCategory = new Category("params");
      testCategory.Save();

      Object testObjects = new Object("params");
      testObjects.Save();

      //Act
      testObjects.AddCategory(testCategory);
      testObjects.Delete();

      List<Object> resultCategoryObjects = testCategory.GetObjects();
      List<Object> testCategoryObjects = new List<Object> {};

      //Assert
      Assert.Equal(testCategoryObjects, resultCategoryObjects);
    }

    public void Dispose()
    {
      Object.DeleteAll();

    }
  }
}
