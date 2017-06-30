using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ProjectName.Objects
{
  public class Category
  {
    private int _id;
    private string _name;
    private string _property;

    public Category (string name, string property, int id = 0)
    {
      _id = id;
      _name = name;
      _property = property;
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName()
    {
      _name = newName;
    }
    public string GetProperty()
    {
      return _property;
    }
    public string SetProperty()
    {
      _property = newProperty;
    }

    public static List<Category> GetAll()
    {
      List<Category> AllCategories = new List<Category>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string property = rdr.GetString(2);
        Category newCategory = new Category(name, property, id);
        AllCategories.Add(newCategory);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return AllCategories;
    }

    public override bool Equals(System.Object otherCategory)
   {
    if(!(otherCategory is Category))
    {
      return false;
    }
    else
     {
      Category newCategory = (Category) otherCategory;
      bool idEquality = (this.GetId() == newCategory.GetId());
      bool nameEquality = (this.GetName() == newCategory.GetName());
      bool propertyEquality = (this.GetProperty() == newCategory.GetProperty());
      return (idEquality && nameEquality);
     }
   }

   public override int GetHashCode()
   {
     return this.GetName().GetHashCode();
   }

   public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories (name, property) OUTPUT INSERTED.id VALUES (@name, @property);", conn);

      SqlParameter namePara = new SqlParameter("@name", this.GetName());
      SqlParameter propertyPara = new SqlParameter("@property", this.GetProperty());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(propertyPara);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Category Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM categories WHERE id = @id;", conn);
      SqlParameter idParameter = new SqlParameter("@id", id.ToString());

      cmd.Parameters.Add(idParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundId = 0;
      string name = null;
      string property = null;

      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        name = rdr.GetString(1);
        property = rdr.GetString(2);
      }
      Category foundCategory = new Category(name, property, foundId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCategory;
    }

    public void Update(string name, string property)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE categories SET name = @name, property = @property WHERE id = @Id;", conn);

      SqlParameter namePara = new SqlParameter("@name", name);
      SqlParameter propertyPara = new SqlParameter("@property", property);
      SqlParameter idPara = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(namePara);
      cmd.Parameters.Add(propertyPara);
      cmd.Parameters.Add(idPara);

      this._name = name;
      this._property = property;
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public List<Object> GetObjects()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT objects.* FROM categories JOIN categories_objects ON (categories.id = categories_objects.category_id) JOIN objects ON (categories_objects.object_id = objects.id) WHERE categories.id = @categoryId;", conn);
      SqlParameter categoryIdParam = new SqlParameter("@categoryId", this.GetId().ToString());

      cmd.Parameters.Add(categoryIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Object> objects = new List<Object>{};

      while(rdr.Read())
      {
        int objectId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string property = rdr.GetString(2);
        Object newObject = new Object(name, property, objectId);
        objects.Add(newObject);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return objects;
    }

    //Add object's id and category's id to categories_objects table
    public void AddObject(Object newObject)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories_objects (object_id, category_id) VALUES (@ObjectId, @CategoryId);", conn);

      SqlParameter objectIdParameter = new SqlParameter( "@ObjectId", newObject.GetId());
      SqlParameter categoryIdParameter = new SqlParameter("@CategoryId", this.GetId());

      cmd.Parameters.Add(objectIdParameter);
      cmd.Parameters.Add(categoryIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM categories WHERE id = @Id; DELETE FROM categories_objects WHERE category_id = @Id;", conn);
      SqlParameter IdParameter = new SqlParameter("@Id", this.GetId());

      cmd.Parameters.Add(IdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM categories;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
