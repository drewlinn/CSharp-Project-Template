using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ProjectName.Objects
{
  public class Object
  {
    private int _id;
    

    public Object( , int id = 0)
    {
      _id = id;
      
    }
    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    

    public override bool Equals(System.Object otherInventory)
    {
      if(!(otherInventory is Inventory))
      {
        return false;
      }
      else
      {
        Inventory newInventory = (Inventory) otherInventory;
        bool idEquality = (this.GetId() == newInventory.GetId());

        bool equipmentEquality = (this.GetEquipment() == newInventory.GetEquipment());
        return (idEquality && );
      }
    }

    public override int GetHashCode()
    {
      return this.GetHashCode();
    }
  }
}
