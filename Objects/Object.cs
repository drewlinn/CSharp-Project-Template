using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ProjectName.Objects
{
  public class Object
  {
    private int _id;
    private string _name;
    private string _property;

    public Object(string name, string property, int id = 0)
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


    public override bool Equals(System.Object otherObject)
    {
      if(!(otherObject is Object))
      {
        return false;
      }
      else
      {
        Object newObject = (Object) otherObject;
        bool idEquality = (this.GetId() == newObject.GetId());
        bool nameEquality = (this.GetName() == newObject.GetName());
        bool propertyEquality = (this.GetProperty() == newObject.GetProperty());
        return (idEquality && );
      }
    }

    public override int GetHashCode()
    {
      return this.GetHashCode();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO objects (name, property) OUTPUT INSERTED.id VALUES (@name, @property);", conn);

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

    public static Object Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM objects WHERE id = @id;", conn);
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
      Object foundObject = new Object(name, property, foundId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundObject;
    }

    public void Update(string name, string property)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE objects SET name = @name, property = @property WHERE id = @Id;", conn);

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

    public List<Category> GetCategories()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT categories.* FROM objects JOIN categories_objects ON (objects.id = categories_objects.object_id) JOIN categories ON (categories_objects.category_id = categories.id) WHERE objects.id = @ObjectsId;", conn);
      SqlParameter objectsIdParam = new SqlParameter("@ObjectsId", this.GetId().ToString());

      cmd.Parameters.Add(objectsIdParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<Category> categories = new List<Category>{};

      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string name = rdr.GetString(1);
        string address = rdr.GetString(2);
        Category newCategory = new Category(name, address, categoryId);
        categories.Add(newCategory);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return categories;
    }

    public void AddCategory(Category newCategory)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO categories_objects (object_id, category_id) VALUES (@ObjectId, @CategoryId);", conn);

      SqlParameter objectIdParameter = new SqlParameter("@ObjectId", this.GetId());
      SqlParameter categoryIdParameter = new SqlParameter( "@CategoryId", newCategory.GetId());

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

      SqlCommand cmd = new SqlCommand("DELETE FROM objects WHERE id = @objectId; DELETE FROM categories_objects WHERE object_id = @objectId;", conn);
      SqlParameter objectIdParameter = new SqlParameter("@objectId", this.GetId());

      cmd.Parameters.Add(objectIdParameter);
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
     SqlCommand cmd = new SqlCommand("DELETE FROM objects;", conn);
     cmd.ExecuteNonQuery();
     conn.Close();
    }
  }
}
