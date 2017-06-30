using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using ProjectName.Objects;

namespace ProjectName
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/objects"] = _ =>  {
        List<Object> AllObjects = Object.GetAll();
        return View["objects.cshtml", AllObjects];
      };
      Get["/categories"] = _ => {
        List<Category> AllCategories = Category.GetAll();
        return View["categories.cshtml", AllCategories];
      };

      //CREATE
      Get["/object/new"] = _ =>  {
        List<Category> AllCategories = Category.GetAll();
        return View["/object_add.cshtml", AllCategories];
      };
      Get["/category/new"] = _ =>  {
        return View["/category_add.cshtml"];
      };
      Post["/objects"]= _ =>  {
        Object newObject = new Object(Request.Form["name"], Request.Form["property"]);
        newObject.Save();
        return View["success.cshtml", newObject];
      };
      Post["/categories"]= _ => {
        Category newCategory = new Category(Request.Form["name"], Request.Form["property"]);
        newCategory.Save();
        return View["success.cshtml", newCategory];
      };

      //READ
      Get["/object/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var selectedObject = Object.Find(parameters.id);
        var objectCategories = selectedObject.GetCategories();
        model.Add("categories", objectCategories);
        model.Add("object", selectedObject);
        return View["object.cshtml", model];
      };
      Get["/category/{id}"]= parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var selectedCategory = Category.Find(parameters.id);
        var ObjectsCategory = selectedCategory.GetObjects();
        model.Add("category", selectedCategory);
        model.Add("objects", ObjectsCategory);
        return View["category.cshtml", model];
      };
      //UPDATE
      Get["/category/edit/{id}"] = parameters => {
        Category SelectedCategory = Category.Find(parameters.id);
        return View["category_edit.cshtml", SelectedCategory];
      };
      Patch["/category/edit/{id}"] = parameters =>{
        Category SelectedCategory = Category.Find(parameters.id);
        SelectedCategory.Update(Request.Form["name"], Request.Form["property"]);
        return View["success.cshtml"];
      };
      Get["/object/edit/{id}"] = parameters => {
        Object selectedObject = Object.Find(parameters.id);
        return View["object_edit.cshtml", selectedObject];
      };
      Patch["/object/edit/{id}"] = parameters =>{
        Object SelectedObject = Object.Find(parameters.id);
        SelectedObject.Update(Request.Form["name"], Request.Form["property"]);
        return View["success.cshtml"];
      };

      //DESTROY
      Get["category/delete/{id}"] = parameters => {
        Category SelectedCategory = Category.Find(parameters.id);
        return View["category_delete.cshtml", SelectedCategory];
      };
      Delete["category/delete/{id}"] = parameters => {
        Category SelectedCategory = Category.Find(parameters.id);
        SelectedCategory.Delete();
        return View["success.cshtml"];
      };
      Get["object/delete/{id}"] = parameters => {
        Object SelectedObject = Object.Find(parameters.id);
        return View["object_delete.cshtml", SelectedObject];
      };
      Delete["object/delete/{id}"] = parameters => {
        Object SelectedObject = Object.Find(parameters.id);
        SelectedObject.Delete();
        return View["success.cshtml"];
      };
    }
  }
}
