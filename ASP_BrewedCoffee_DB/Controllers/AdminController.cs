﻿using Microsoft.AspNetCore.Mvc;

namespace ASP_BrewedCoffee_DB.Controllers;
public class AdminController : Controller
{
    public CPosts PostsModel;
    public CCategories CategoriesModel;
    public CAuth AuthModel;

    public AdminController(CAuth auth, CCategories cats, CPosts posts)
    {
        AuthModel = auth;
        PostsModel = posts;
        CategoriesModel = cats;
    }
    //---------------------------------------------------------------------
    public bool IsAdmin() => AuthModel.CheckAuth(HttpContext);
    // main admin page
    [Route("/admin")]
    public IActionResult Index() => IsAdmin() ? Redirect("/admin/posts") : View();
    // auth admin control post request
    [HttpPost]
    [Route("/admin")]
    public IActionResult Index(RAuthData data) => AuthModel.Authorization(HttpContext, data) ? Redirect("/admin/posts") : View();
    
    [Route("/admin/logout")]
    public IActionResult LogOut() 
    {
        HttpContext.Response.Cookies.Append("is_auth", "false");
        return Redirect("/admin");
    }
    //---------------------------------------------------------------------
    // all posts table
    [Route("/admin/posts")]
    public IActionResult Posts() => IsAdmin() ? View(PostsModel.GetPosts()) : Redirect("/admin");

    // all categories table
    [Route("/admin/categories")]
    public IActionResult Categories() => IsAdmin() ? View(CategoriesModel.GetCats()) : Redirect("/admin");
        
    //---------------------------------------------------

    // тут пользователь идет по пути с возможностью добавления
    // айди. если айди есть - форма заполняется данными во вьюшке. Если нет - приходит пустая форма для 
    // заполнения. - добавление нового поста
    [Route("/admin/posts/post/{id?}")]
    public IActionResult EditPost(int? id) => IsAdmin() ? View(id == null ? new CPost() : PostsModel.GetPost(id)) : Redirect("/admin");

    // тут будет только после пост запроса отрабатывать этот метод. т.е. уже принимать заполненную
    // форму с новым постом, который добавится в базу данных. Если присутствует айди - 
    // в базе данных меняются даные поста по этому айдишнику.
    [HttpPost]
    [Route("/admin/posts/post/{id?}")]
    public IActionResult EditPost(CPost post, int? id)
    {
        if (!IsAdmin()) Redirect("/admin");
        if (id == null) PostsModel.Add(post);
        else PostsModel.Edit(post, id.Value);

        return View(post);
    }

    // тут пользователь идет по пути с возможностью добавления
    // айди. если айди есть - форма заполняется данными во вьюшке. Если нет - приходит пустая форма для 
    // заполнения. - добавление новой категории
    [Route("/admin/categories/category/{id?}")]   
    public IActionResult EditCategory(int? id) => IsAdmin() ? View(id == null ? new CCategory() : CategoriesModel.GetCat(id)) : Redirect("/admin");

    // тут будет только после пост запроса отрабатывать этот метод. т.е. уже принимать заполненную
    // форму с новой категорией, которая добавится в базу данных. Если присутствует айди - 
    // в базе данных меняются даные категории по этому айдишнику.
    [HttpPost]
    [Route("/admin/categories/category/{id?}")]
    public IActionResult EditCategory(CCategory category, int? id)
    {
        if (!IsAdmin()) Redirect("/admin");
        if (id == null) CategoriesModel.Add(category);
        else CategoriesModel.Edit(category, id.Value);

        return View(category);
    }
}
