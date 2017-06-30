using Xunit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ProjectName.Objects;

namespace ProjectName
{
  [Collection("ProjectName")]
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=database_name_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Category.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Override_ObjectsAreEqual()
    {
      //Arrange, Act
      Category Category1 = new Category("params");
      Category Category2 = new Category("params");
      //Assert
      Assert.Equal(Category1, Category2);
    }

    [Fact]
     public void Test_Save_SavesToDatabase()
     {
      //Arrange
     Category testCategory = new Category("params");

      //Act
      testCategory.Save();
      List<Category> result =Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
     }

     [Fact]
    public void Test_Find_FindCategoryInDatabase()
    {
      //Arrange
      Category testCategory = new Category("params");
      testCategory.Save();

      //Act
      Category foundCategory = Category.Find(testCategory.GetId());

      //Assert
      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Test_Update_UpdatesCategoryInDatabase()
    {
      //Arrange
      Category testCategory = new Category("params");
      testCategory.Save();
      string newProperty = "params";
      //Act
      testCategory.Update("params");
      string result = testCategory.GetProperty();

      //Assert
      Assert.Equal(newProperty, result);
    }

    [Fact]
    public void GetObjects_ReturnsAllCategoryObject_ObjectList()
    {
     //Arrange
     Category testCategory = new Category("params");
     testCategory.Save();

     Object testObject1 = new Object("params");
     testObject1.Save();

     Object testObject2 = new Object("params");
     testObject2.Save();

     //Act
     testCategory.AddObject(testObject1);
     List<Object> savedObject = testCategory.GetObjects();
     List<Object> testList = new List<Object> {testObject1};

     //Assert
     Assert.Equal(testList, savedObject);
    }

    [Fact]
    public void Test_AddObject_AddsObjectToCategory()
    {
      //Arrange
      Category testCategory = new Category("params");
      testCategory.Save();

      Object testObject = new Object("params");
      testObject.Save();

      Object testObject2 = new Object("params");
      testObject2.Save();

      //Act
      testCategory.AddObject(testObject);
      testCategory.AddObject(testObject2);

      List<Object> result = testCategory.GetObjects();
      List<Object> testList = new List<Object>{testObject, testObject2};

      //Assert
      Assert.Equal(testList, result);
    }


    [Fact]
    public void Delete_DeletesCategoryAssociationsFromDatabase_CategoryList()
    {
      //Arrange
      Object testObject = new Object("params");
      testObject.Save();

      Category testCategory = new Category("params");
      testCategory.Save();

      //Act
      testCategory.AddObject(testObject);
      testCategory.Delete();

      List<Category> resultObjectCategory = testObject.GetCategories();
      List<Category> testObjectCategory = new List<Category> {};

      //Assert
      Assert.Equal(testObjectCategory, resultObjectCategory);
    }

    public void Dispose()
    {
      Object.DeleteAll();
      Category.DeleteAll();
    }
  }
}
